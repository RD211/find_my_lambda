using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Models;
using Action = Server.Models.Action;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class LambdaController
{

    private readonly IConfiguration _config;
    private readonly ILogger<LambdaController> _logger;
    private readonly string _connectionString;
    public LambdaController(ILogger<LambdaController> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = config["Database:DataSource"],
            UserID = config["Database:UserId"],
            Password = config["Database:Password"],
            InitialCatalog = config["Database:Catalog"]
        };

        _connectionString = builder.ConnectionString;
    }
    
    
    [HttpGet(Name = "{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Lambda))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetLambda([FromQuery] int id)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            connection.Open();

            var sql = $"SELECT * FROM dbo.lambdas WHERE id = @id;";

            using var command = new SqlCommand(sql, connection);
            command.Prepare();
            command.Parameters.AddWithValue("@id", id);
            
            using var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                _logger.Log(LogLevel.Information, 
                    "Couldn't find lambda function with id: " + id);
                return new NotFoundResult(); 
            }
            
            reader.Read();
            return new OkObjectResult(Lambda.ReadLambda(reader));
        }
        catch (SqlException e)
        {
            _logger.Log(LogLevel.Error, e.Message);
            return new NotFoundResult();
        }
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UploadLambda([FromBody] LambdaPayload lambdaPayload)
    {
        try
        {
            var payload = new EvaluatePayload(
                Action.Verify, 
                lambdaPayload.ProgrammingLanguage, 
                null,
                lambdaPayload.Code);
            
            var resultVerify = SendToLanguageServer(payload);

            if (resultVerify.Result == Result.FAILURE || 
                (resultVerify.Result == Result.OK && resultVerify.Payload!.ToString() == "false"))
            {
                return new BadRequestResult();
            }

            payload.Action = Action.Types;
            var resultTypes = SendToLanguageServer(payload);
            
            
            if (resultTypes.Result == Result.FAILURE)
            {
                return new BadRequestResult();
            }

            var types = resultTypes.Payload as JArray;
            
            
            using var connection = new SqlConnection(_connectionString);
            
            connection.Open();

            const string sql = 
                "INSERT INTO dbo.lambdas(programming_language,code,input_type,return_type,upload_date,times_used) " +
                "VALUES(@language, @code, @input, @return, @date, 0);";

            using var command = new SqlCommand(sql, connection);
            command.Prepare();
            command.Parameters.AddWithValue("@language", payload.Language);
            command.Parameters.AddWithValue("@code", payload.Code);
            command.Parameters.AddWithValue("@input", types?[0]);
            command.Parameters.AddWithValue("@return", types?[1]);
            command.Parameters.AddWithValue("@date", DateTime.Now);


            using var reader = command.ExecuteReader();
            reader.Read();
            return new OkResult();
        }
        catch (SqlException e)
        {
            _logger.Log(LogLevel.Error, e.Message);
            return new NotFoundResult();
        }
    }
    
    [HttpPost("search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Lambda>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SearchLambda([FromBody] SearchPayload searchPayload)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            connection.Open();

            var sql = $"SELECT * FROM dbo.lambdas WHERE input_type = @input_type ORDER BY times_used;";
            using var command = new SqlCommand(sql, connection);
            
            command.Prepare();
            command.Parameters.AddWithValue("@input_type", "(int)");
            
            using var reader = command.ExecuteReader();

            var found = new List<Lambda>();
            while (reader.Read())
            {
                found.Add(new Lambda(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetDateTime(5),
                    reader.GetInt32(6)
                ));
            }

            return new OkObjectResult(found.Where(lambda => CheckMethod(lambda, searchPayload)));
        }
        catch (SqlException e)
        {
            _logger.Log(LogLevel.Error, e.Message);
            return new NotFoundResult();
        }
    }

    private bool CheckMethod(Lambda lambda, SearchPayload searchPayload)
    {
        var results = searchPayload.Inputs.Select(s => GetResultOfInput(lambda, s)).ToArray();
        return results.SequenceEqual(searchPayload.Results);
    }

    private void CacheResult(Lambda lambda, string input, string output)
    {
        using var connection = new SqlConnection(_connectionString);
            
        connection.Open();

        var sql = $"INSERT INTO cached_results(lambda_id, input, result) VALUES(@id, @input, @result)";
        using var command = new SqlCommand(sql, connection);
        
        command.Prepare();
        command.Parameters.AddWithValue("@id", lambda.Id);
        command.Parameters.AddWithValue("@input", input);
        command.Parameters.AddWithValue("@result", output);

        using var reader = command.ExecuteReader();
    }

    private string? GetResultOfInput(Lambda lambda, string input)
    {
        using var connection = new SqlConnection(_connectionString);
            
        connection.Open();

        var sql = $"SELECT * FROM dbo.cached_results WHERE lambda_id = @id AND input = @input;";
        using var command = new SqlCommand(sql, connection);
        
        command.Prepare();
        command.Parameters.AddWithValue("@id", lambda.Id);
        command.Parameters.AddWithValue("@input", input);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return reader.GetString(2);
        }

        var result = SendToLanguageServer(new EvaluatePayload(
            Action.Evaluate, lambda.ProgrammingLanguage, new[] { input }, lambda.Code));
        
        if (result.Result == Result.FAILURE)
            return null;
        
        var output = (result.Payload as JArray)?[0].Value<string>();
        output = output?.Substring(1, output.Length - 2);
        CacheResult(lambda, input, output!);
        
        return output;
    }

    private static ResponsePayload SendToLanguageServer(EvaluatePayload evaluatePayload)
    {
        var bytes = new byte[2048];
        var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        var ipAddress = ipHostInfo.AddressList[0];
        var remoteEp = new IPEndPoint(new IPAddress(new byte[]{127,0,0,1}), 11000);

        var sender = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        sender.Connect(remoteEp);

        var msg = Encoding.ASCII.GetBytes($"{JsonConvert.SerializeObject(evaluatePayload)}<EOF>");

        sender.Send(msg);

        var bytesRec = sender.Receive(bytes);
        var response =
            JsonConvert.DeserializeObject<ResponsePayload>(Encoding.ASCII.GetString(bytes, 0, bytesRec));

        if (response is null)
        {
            throw new Exception("Something went wrong while serializing the response.");
        }

        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
        return response;
    }
}
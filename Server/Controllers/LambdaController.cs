using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Server.Communication;
using Server.Models;
using Server.Searcher;
using Server.SQL;
using Action = Server.Models.Action;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class LambdaController
{

    private readonly IConfiguration _config;
    private readonly ILogger<LambdaController> _logger;
    private readonly LambdaDatabase _lambdaDatabase;
    private readonly LambdaFinder _lambdaFinder;

    public LambdaController(ILogger<LambdaController> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
        _lambdaDatabase = new LambdaDatabase(_config);
        _lambdaFinder = new LambdaFinder(_lambdaDatabase);
    }
    
    
    [HttpGet(Name = "{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Lambda))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetLambda([FromQuery] int id)
    {
        try
        {
            var lambda = _lambdaDatabase.GetLambdaById(id);
            if (lambda is null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(lambda);
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
            
            var resultVerify = LanguageServer.Send(payload);

            if (resultVerify.Result == Result.FAILURE || 
                (resultVerify.Result == Result.OK && resultVerify.Payload!.ToString() == "false"))
            {
                return new BadRequestResult();
            }

            payload.Action = Action.Types;
            var resultTypes = LanguageServer.Send(payload);
            
            
            if (resultTypes.Result == Result.FAILURE)
            {
                return new BadRequestResult();
            }

            var types = resultTypes.Payload as JArray;
            
            
            var result = _lambdaDatabase.InsertLambda(
                lambdaPayload.ProgrammingLanguage ?? throw new InvalidOperationException(), 
                lambdaPayload.Code, 
                types?[0].Value<string>() ?? throw new ArgumentException("Function has unsupported parameter types."),
                types[1].Value<string>() ?? throw new ArgumentException("Function has unsupported return type.")
            );

            if (result)
            {
                return new OkResult();
            }

            return new NotFoundResult();
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
            // TODO: dynamically figure out the input type.
            var lamdbas = _lambdaDatabase.GetLambdasByInputType("(int)");

            return new OkObjectResult(lamdbas.Where(lambda => _lambdaFinder.CheckMethod(lambda, searchPayload)));
        }
        catch (SqlException e)
        {
            _logger.Log(LogLevel.Error, e.Message);
            return new NotFoundResult();
        }
    }



    
}
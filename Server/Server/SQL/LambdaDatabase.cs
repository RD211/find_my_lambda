using System.Data.SqlClient;
using Microsoft.OpenApi.Extensions;
using Server.Models;

namespace Server.SQL;

public class LambdaDatabase
{
    private enum SqlOperation{
        CacheResult,
        GetFogById,
        GetFogByInputType,
        GetFogMembers,
        GetLambdaById,
        GetLambdaByInputType,
        GetResultByInputAndFog,
        GetResultsByInput,
        InsertFog,
        InsertFogMember,
        InsertLambda,
        GetSelfFogOfLambda,
    }

    private readonly Dictionary<SqlOperation, string> _sql;

    private readonly string _connectionString;
    public LambdaDatabase(IConfiguration config)
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = config["Database:DataSource"],
            UserID = config["Database:UserId"],
            Password = config["Database:Password"],
            InitialCatalog = config["Database:Catalog"]
        };

        _connectionString = builder.ConnectionString;
        
        _sql = Enum.GetValues(typeof(SqlOperation))
            .Cast<SqlOperation>()
            .ToDictionary(op => op, GetSqlQuery);
    }

    public Lambda? GetLambdaById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
            
        connection.Open();

        var sql = _sql[SqlOperation.GetLambdaById];
        using var command = new SqlCommand(sql, connection);
        
        command.Prepare();
        command.Parameters.AddWithValue("@id", id);
            
        using var reader = command.ExecuteReader();
        if (!reader.HasRows)
        {
            return null; 
        }
            
        reader.Read();
        return Lambda.ReadLambda(reader);
    }
    
    public Fog? GetFogById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
            
        connection.Open();

        var sql = _sql[SqlOperation.GetFogById];
        using var command = new SqlCommand(sql, connection);
        
        command.Prepare();
        command.Parameters.AddWithValue("@id", id);
            
        using var reader = command.ExecuteReader();
        if (!reader.HasRows)
        {
            return null; 
        }
            
        reader.Read();
        return Fog.ReadFog(reader);
    }

    public int InsertLambda(string name, string description, string email, string language, string code, string input, string output)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();

            var sql = _sql[SqlOperation.InsertLambda];

            using var command = new SqlCommand(sql, connection);
            command.Prepare();
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@language", language);
            command.Parameters.AddWithValue("@code", code);
            command.Parameters.AddWithValue("@input", input);
            command.Parameters.AddWithValue("@return", output);
            command.Parameters.AddWithValue("@date", DateTime.Now);

            var lambdaId = (int)command.ExecuteScalar();
            InsertFog(new List<int> { lambdaId });
            return lambdaId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    public int InsertFog(IEnumerable<int> lambdas)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();

            var sql = _sql[SqlOperation.InsertFog];

            var lambdaList = lambdas.ToList();
            
            var first = GetLambdaById(lambdaList.First())!;
            var last = GetLambdaById(lambdaList.Last())!;
            using var command = new SqlCommand(sql, connection);
            command.Prepare();
            command.Parameters.AddWithValue("@input_type", first.InputType);
            command.Parameters.AddWithValue("@return_type", last.ReturnType);
            command.Parameters.AddWithValue("@member_count", lambdaList.Count);

            var fogId = (int)command.ExecuteScalar();

            for (var i = 0; i < lambdaList.Count; i++)
            {
                InsertFogMember(fogId, lambdaList[i],i);
            }

            return fogId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    private void InsertFogMember(int fogId, int lambdaId, int position)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();

            var sql = _sql[SqlOperation.InsertFogMember];

            using var command = new SqlCommand(sql, connection);
            command.Prepare();
            command.Parameters.AddWithValue("@fog_id", fogId);
            command.Parameters.AddWithValue("@lambda_id", lambdaId);
            command.Parameters.AddWithValue("@position", position);

            using var reader = command.ExecuteReader();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public IEnumerable<Lambda> GetLambdasByInputType(string inputType)
    {
        using var connection = new SqlConnection(_connectionString);
            
        connection.Open();

        var sql = _sql[SqlOperation.GetLambdaByInputType];
        using var command = new SqlCommand(sql, connection);
            
        command.Prepare();
        command.Parameters.AddWithValue("@input_type", inputType);
            
        using var reader = command.ExecuteReader();

        var found = new List<Lambda>();
        while (reader.Read())
        {
            found.Add(Lambda.ReadLambda(reader));
        }

        return found;
    }
    
    public IEnumerable<Fog> GetFogsByInputType(string inputType)
    {
        using var connection = new SqlConnection(_connectionString);
            
        connection.Open();

        var sql = _sql[SqlOperation.GetFogByInputType];
        using var command = new SqlCommand(sql, connection);
            
        command.Prepare();
        command.Parameters.AddWithValue("@input_type", inputType);
            
        using var reader = command.ExecuteReader();

        var found = new List<Fog>();
        while (reader.Read())
        {
            found.Add(Fog.ReadFog(reader));
        }

        return found;
    }

    public IEnumerable<int> GetFogMembers(int fogId)
    {
        using var connection = new SqlConnection(_connectionString);
            
        connection.Open();

        var sql = _sql[SqlOperation.GetFogMembers];
        using var command = new SqlCommand(sql, connection);
            
        command.Prepare();
        command.Parameters.AddWithValue("@id", fogId);
            
        using var reader = command.ExecuteReader();

        var found = new List<int>();
        while (reader.Read())
        {
            found.Add(reader.GetInt32(0));
        }

        return found;
    }

    public void CacheResult(int fogId, string input, string output)
    {
        using var connection = new SqlConnection(_connectionString);
            
        connection.Open();

        var sql = _sql[SqlOperation.CacheResult];
        
        using var command = new SqlCommand(sql, connection);
        
        command.Prepare();
        command.Parameters.AddWithValue("@id", fogId);
        command.Parameters.AddWithValue("@input", input);
        command.Parameters.AddWithValue("@result", output);
        
        using var reader = command.ExecuteReader();
    }

    public string? GetResultByInputAndFog(int fogId, string input)
    {
        using var connection = new SqlConnection(_connectionString);
            
        connection.Open();

        var sql = _sql[SqlOperation.GetResultByInputAndFog];
        using var command = new SqlCommand(sql, connection);
        
        command.Prepare();
        command.Parameters.AddWithValue("@id", fogId);
        command.Parameters.AddWithValue("@input", input);

        using var reader = command.ExecuteReader();

        return reader.Read() ? reader.GetString(2) : null;
    }
    
    public List<(int, string)> GetResultsByInput(string input)
    {
        using var connection = new SqlConnection(_connectionString);
            
        connection.Open();

        var sql = _sql[SqlOperation.GetResultsByInput];
        using var command = new SqlCommand(sql, connection);
        
        command.Prepare();
        command.Parameters.AddWithValue("@input", input);

        using var reader = command.ExecuteReader();

        var res = new List<(int, string)>();
        while (reader.Read())
        {
            res.Add((reader.GetInt32(0), reader.GetString(2)));
        }

        return res;
    }
    
    public Fog GetSelfFogOfLambda(int lambdaId)
    {
        using var connection = new SqlConnection(_connectionString);
            
        connection.Open();

        var sql = _sql[SqlOperation.GetSelfFogOfLambda];
        using var command = new SqlCommand(sql, connection);
            
        command.Prepare();
        command.Parameters.AddWithValue("@lambda_id", lambdaId);
            
        using var reader = command.ExecuteReader();

        reader.Read();

        return Fog.ReadFog(reader);
    }

    private static string GetSqlQuery(SqlOperation operation)
    {
        return File.ReadAllText("./SQL/Queries/" + operation.GetDisplayName() + ".sql");
    }
}
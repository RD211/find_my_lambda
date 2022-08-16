using System.Data.SqlClient;
using Microsoft.OpenApi.Extensions;
using Server.Models;

namespace Server.SQL;

public class LambdaDatabase
{
    private enum SqlOperation{
        GetById,
        GetByInputType,
        CacheResult,
        GetResultByInput
    }
    
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
    }

    public Lambda? GetLambdaById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
            
        connection.Open();

        var sql = GetSqlQuery(SqlOperation.GetById);
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

    public bool InsertLambda(string language, string code, string input, string output)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();

            const string sql =
                "INSERT INTO dbo.lambdas(programming_language,code,input_type,return_type,upload_date,times_used) " +
                "VALUES(@language, @code, @input, @return, @date, 0);";

            using var command = new SqlCommand(sql, connection);
            command.Prepare();
            command.Parameters.AddWithValue("@language", language);
            command.Parameters.AddWithValue("@code", code);
            command.Parameters.AddWithValue("@input", input);
            command.Parameters.AddWithValue("@return", output);
            command.Parameters.AddWithValue("@date", DateTime.Now);


            using var reader = command.ExecuteReader();
            reader.Read();
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    public List<Lambda> GetLambdasByInputType(string inputType)
    {
        using var connection = new SqlConnection(_connectionString);
            
        connection.Open();

        var sql = GetSqlQuery(SqlOperation.GetByInputType);
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

    public void CacheResult(int lambdaId, string input, string output)
    {
        using var connection = new SqlConnection(_connectionString);
            
        connection.Open();

        var sql = GetSqlQuery(SqlOperation.CacheResult);
        
        using var command = new SqlCommand(sql, connection);
        
        command.Prepare();
        command.Parameters.AddWithValue("@id", lambdaId);
        command.Parameters.AddWithValue("@input", input);
        command.Parameters.AddWithValue("@result", output);

        using var reader = command.ExecuteReader();
    }

    public string? GetResultByInput(int lambdaId, string input)
    {
        using var connection = new SqlConnection(_connectionString);
            
        connection.Open();

        var sql = GetSqlQuery(SqlOperation.GetResultByInput);
        using var command = new SqlCommand(sql, connection);
        
        command.Prepare();
        command.Parameters.AddWithValue("@id", lambdaId);
        command.Parameters.AddWithValue("@input", input);

        using var reader = command.ExecuteReader();

        return reader.Read() ? reader.GetString(2) : null;
    }

    private string GetSqlQuery(SqlOperation operation)
    {
        return File.ReadAllText("./SQL/Queries/" + operation.GetDisplayName() + ".sql");
    }
}
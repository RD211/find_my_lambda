using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Server.Communication;
using Server.Lambda_Input;
using Server.Parser;
using Server.SQL;

namespace Server.Models;

public class Lambda
{
    public Lambda(int id, string name, string description, string email, string programmingLanguage, string code, string inputType, string returnType, DateTime uploadDate, int timesUsed, bool confirmed, bool verified, int likes)
    {
        Id = id;
        Name = name;
        Description = description;
        Email = email;
        ProgrammingLanguage = programmingLanguage;
        Code = code;
        InputType = inputType;
        ReturnType = returnType;
        UploadDate = uploadDate;
        TimesUsed = timesUsed;
        Confirmed = confirmed;
        Verified = verified;
        Likes = likes;
    }

    public static Lambda ReadLambda(SqlDataReader reader)
    {
        return new Lambda(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetString(4),
            reader.GetString(5),
            reader.GetString(6),
            reader.GetString(7),
            reader.GetDateTime(8),
            reader.GetInt32(9),
            reader.GetBoolean(10),
            reader.GetBoolean(11),
            reader.GetInt32(12)
        );
    }
    

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string ProgrammingLanguage { get; set; }
    public string Code { get; set; }
    public string InputType { get; set; }
    public string ReturnType { get; set; }
    public DateTime UploadDate { get; set; }
    public int TimesUsed { get; set; }
    public bool Confirmed { get; set; }
    public bool Verified { get; set; }
    public int Likes { get; set; }


    public LambdaInput[]? Evaluate(IEnumerable<LambdaInput> inputs, LambdaDatabase lambdaDatabase)
    {
        var res = inputs.Select(s => Evaluate(s, lambdaDatabase)).ToArray();
        if (res.Any(s => s is null))
        {
            return null;
        }
        
        return res!;
    }
    private LambdaInput? Evaluate(LambdaInput input, LambdaDatabase lambdaDatabase)
    {
        var fogId = lambdaDatabase.GetSelfFogOfLambda(Id).Id;
        var cached = lambdaDatabase.GetResultByInputAndFog(fogId, input.GetStringRepresentation());
        if (cached is not null) return InputParser.Parse(cached);

        var lambda = lambdaDatabase.GetLambdaById(Id)!;
        
        var result = LanguageServer.Send(new EvaluatePayload(
            Action.Evaluate, lambda.ProgrammingLanguage, new[] { input.GetStringRepresentation() }, lambda.Code));
        
        if (result.Result == Result.FAILURE)
            return null;
        
        var output = (result.Payload as JArray)?[0].Value<string>();
        output = output?.Substring(1, output.Length - 2);

        if (output == null)
        {
            return null;
        }

        lambdaDatabase.CacheResult(fogId, input.GetStringRepresentation(), output);
        return InputParser.Parse(output);
    }
}
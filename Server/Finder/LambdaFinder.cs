using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Server.Communication;
using Server.Models;
using Server.SQL;
using Action = Server.Models.Action;

namespace Server.Searcher;

public class LambdaFinder
{
    private readonly LambdaDatabase _lambdaDatabase;

    public LambdaFinder(LambdaDatabase lambdaDatabase)
    {
        _lambdaDatabase = lambdaDatabase;
    }
    
    
    public bool CheckMethod(Lambda lambda, SearchPayload searchPayload)
    {
        var results = searchPayload.Inputs.Select(s => GetResultOfInput(lambda, s)).ToArray();
        return results.SequenceEqual(searchPayload.Results);
    }

    private string? GetResultOfInput(Lambda lambda, string input)
    {

        var cached = _lambdaDatabase.GetResultByInput(lambda.Id, input);

        if (cached is not null) return cached;
        
        var result = LanguageServer.Send(new EvaluatePayload(
            Action.Evaluate, lambda.ProgrammingLanguage, new[] { input }, lambda.Code));
        
        if (result.Result == Result.FAILURE)
            return null;
        
        var output = (result.Payload as JArray)?[0].Value<string>();
        output = output?.Substring(1, output.Length - 2);

        if (output == null)
        {
            return null;
        }
        
        _lambdaDatabase.CacheResult(lambda.Id, input, output);
        return output;
    }
}
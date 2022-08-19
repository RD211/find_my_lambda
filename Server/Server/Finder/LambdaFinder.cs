using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Server.Communication;
using Server.Models;
using Server.Parser;
using Server.SQL;
using Action = Server.Models.Action;

namespace Server.Finder;

public class LambdaFinder
{
    private readonly LambdaDatabase _lambdaDatabase;

    public LambdaFinder(LambdaDatabase lambdaDatabase)
    {
        _lambdaDatabase = lambdaDatabase;
    }
    
    
    private bool CheckMethod(Lambda lambda, SearchPayload searchPayload)
    {
        var results = searchPayload.Inputs.Select(s => GetResultOfInput(lambda, s)).ToArray();
        return results.SequenceEqual(searchPayload.Results);
    }
    
    private SearchPayload TransformInputs(Lambda lambda, SearchPayload searchPayload)
    {
        var results = searchPayload.Inputs.Select(s => GetResultOfInput(lambda, s)).ToArray();
        if (results.Any(s => s is null))
        {
            throw new ArgumentException("The search payload was badly written.");
        }
        
        return new SearchPayload(results!, searchPayload.Results);
    }

    public List<Lambda> Find(SearchPayload searchPayload)
    {
        var pq = new PriorityQueue<(List<Lambda>, SearchPayload), (int, int)>();
        pq.Enqueue((new List<Lambda>(), searchPayload), (0,0));

        var times = 0;
        while (pq.Count > 0 || times > 100_000)
        {
            times++;
            pq.TryDequeue(out var values, out var priorities);
            var (pastLambdas, search) = values;
            var (len, points) = priorities;
            
            var inputTypeString = InputParser.Convert(search.Inputs.First());
        
            var validSearchMembers = _lambdaDatabase.GetLambdasByInputType(inputTypeString);

            var searchMembers = validSearchMembers as Lambda[] ?? validSearchMembers.ToArray();
            var already = searchMembers.AsParallel().Any((l) => CheckMethod(l, search));

            if (already)
            {
                var found = searchMembers.First((l) => CheckMethod(l, search));
                pastLambdas.Add(found);
                return pastLambdas;
            }

            Parallel.ForEach(searchMembers, lambda =>
            {
                var transformed = TransformInputs(lambda, search);
                var newLambdas = pastLambdas.ToList();
                newLambdas.Add(lambda);

                pq.Enqueue((newLambdas, transformed), (len + 1, points - lambda.TimesUsed));
            });
        }

        return new List<Lambda>();
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
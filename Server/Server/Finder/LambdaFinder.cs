using System.Diagnostics;
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
    
    
    private bool CheckFog(Fog fog, SearchPayload searchPayload)
    {
        var results = searchPayload.Inputs.Select(s => GetResultOfFog(fog, s)).ToArray();
        return results.SequenceEqual(searchPayload.Results);
    }
    
    private SearchPayload TransformInputs(Fog fog, SearchPayload searchPayload)
    {
        var results = searchPayload.Inputs.Select(s => GetResultOfFog(fog, s)).ToArray();
        if (results is null || results.Any(s => s is null))
        {
            throw new ArgumentException("The search payload was badly written.");
        }
        
        return new SearchPayload((string[])results, searchPayload.Results);
    }
    
    public async Task<List<Lambda>> Find(SearchPayload searchPayload)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        
        var pq = new PriorityQueue<(List<int>, SearchPayload), (int, int)>();
        pq.Enqueue((new List<int>(), searchPayload), (0,0));

        while (pq.Count > 0)
        {
            pq.TryDequeue(out var values, out var priorities);
            var (pastFogs, search) = values;
            var (len, points) = priorities;
            
            var inputTypeString = InputParser.Parse(search.Inputs.First()).ToGeneric().GetStringRepresentation();
        
            var validSearchMembers = _lambdaDatabase.GetFogsByInputType(inputTypeString);

            var searchMembers = validSearchMembers as Fog[] ?? validSearchMembers.ToArray();
            var already = searchMembers.Any(l => CheckFog(l, search));

            if (already)
            {
                var found = searchMembers.First((l) => CheckFog(l, search));
                pastFogs.Add(found.Id);
                var res = pastFogs
                    .SelectMany(i => _lambdaDatabase.GetFogMembers(i)).ToList();
                
                if(pastFogs.Count > 1)
                    _lambdaDatabase.InsertFog(res);
                
                return res
                    .Select(i => _lambdaDatabase.GetLambdaById(i)!)
                    .ToList();
            }


            var newMembers = searchMembers.Select(fog => Task.Run(() =>
            {
                var transformed = TransformInputs(fog, search);
                var newFogs = pastFogs.ToList();
                newFogs.Add(fog.Id);

                return ((newFogs, transformed), (len + fog.MemberCount, points - fog.TimesUsed));
            }));

            var results = await Task.WhenAll(newMembers);
            
            foreach (var valueTuple in results)
            {
                pq.Enqueue(valueTuple.Item1, valueTuple.Item2);
            }

            if (stopWatch.ElapsedMilliseconds >= 60 * 1000 * 3)
            {
                throw new TimeoutException("Search took too long.");
            }
        }

        return new List<Lambda>();
    }

    private string? GetResultOfFog(Fog fog, string input)
    {
        try
        {
            var cached = _lambdaDatabase.GetResultByInputAndFog(fog.Id, input);
            if (cached is not null) return cached;

            var members = _lambdaDatabase.GetFogMembers(fog.Id).ToList();

            var output = members.Aggregate(input, (s, i) => GetResultOfInput(i, s)!);
            
            if(members.Count > 1)
                _lambdaDatabase.CacheResult(fog.Id, input, output);
            
            return output;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    private string? GetResultOfInput(int lambdaId, string input)
    {
        var fogId = _lambdaDatabase.GetSelfFogOfLambda(lambdaId).Id;
        var cached = _lambdaDatabase.GetResultByInputAndFog(fogId, input);
        if (cached is not null) return cached;

        var lambda = _lambdaDatabase.GetLambdaById(lambdaId)!;
        
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

        _lambdaDatabase.CacheResult(fogId, input, output);
        return output;
    }
}
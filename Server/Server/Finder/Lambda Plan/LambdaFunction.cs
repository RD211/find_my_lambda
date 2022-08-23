using Server.Lambda_Input;
using Server.Models;
using Server.SQL;

namespace Server.Finder.Lambda_Plan;

public class LambdaFunction : LambdaNode
{
    private List<Lambda>? Lambdas { get; set; }
    private int LambdasMax { get; set; }
    private LambdaNode? NextLambda { get; set; }
    
    public LambdaFunction(List<Lambda> lambdas)
    {
        Lambdas = lambdas;
    }

    public LambdaFunction(int maxLambdas, LambdaNode nextLambda)
    {
        LambdasMax = maxLambdas;
        NextLambda = nextLambda;
    }
    
    public LambdaFunction(List<Lambda> lambdas, LambdaNode nextLambda)
    {
        Lambdas = lambdas;
        NextLambda = nextLambda;
    }

    public LambdaFunction()
    {
    }
    
    public override LambdaInput[]? Forward(IEnumerable<LambdaInput> inputs, LambdaDatabase lambdaDatabase)
    {
        if (Lambdas is null) return null;
        
        var res = Lambdas!.Aggregate(inputs, (results, lambda) => lambda.Evaluate(results, lambdaDatabase)!).ToArray();
        if (NextLambda is null) return res;
        if (!NextLambda.Finished()) return null;
        return NextLambda.Forward(res, lambdaDatabase);
    }

    public override bool Finished()
    {
        return Lambdas is not null && (NextLambda is null || NextLambda.Finished());
    }

    public override LambdaNode? Search(IEnumerable<LambdaInput> inputs, IEnumerable<LambdaInput> outputs, LambdaDatabase lambdaDatabase)
    {
        var inputType = inputs.First().ToGeneric().GetStringRepresentation();
        if (NextLambda is null)
        {
            var finalType = outputs.First().ToGeneric().GetStringRepresentation();
            var candidates = lambdaDatabase.GetFogByInputAndOutputTypeAndLessMembers(inputType, finalType, LambdasMax);
            
        }
    }
}
using Server.Lambda_Input;
using Server.Models;
using Server.SQL;

namespace Server.Finder.Lambda_Plan;

public class LambdaArrayMap : LambdaMetaArray
{
    private LambdaNode? MapLambda { get; set; }
    private LambdaNode? NextLambda { get; set; }

    public LambdaArrayMap(LambdaNode mapLambda, LambdaNode nextLambda)
    {
        MapLambda = mapLambda;
        NextLambda = nextLambda;
    }
    
    public LambdaArrayMap(){}
    
    public override LambdaInput[]? Forward(IEnumerable<LambdaInput> inputs, LambdaDatabase lambdaDatabase)
    {
        if (MapLambda is null) return null;

        var res = inputs.Select(input =>
        {
            var decomposed = input.Decompose();
            if (decomposed.Count > 1)
            {
                throw new ArgumentException("Input is of invalid format.");
            }

            var arr = decomposed.First();

            if (arr is not LambdaArray)
            {
                throw new ArgumentException("Input needs to be an array but something else received.");
            }

            var res = MapLambda.Forward(arr.Decompose(), lambdaDatabase);
            if (res is null) throw new ArgumentException("Something went wrong while applying Map.");

            return new LambdaArray(res.ToList());
        });

        if (NextLambda is null) return res.ToArray() as LambdaInput[];
        return NextLambda.Forward(res, lambdaDatabase);
    }

    public override bool Finished()
    {
        return MapLambda is not null && MapLambda.Finished() 
                                     && (NextLambda is null || NextLambda.Finished());
    }

    public override LambdaNode? GetMetaFunction()
    {
        return MapLambda;
    }
}
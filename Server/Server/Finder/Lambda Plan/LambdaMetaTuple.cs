using Server.SQL;

namespace Server.Finder.Lambda_Plan;

public abstract class LambdaMetaTuple : LambdaNode
{
    public abstract List<LambdaNode>? GetTupleFunctions();
}
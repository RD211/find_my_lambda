using Server.Lambda_Input;
using Server.Models;
using Server.SQL;

namespace Server.Finder.Lambda_Plan;

public abstract class LambdaNode
{
    public abstract LambdaInput[]? Forward(IEnumerable<LambdaInput> inputs, LambdaDatabase lambdaDatabase);
    public abstract bool Finished();
    public abstract LambdaNode? Search(IEnumerable<LambdaInput> inputs, IEnumerable<LambdaInput> outputs, LambdaDatabase lambdaDatabase)
}
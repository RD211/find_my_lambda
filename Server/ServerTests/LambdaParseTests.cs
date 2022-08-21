using Server.Parser;

namespace ServerTests;

public class LambdaParseTests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Test1()
    {
        Console.WriteLine(InputParser.Parse("([ 1, 2, 3], 5,\" hi\")").GetStringRepresentation());
    }
}
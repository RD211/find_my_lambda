using Server.Parser;

namespace ServerTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Test1()
    {
        Console.WriteLine(InputParser.Convert("(1,2,3)"));
    }
}
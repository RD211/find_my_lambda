using System.Diagnostics;
using CSRunner;
using CSRunner.Parser;

namespace CSRunnerTests;

public class ParserTests
{
    private readonly List<(string, Type, object)> _testCases = new()
    {
        ("5", typeof(int), 5),
        ("5.0", typeof(double), 5.0d),
        ("\"wow\"", typeof(string), "wow"),
        ("(5,5)", typeof(ValueTuple<int,int>), (5,5)),
        ("[[5],[5]]", typeof(int[][]), new []{new []{5}, new []{5}}),
        ("(\"wow\",[5,5])", typeof(ValueTuple<string, int[]>), ("wow", new []{5,5})),
        ("(\"wow\",[5,5,6,3,1,4,5,8,0,8])", typeof(ValueTuple<string, int[]>), ("wow", new []{5,5,6,3,1,4,5,8,0,8})),
    };
    
    [SetUp]
    public void Setup() {}
    
    [Test]
    public void TestParser()
    {
        _testCases.ForEach(testCase => 
            Assert.That(InputParser.Parse(testCase.Item1, testCase.Item2), Is.EqualTo(testCase.Item3)));
    }
}
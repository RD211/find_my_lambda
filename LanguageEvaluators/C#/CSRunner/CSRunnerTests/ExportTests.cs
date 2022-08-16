using CSRunner.Parser;

namespace CSRunnerTests;

public class ExportTests
{
    private readonly List<(object, string)> _testCases = new()
    {
        (5, "5"),
        ((5,5), "(5,5)"),
        (new[]{5,1}, "[5,1]"),
        (new[]{(3,4),(7,9)}, "[(3,4),(7,9)]"),
        (new[]{(3,"asd"),(7,"asdd")}, "[(3,\"asd\"),(7,\"asdd\")]"),
    };
    
    private readonly List<(Type, string)> _testCases2 = new()
    {
        (typeof(int), "int"),
        (typeof(string), "string"),
        (typeof((string, int)), "(string,int)"),
        (typeof((int, double)[]), "[(int,double)]"),
    };
    
    
    [SetUp]
    public void Setup() {}
    
    [Test]
    public void TestExport()
    {
        _testCases.ForEach(testCase => 
            Assert.That(InputParser.Export(testCase.Item1), Is.EqualTo(testCase.Item2)));
    }
    
    [Test]
    public void TestExport2()
    {
        _testCases2.ForEach(testCase => 
            Assert.That(InputParser.Export(testCase.Item1), Is.EqualTo(testCase.Item2)));
    }
}
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
    
    [SetUp]
    public void Setup() {}
    
    [Test]
    public void TestExport()
    {
        _testCases.ForEach(testCase => 
            Assert.That(InputParser.Export(testCase.Item1), Is.EqualTo(testCase.Item2)));
    }
}
using System.Runtime.Caching;
using CSRunner;
using CSRunner.Helpers;

namespace CSRunnerTests;

public class EvaluateTests
{
    private readonly string 
        _code1 = File.ReadAllText("./Lambdas/Valid/Code1.txt"), 
        _code2 = File.ReadAllText("./Lambdas/Valid/Code2.txt"), 
        _code3 = File.ReadAllText("./Lambdas/Valid/Code3.txt");

    private readonly Evaluator _evaluator = 
        new(new MemoryCache("big cache"), new CacheItemPolicy(), new RandomDataFactory(new Random()));
    
    [SetUp]
    public void Setup()
    { }
    
    [Test]
    public void TestCode1()
    {
        Assert.That(
            _evaluator.Evaluate(_code1, new List<string>
            {
                "(1)",
                "(2)",
                "(3)",
                "(4)",
                "(5)",
                "(6)",
            }),
            Is.EqualTo(new List<string>
            {
                "(2)","(3)","(4)","(5)","(6)","(7)"
            }));
    }
    
    [Test]
    public void TestCode2()
    {
        Assert.That(
            _evaluator.Evaluate(_code2, new List<string>
            {
                 "((1,2))" ,
                "((2,1))",
                "((100,0))",
                "((1000,-1))",
                "((-1,1000))",
                "((0,0))",
            }),
            Is.EqualTo(new List<string>
            {
                "(1)","(2)","(100)","(1000)","(-1)","(0)"
            }));
    }
    
    [Test]
    public void TestCode3()
    {
        Assert.That(
            _evaluator.Evaluate(_code3, new List<string>
            {
                "([(5,[1])])",
                "([(5,[])])",
            }),
            Is.EqualTo(new List<string>
            {
                "((5,5))","((5,5))"
            }));
    }
}
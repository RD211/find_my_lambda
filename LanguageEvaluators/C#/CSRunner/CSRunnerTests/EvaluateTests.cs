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
            _evaluator.Evaluate(_code1, new List<string[]>
            {
                new [] { "1" },
                new [] { "2" },
                new [] { "3" },
                new [] { "4" },
                new [] { "5" },
                new [] { "6" },
            }),
            Is.EqualTo(new List<object?>
            {
                2, 3, 4, 5, 6, 7
            }));
    }
    
    [Test]
    public void TestCode2()
    {
        Assert.That(
            _evaluator.Evaluate(_code2, new List<string[]>
            {
                new [] { "(1,2)" },
                new [] { "(2,1)" },
                new [] { "(100,0)" },
                new [] { "(1000,-1)" },
                new [] { "(-1,1000)" },
                new [] { "(0,0)" },
            }),
            Is.EqualTo(new List<object?>
            {
                1,2,100,1000,-1,0
            }));
    }
    
    [Test]
    public void TestCode3()
    {
        Assert.That(
            _evaluator.Evaluate(_code3, new List<string[]>
            {
                new [] { "[(5,[1])]" },
                new [] { "[(5,[])]" },
            }),
            Is.EqualTo(new List<object?>
            {
                (5,5),(5,5)
            }));
    }
}
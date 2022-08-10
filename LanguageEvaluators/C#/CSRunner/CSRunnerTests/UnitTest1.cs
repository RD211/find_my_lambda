using System.Runtime.Caching;
using CSRunner;

namespace CSRunnerTests;

public class Tests
{
    private string _code1 = "", _code1_w_tests = "", _code2 = "";
    [SetUp]
    public void Setup()
    {
        _code1 = @"using System;
public class Lambda {
    public (string,int,int) lambda(string[] a, int b) {
        return (a[1],b,b);
    }
    }";
        _code1_w_tests = @"using System;
public class Lambda {
    public static (int, (int,string)) lambda(int a) {
        return (a, (30,""40""));
    }
}
return new (int, (int,string))[]{
    Lambda.lambda(5),
    Lambda.lambda(6),};
";

    }

    [Test]
    public void TestInsertInputs1()
    {

        var evaluator = new Evaluator(new MemoryCache("big cache"), new CacheItemPolicy(), new RandomDataFactory(new Random()));

        var result = evaluator.Evaluate(_code1, new List<string[]>()
        {
            new string[] { @"[""wow"",""damn""]", "6" },
        });
        foreach (var o in result)
        {
            Console.WriteLine(o);
        }

    }
    
    [Test]
    public async Task TestEvaluate1()
    {
        var input = new List<string>() { "5", "6" };
    }
}
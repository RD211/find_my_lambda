using CSRunner;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace CSRunnerTests;

public class Tests
{
    private string _code1 = "", _code1_w_tests = "", _code2 = "";
    [SetUp]
    public void Setup()
    {
        _code1 = @"using System;
public class Lambda {
    public static (int, (int,string)) lambda(int a) {
        return (a, (30,""40""));
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

        var input = new List<string>() { "5", "6" };
        Console.WriteLine(Evaluator.InsertInputsInCode(_code1, input));
    }
    
    [Test]
    public async Task TestEvaluate1()
    {
        var input = new List<string>() { "5", "6" };
        Console.WriteLine((await Evaluator.EvaluateCode(_code1_w_tests)).Value.GetType());
    }
}
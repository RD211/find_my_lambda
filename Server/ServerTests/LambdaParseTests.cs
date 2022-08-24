using Server.Lambda_Input;
using Server.Parser;

namespace ServerTests;

public class LambdaParseTests
{

    [TestCaseSource(nameof(ParseCases))]
    public void TestsParse(string input, LambdaInput parsedLambda)
    {
        Assert.That(InputParser.Parse(input), Is.EqualTo(parsedLambda));
    }

    [TestCaseSource(nameof(InvalidCases))]
    public void TestsInvalid(string input)
    {
        Assert.Throws<ArgumentException>(() => InputParser.Parse(input));
    }

    static object[] ParseCases = {
        // Bool tests
        new object[]{"true", new LambdaBool(true)},
        new object[]{"false", new LambdaBool(false)},

        // Int tests
        new object[]{"1", new LambdaInt(1)},
        new object[]{"-1231231", new LambdaInt(-1231231)},
        new object[]{"2", new LambdaInt(2)},
        
        // Double tests
        new object[]{"2.22", new LambdaDouble(2.22d)},
        new object[]{"-2.22", new LambdaDouble(-2.22d)},
        
        // String tests
        new object[]{"\"Hiiii\"", new LambdaString("Hiiii")},
        new object[]{"\"well hello there you\"", new LambdaString("well hello there you")},
        new object[]{"\"\"", new LambdaString("")},

        // Tuple tests
        new object[]{"(5)", new LambdaTuple(new List<LambdaInput>
        {
            new LambdaInt(5),
        })},
        new object[]{"(-123123)", new LambdaTuple(new List<LambdaInput>
        {
            new LambdaInt(-123123),
        })},
        new object[]{"(-123123, 123)", new LambdaTuple(new List<LambdaInput>
        {
            new LambdaInt(-123123),
            new LambdaInt(123),
        })},
        new object[]{"(1,2,3,4,5,6,7,8)", new LambdaTuple(new List<LambdaInput>
        {
            new LambdaInt(1),
            new LambdaInt(2),
            new LambdaInt(3),
            new LambdaInt(4),            
            new LambdaInt(5),
            new LambdaInt(6),
            new LambdaInt(7),
            new LambdaInt(8),
        })},
        new object[]{"((1,2),(3))", new LambdaTuple(new List<LambdaInput>
        {
            new LambdaTuple(new List<LambdaInput>(){new LambdaInt(1), new LambdaInt((2))}),
            new LambdaTuple(new List<LambdaInput>(){new LambdaInt(3)}),
        })},
        
        // Array tests
        new object[]{"[1]", new LambdaArray(new List<LambdaInput>
        {
            new  LambdaInt(1)
        })},
        new object[]{"[1,2]", new LambdaArray(new List<LambdaInput>
        {
            new  LambdaInt(1),
            new LambdaInt(2)
        })},
        new object[]{"[[1,2],[2,3]]", new LambdaArray(new List<LambdaInput>
        {
            new LambdaArray(new List<LambdaInput>{new LambdaInt(1), new LambdaInt(2)}),
            new LambdaArray(new List<LambdaInput>{new LambdaInt(2), new LambdaInt(3)}),
        })},
        
        // Mixed tests
        new object[]{"([ 1, 2, 3], 5,\" hi\")", new LambdaTuple(new List<LambdaInput>
        {
            new LambdaArray(new List<LambdaInput>() {new LambdaInt(1),new LambdaInt(2),new LambdaInt(3)}),
            new LambdaInt(5),
            new LambdaString(" hi")
        })},
    };

    private static object[] InvalidCases =
    {
        "",
        "(,)",
        "(,,,)",
        "[][]",
        "[",
        "'",
        "ad",
        "'ada'",
        "True",
        "tRue",
        "False",
        "98012132908132098",
        "-67878787687678876867",
        "()",
        "[]",
        "[,]",
        "[1,,2]",
        "[1)",
        "[[3]",
        "]",
        "[1#2]",
        "[1,\"hi\"]",
        "(1,2,3,4,5,6,7,8,9,10,11,12)",
        "[][][][",
        "[1",
        "asd",
        "wow",
        "[1)",
        "tru",
        "fals",
        "(1,2,3,4,5,6,7,8,9)",
        "(1,2,3,4,5,6,7,8,9,10)"
    };
}
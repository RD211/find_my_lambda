using Server.Lambda_Input;
using Server.Parser;

namespace ServerTests;

public class LambdaInputTests
{
    [TestCaseSource(nameof(ToStringCases))]
    public void TestsGeneral(LambdaInput input, string stringRepresentation, string genericRepresentation)
    {
        Assert.That(input.GetStringRepresentation(), Is.EqualTo(stringRepresentation));
        Assert.That(input.ToGeneric().GetStringRepresentation(), Is.EqualTo(genericRepresentation));
    }

    [TestCaseSource(nameof(GenericTrueCases))]
    public void TestIsGenericTrue(LambdaInput lambdaInput)
    {
        Assert.That(lambdaInput.IsGeneric, Is.True);
    }
    
    [TestCaseSource(nameof(GenericFalseCases))]
    public void TestIsGenericFalse(LambdaInput lambdaInput)
    {
        Assert.That(lambdaInput.IsGeneric, Is.False);
    }

    [TestCaseSource(nameof(DecomposeCases))]
    public void TestsToGeneric(LambdaInput lambdaInput, List<LambdaInput> decomposed)
    {
        Assert.That(lambdaInput.Decompose(), Is.EqualTo(decomposed));
    }
    
    [TestCaseSource(nameof(GenericFalseCases))]
    public void TestToGenericFalse(LambdaInput lambdaInput)
    {
        Assert.That(lambdaInput.ToGeneric().IsGeneric, Is.True);
    }
    
    [TestCaseSource(nameof(GenericTrueCases))]
    public void TestToGenericTrue(LambdaInput lambdaInput)
    {
        Assert.That(lambdaInput.ToGeneric().IsGeneric, Is.True);
    }

    [TestCaseSource(nameof(ValueCases))]
    public void TestsToValue(LambdaInput lambdaInput, object value)
    {
        Assert.That(lambdaInput.GetValueRepresentation(), Is.EqualTo(value));
    }
    
    [TestCaseSource(nameof(InvalidValueCases))]
    public void TestsToValueInvalid(LambdaInput lambdaInput)
    {
        Assert.Throws<Exception>(() => lambdaInput.GetValueRepresentation());
    }

    private static object[] InvalidValueCases =
    {
        
        new object[]{new LambdaInt()},
        new object[]{new LambdaBool()},
        new object[]{new LambdaDouble()},
        new object[]{new LambdaString()},
        new object[]{new LambdaArray(new LambdaBool())},
        new object[]{new LambdaTuple(new List<LambdaInput>{new LambdaBool()})},
    };
    
    private static object[] ValueCases =
    {
        // Int tests
        new object[]{new LambdaInt(1), 1},
        new object[]{new LambdaInt(-1), -1},
        
        // Bool tests
        new object[]{new LambdaBool(true), true},
        new object[]{new LambdaBool(false), false},
        
        // String tests
        new object[]{new LambdaString(""), ""},
        new object[]{new LambdaString("wow"), "wow"},
        
        // Double tests
        new object[]{new LambdaDouble(2), 2d},
        new object[]{new LambdaDouble(-2), -2d},
        
        // Array tests
        new object[]{new LambdaArray(new List<LambdaInput>{new LambdaInt(1)}), new []{1}},
        new object[]{new LambdaArray(new List<LambdaInput>{new LambdaInt(1), new LambdaInt(2)}), new []{1,2}},
        
        // Tuple tests
        new object[]{new LambdaTuple(new List<LambdaInput>{new LambdaInt(1), new LambdaInt(2)}), (1,2)},
        new object[]{new LambdaTuple(new List<LambdaInput>
        {
            new LambdaInt(1), 
            new LambdaInt(2),
            new LambdaInt(3),
        }), (1,2,3)},
        new object[]{new LambdaTuple(new List<LambdaInput>
        {
            new LambdaInt(1), 
            new LambdaInt(2),
            new LambdaInt(3),
            new LambdaInt(4),
        }), (1,2,3,4)},
        new object[]{new LambdaTuple(new List<LambdaInput>
        {
            new LambdaInt(1), 
            new LambdaInt(2),
            new LambdaInt(3),
            new LambdaInt(4),
            new LambdaInt(5),
        }), (1,2,3,4,5)},
        new object[]{new LambdaTuple(new List<LambdaInput>
        {
            new LambdaInt(1), 
            new LambdaInt(2),
            new LambdaInt(3),
            new LambdaInt(4),
            new LambdaInt(5),
            new LambdaInt(6),
        }), (1,2,3,4,5,6)},
        new object[]{new LambdaTuple(new List<LambdaInput>
        {
            new LambdaInt(1), 
            new LambdaInt(2),
            new LambdaInt(3),
            new LambdaInt(4),
            new LambdaInt(5),
            new LambdaInt(6),
            new LambdaInt(7),
        }), (1,2,3,4,5,6,7)},
        new object[]{new LambdaTuple(new List<LambdaInput>
        {
            new LambdaInt(1), 
            new LambdaInt(2),
            new LambdaInt(3),
            new LambdaInt(4),
            new LambdaInt(5),
            new LambdaInt(6),
            new LambdaInt(7),
            new LambdaInt(8),
        }), (1,2,3,4,5,6,7,8)},
        new object[]{new LambdaTuple(new List<LambdaInput>{new LambdaInt(1)}), ValueTuple.Create(1)},
    };

    static object[] DecomposeCases =
    {
        new object[]{new LambdaInt(), new List<LambdaInput>{new LambdaInt()}},
        new object[]{new LambdaBool(), new List<LambdaInput>{new LambdaBool()}},
        new object[]{new LambdaDouble(), new List<LambdaInput>{new LambdaDouble()}}, 
        new object[]{new LambdaString(), new List<LambdaInput>{new LambdaString()}},
        new object[]{new LambdaArray(new LambdaInt()), new List<LambdaInput>{new LambdaInt()}},
        new object[]{new LambdaTuple(new List<LambdaInput> { new LambdaDouble(), new LambdaBool() })
            , new List<LambdaInput>{new LambdaDouble(), new LambdaBool()}},
        
        new object[]{new LambdaInt(5), new List<LambdaInput>{new LambdaInt(5)}},
        new object[]{new LambdaBool(true), new List<LambdaInput>{new LambdaBool(true)}},
        new object[]{new LambdaDouble(1), new List<LambdaInput>{new LambdaDouble(1)}}, 
        new object[]{new LambdaString(""), new List<LambdaInput>{new LambdaString("")}},
        new object[]{new LambdaArray(new List<LambdaInput>{new LambdaInt(1)}), new List<LambdaInput>{new LambdaInt(1)}},
        new object[]{new LambdaTuple(new List<LambdaInput> { new LambdaDouble(1), new LambdaBool(true) })
            , new List<LambdaInput>{new LambdaDouble(1), new LambdaBool(true)}},
    };
    
    static object[] GenericTrueCases =
    {
        new object[]{new LambdaInt()},
        new object[]{new LambdaBool()},
        new object[]{new LambdaDouble()}, 
        new object[]{new LambdaString()},
        new object[]{new LambdaArray(new LambdaInt())},
        new object[]{new LambdaTuple(new List<LambdaInput> { new LambdaDouble(), new LambdaBool() })},
    };
    
    static object[] GenericFalseCases =
    {
        new object[]{new LambdaInt(1)},
        new object[]{new LambdaBool(true)},
        new object[]{new LambdaDouble(1)},
        new object[]{new LambdaString("")},
        new object[]{new LambdaArray(new List<LambdaInput>{new LambdaInt(1)})},
        new object[]{new LambdaTuple(new List<LambdaInput> { new LambdaDouble(2), new LambdaBool(true) })},
    };
    
    static object[] ToStringCases = {
        // Bool tests
        new object[]{new LambdaBool(), "bool", "bool"},
        new object[]{new LambdaBool(true), "true", "bool"},
        new object[]{new LambdaBool(false), "false", "bool"},

        // Int tests
        new object[]{new LambdaInt(), "int", "int"},
        new object[]{new LambdaInt(1), "1", "int"},
        new object[]{new LambdaInt(2), "2", "int"},
        new object[]{new LambdaInt(-2), "-2", "int"},

        // Double tests
        new object[]{new LambdaDouble(), "double", "double"},
        new object[]{new LambdaDouble(2.2), "2.2", "double"},
        new object[]{new LambdaDouble(-2.2), "-2.2", "double"},
        
        // String tests
        new object[]{new LambdaString(), "string", "string"},
        new object[]{new LambdaString("hi"), "\"hi\"", "string"},
        new object[]{new LambdaString("well well"), "\"well well\"", "string"},
        
        // Array tests
        new object[]{new LambdaArray(new LambdaBool()), "[bool]", "[bool]"},
        new object[]{new LambdaArray(new LambdaArray(new LambdaInt())), "[[int]]", "[[int]]"},
        new object[]{new LambdaArray(new List<LambdaInput>
        {
            new LambdaBool(true),
            new LambdaBool(false)
        }), "[true,false]", "[bool]"},
        
        // Tuple tests
        new object[]{new LambdaTuple(new List<LambdaInput>
        {
            new LambdaInt(),
            new LambdaDouble(),
            new LambdaBool(),
            new LambdaString(),
            new LambdaArray(new LambdaInt())
        }), "(int,double,bool,string,[int])", "(int,double,bool,string,[int])"},
        new object[]{new LambdaTuple(new List<LambdaInput>
        {
            new LambdaInt(1),
        }), "(1)", "(int)"},
        
        new object[]{new LambdaTuple(new List<LambdaInput>
        {
            new LambdaBool(true),
            new LambdaBool(false)
        }), "(true,false)", "(bool,bool)"},
    };
}
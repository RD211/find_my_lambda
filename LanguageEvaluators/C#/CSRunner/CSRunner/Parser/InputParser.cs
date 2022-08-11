using System.Globalization;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CSRunner.Helpers;

namespace CSRunner.Parser;

public static class InputParser
{
    public static object Parse(string input, Type resultingType)
    {
        var stream = CharStreams.fromString(input);
        var lexer = new LambdaLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new LambdaParser(tokens) { BuildParseTree = true };

        IParseTree tree = parser.input();

        return Walk(tree, resultingType, parser);
    }

    public static string Export(object? obj)
    {
        var t = obj.GetType();
        var dynamicObj = (dynamic)obj;
        
        if (t.IsGenericType)
        {
            var gt = t.GetGenericTypeDefinition();
            if (gt == typeof(ValueTuple<>))
                return $"({Export(dynamicObj.Item1)})";
            if (gt == typeof(ValueTuple<,>))
                return $"({Export(dynamicObj.Item1)},{Export(dynamicObj.Item2)})";
            if (gt == typeof(ValueTuple<,,>))
                return $"({Export(dynamicObj.Item1)},{Export(dynamicObj.Item2)},{Export(dynamicObj.Item3)})";
            if (gt == typeof(ValueTuple<,,,>))
                return $"({Export(dynamicObj.Item1)},{Export(dynamicObj.Item2)},{Export(dynamicObj.Item3)},{Export(dynamicObj.Item4)})";
            if (gt == typeof(ValueTuple<,,,,>))
                return $"({Export(dynamicObj.Item1)},{Export(dynamicObj.Item2)},{Export(dynamicObj.Item3)},{Export(dynamicObj.Item4)},{Export(dynamicObj.Item5)})";
            if (gt == typeof(ValueTuple<,,,,,>))
                return $"({Export(dynamicObj.Item1)},{Export(dynamicObj.Item2)},{Export(dynamicObj.Item3)},{Export(dynamicObj.Item4)},{Export(dynamicObj.Item5)},{Export(dynamicObj.Item6)})";
            if (gt == typeof(ValueTuple<,,,,,,>))
                return $"({Export(dynamicObj.Item1)},{Export(dynamicObj.Item2)},{Export(dynamicObj.Item3)},{Export(dynamicObj.Item4)},{Export(dynamicObj.Item5)},{Export(dynamicObj.Item6)},{Export(dynamicObj.Item7)})";
            if (gt == typeof(ValueTuple<,,,,,,,>))
                return $"({Export(dynamicObj.Item1)},{Export(dynamicObj.Item2)},{Export(dynamicObj.Item3)},{Export(dynamicObj.Item4)},{Export(dynamicObj.Item5)},{Export(dynamicObj.Item6)},{Export(dynamicObj.Item7)},{Export(dynamicObj.Item8)})";
        }
        else if (t.IsArray)
        {
            var acc = "";
            for (var i = 0; i < dynamicObj.Length; i++)
            {
                acc += Export(dynamicObj[i]);
                if (i != dynamicObj.Length - 1)
                {
                    acc += ',';
                }
            }

            return $"[{acc}]";
        }
        else
        {
            return obj switch
            {
                int i => i.ToString(),
                string s => $"\"{s}\"",
                double d => d.ToString(CultureInfo.InvariantCulture),
                _ => throw new ArgumentException("Couldn't recognize type.")
            };
        }

        throw new ArgumentException("Couldn't recognize type.");
    }

    private static object Walk(IParseTree tree, Type desiredType, LambdaParser parser)
    {
        
        if (tree.Payload is IToken token)
        {
            switch (TokenToStringIdentifier(token, parser))
            {
                case "true":
                    return true;
                case "false":
                    return false;
                case "STRING":
                    return token.Text.Substring(1,token.Text.Length - 2);
                case "INTEGER":
                    return int.Parse(token.Text);
                case "REAL":
                    return double.Parse(token.Text);
            }
        }
        
        if (tree.Payload is RuleContext rule)
        {
            var ruleName = parser.RuleNames[rule.RuleIndex];

            switch (ruleName)
            {
                case "tuple":
                    var generics = desiredType.GenericTypeArguments;
                    return ReflectionHelpers.CreateTupleWithRuntimeType(
                        generics, 
                        Enumerable.Range(0, rule.ChildCount)
                            .Where(i => i%2 == 1)
                            .Select(i => Walk(rule.GetChild(i), generics[(i-1)/2], parser))
                            .ToArray()
                        );
                case "arr":
                    var innerType = desiredType.GetElementType();
                    
                    if (innerType is null)
                    {
                        throw new ArgumentException("Invalid type provided.");
                    }

                    if (rule.ChildCount == 2)
                    {
                        return ReflectionHelpers.CreateArrayWithRuntimeType(innerType, 
                            Array.Empty<object>());
                    }

                    return ReflectionHelpers.CreateArrayWithRuntimeType(
                        innerType,
                        Enumerable.Range(0, rule.ChildCount)
                            .Where(i => i%2 == 1)
                            .Select(i => Walk(rule.GetChild(i), innerType, parser))
                            .ToArray()
                        );
                case "input":
                    return Walk(tree.GetChild(0), desiredType, parser);
                case "value":
                    return Walk(tree.GetChild(0), desiredType, parser);
            }
        }

        throw new ArgumentException("Wrong type string provided.");
    }

    private static string TokenToStringIdentifier(IToken token, LambdaParser parser)
    {
        var id = token.Type;
        var tokenName = "";
        foreach (var (key, value) in parser.TokenTypeMap)
            if (value == id)
                tokenName = key;
        return tokenName;
    }
}
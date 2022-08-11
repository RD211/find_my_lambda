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
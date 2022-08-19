using System.Globalization;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Server.Parser;

public static class InputParser
{
    public static string Convert(string input)
    {
        var stream = CharStreams.fromString(input);
        var lexer = new LambdaLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new LambdaParser(tokens) { BuildParseTree = true };

        IParseTree tree = parser.input();

        return Walk(tree, parser);
    }
    
    private static string Walk(IParseTree tree, LambdaParser parser)
    {
        
        if (tree.Payload is IToken token)
        {
            switch (TokenToStringIdentifier(token, parser))
            {
                case "true":
                    return "bool";
                case "false":
                    return "bool";
                case "STRING":
                    return "string";
                case "INTEGER":
                    return "int";
                case "REAL":
                    return "double";
            }
        }

        if (tree.Payload is not RuleContext rule) throw new ArgumentException("Wrong value string provided.");
        
        var ruleName = parser.RuleNames[rule.RuleIndex];

        return ruleName switch
        {
            "tuple" => @$"({string.Join(',',Enumerable.Range(0, rule.ChildCount)
                .Where(i => i%2 == 1)
                .Select(i => Walk(rule.GetChild(i), parser))
                .ToArray())})",
            "arr" => $"[{Walk(rule.GetChild(1), parser)}]",
            "input" => Walk(tree.GetChild(0), parser),
            "value" => Walk(tree.GetChild(0), parser),
            _ => throw new ArgumentException("Wrong value string provided.")
        };
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
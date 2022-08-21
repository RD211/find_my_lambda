using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Server.Lambda_Input;

namespace Server.Parser;

public static class InputParser
{
    public static LambdaInput Parse(string input)
    {
        var stream = CharStreams.fromString(input);
        var lexer = new LambdaLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new LambdaParser(tokens) { BuildParseTree = true };

        IParseTree tree = parser.input();

        return WalkValueStringToLambdaInput(tree, parser);
    }
    
    private static LambdaInput WalkValueStringToLambdaInput(IParseTree tree, LambdaParser parser)
    {
        
        if (tree.Payload is IToken token)
        {
            switch (TokenToStringIdentifier(token, parser))
            {
                case "true":
                    return new LambdaBool(true);
                case "false":
                    return new LambdaBool(false);
                case "STRING":
                    return new LambdaString(token.Text.Substring(1,token.Text.Length - 2));
                case "INTEGER":
                    return new LambdaInt(int.Parse(token.Text));
                case "REAL":
                    return new LambdaDouble(double.Parse(token.Text));
            }
        }

        if (tree.Payload is not RuleContext rule) throw new ArgumentException("Wrong value string provided.");
        
        var ruleName = parser.RuleNames[rule.RuleIndex];

        return ruleName switch
        {
            "tuple" => new LambdaTuple(Enumerable.Range(0, rule.ChildCount)
                .Where(i => i%2 == 1)
                .Select(i => WalkValueStringToLambdaInput(rule.GetChild(i), parser))
                .ToList()),
            "arr" => new LambdaArray(Enumerable.Range(0, rule.ChildCount)
                .Where(i => i%2 == 1)
                .Select(i => WalkValueStringToLambdaInput(rule.GetChild(i), parser))
                .ToList()),
            "input" => WalkValueStringToLambdaInput(tree.GetChild(0), parser),
            "value" => WalkValueStringToLambdaInput(tree.GetChild(0), parser),
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
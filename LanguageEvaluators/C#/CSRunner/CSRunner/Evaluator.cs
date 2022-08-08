using CSRunner.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace CSRunner;

public class Evaluator
{
    public static string InsertInputsInCode(string code, IEnumerable<string> inputs)
    {
        var function = SyntaxAnalysisHelpers.GetMethodsByName(code, "lambda");
        var returnType = SyntaxAnalysisHelpers.GetFunctionReturn(function[0]);
        return $"{code}\n" +
               $"return new {returnType.ToString()}[]{{" +
               $"{inputs.Select(s => $"\nLambda.lambda({s}).ToString(),").Aggregate((s, s1) => s + s1)}" +
               @"};";
    }

    public static string GenerateInputsFromCode(string code)
    {
        return "";
    }

    public static async Task<Optional<object>> EvaluateCode(string code)
    {
        var result = await CSharpScript.EvaluateAsync<object>(code);
        return result ?? new Optional<object>();
    }
}
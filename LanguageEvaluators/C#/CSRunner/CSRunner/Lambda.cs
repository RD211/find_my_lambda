using CSRunner.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSRunner;

public static class Lambda
{

    private static bool CheckClass(string code)
    {
        //var declaration = SyntaxAnalysisHelpers.GetClassDeclaration(code);
        //return declaration.Identifier.ToString() == "Lambda";
        return false;
    }

    private static bool CheckMethods(string code)
    {
        //var methods = SyntaxAnalysisHelpers.GetMethodsByName(code, "lambda");
        //return methods.Count == 1;
        return false;
    }

    private static bool CheckParameters(string code)
    {
        //var methods = SyntaxAnalysisHelpers.GetMethodsByName(code, "lambda");
        //if (methods.Count != 1) return false;
        
        //var method = methods[0];
        //var parameters = SyntaxAnalysisHelpers.GetFunctionParameters(method).ToList();
        //return parameters.Count >= 1;
        return false;
    }
    
    public static bool Verify(string code)
    {
        try
        {
            return CheckClass(code) && CheckMethods(code) && CheckParameters(code);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }
    }
}
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSRunner;

public static class CsRunner
{
    private static async void CompileCode(string code)
    {
        var r = await CSharpScript.EvaluateAsync<int>(code);
        Console.WriteLine(r);
    }
    
    public static void Main(string[] args)
    {
        if (args.Length <= 1)
        {
            throw new Exception("Bad Input");
        }

        var actionType = args[0]; 
        switch (actionType)
        {
            case "PUT":
                
                break;
            case "VERIFY":
                
                break;
            case "CALCULATE":
                
                break;
        }
        
        CompileCode("");
        Thread.Sleep(3000);
        Console.WriteLine("hello there");
    }
}
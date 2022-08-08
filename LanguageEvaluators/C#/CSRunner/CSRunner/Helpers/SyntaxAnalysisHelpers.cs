using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSRunner.Helpers;

public static class SyntaxAnalysisHelpers
{
    public static ClassDeclarationSyntax GetClassDeclaration(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = tree.GetCompilationUnitRoot();
        if (root.Members.Count != 1)
        {
            throw new Exception("Lambda code has the incorrect amount of top level members.");
        }
        
        var programDeclaration = (ClassDeclarationSyntax)root.Members[0];
        return programDeclaration;
    }
    
    private static IEnumerable<MemberDeclarationSyntax> GetAllMembersOfClass(string code)
    {
        var programDeclaration = GetClassDeclaration(code);
        return programDeclaration.Members.ToList();
    }

    private static IEnumerable<MethodDeclarationSyntax> GetMethods(string code)
    {
        var members = GetAllMembersOfClass(code);
        return members.OfType<MethodDeclarationSyntax>().ToList();
    }

    public static List<MethodDeclarationSyntax> GetMethodsByName(string code, string methodName)
    {
        var methods = GetMethods(code);
        return methods.Where(syntax => syntax.Identifier.ToString() == methodName).ToList();
    }

    public static IEnumerable<ParameterSyntax> GetFunctionParameters(MethodDeclarationSyntax method)
    {
        return method.ParameterList.Parameters.ToList();
    }
    
    public static TypeSyntax GetFunctionReturn(MethodDeclarationSyntax method)
    {
        return method.ReturnType;
    }
}
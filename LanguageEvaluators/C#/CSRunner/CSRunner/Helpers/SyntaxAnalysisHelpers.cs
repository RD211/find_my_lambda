using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSRunner.Helpers;

public static class SyntaxAnalysisHelpers
{

    /**
     * Gets all classes declared in the code.
     */
    public static IEnumerable<ClassDeclarationSyntax> GetClassDeclarations(SyntaxTree tree)
    {
        var root = tree.GetRoot();

        return root.DescendantNodes().OfType<ClassDeclarationSyntax>();
    }
    
    /**
     * Gets a class declaration with the name provided.
     */
    public static ClassDeclarationSyntax GetClassDeclarationByName(SyntaxTree tree, string name)
    {
        var root = tree.GetRoot();

        return root.DescendantNodes()
            .OfType<ClassDeclarationSyntax>().First(syntax => syntax.Identifier.Text == name);
    }

    /**
     * Gets all the methods of the provided class.
     */
    public static IEnumerable<MethodDeclarationSyntax> GetMethodsOfClass(ClassDeclarationSyntax classSyntax)
    {
        return classSyntax.Members.OfType<MethodDeclarationSyntax>();
    }
    
    /**
     * Gets the method with the provided name inside the class declaration.
     */
    public static MethodDeclarationSyntax GetMethodOfClassByName(ClassDeclarationSyntax classSyntax, string methodName)
    {
        return classSyntax.Members.OfType<MethodDeclarationSyntax>().First(syntax => syntax.Identifier.Text == methodName);
    }

    /**
     * Gets the return type of the method as SyntaxNode.
     */
    public static TypeSyntax GetFunctionReturn(MethodDeclarationSyntax method)
    {
        return method.ReturnType;
    }
    
    /**
     * Gets all the function parameters of a function as SyntaxNodes.
     */
    public static IEnumerable<TypeSyntax?> GetFunctionParameters(MethodDeclarationSyntax method)
    {
        if (method.ParameterList is null)
            throw new ArgumentException("Method has invalid parameter list.");
            
        return method.ParameterList.Parameters.Select(syntax => syntax.Type);
    }

    /**
     * Converts a SyntaxNodeType to an actual Reflection Type.
     */
    public static Type GetTypeFromNode(Compilation compilation, SyntaxTree tree, CSharpSyntaxNode node)
    {
        var semanticModel = compilation.GetSemanticModel(tree);

        var typeSymbol = semanticModel.GetTypeInfo(node).Type;
        if (typeSymbol == null) throw new ArgumentException("Invalid type syntax provided.");
        
        return GetTypeFromNode(typeSymbol);
    }
    
    /**
     * Given an TypeSymbol it will try and convert it to a normal reflection type.
     * This method supports only booleans, chars, ints, doubles, strings, arrays, tuples.
     * The tuples and arrays are created in a recursive manner.
     */
    public static Type GetTypeFromNode(ITypeSymbol typeSymbol)
    {
        var namedSymbol = typeSymbol as INamedTypeSymbol;
        var arrayTypeSymbol = typeSymbol as IArrayTypeSymbol;
        var specialType = typeSymbol.SpecialType;
        
        switch (specialType)
        {
            case SpecialType.System_Boolean:
                return typeof(bool);
            case SpecialType.System_Char:
                return typeof(char);
            case SpecialType.System_Int32:
                return typeof(int);
            case SpecialType.System_Double:
                return typeof(double);
            case SpecialType.System_String:
                return typeof(string);
            case SpecialType.None:
                
                if (namedSymbol is { IsTupleType: true })
                {
                    var subTypes = namedSymbol.TupleElements
                        .Select(symbol => GetTypeFromNode(symbol.Type)).ToArray();

                    return ReflectionHelpers.GetTupleType(subTypes.ToList());
                }

                if (arrayTypeSymbol!=null)
                {
                    var subType = GetTypeFromNode(arrayTypeSymbol.ElementType);
                    return Array.CreateInstance(subType,1).GetType();
                }
                
                throw new ArgumentException("Couldn't find type.");
            default:
                throw new ArgumentException("Couldn't find type.");
        }
    }
}
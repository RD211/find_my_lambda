using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSRunner.Helpers;

public static class SyntaxAnalysisHelpers
{

    /**
     * Gets all classes declared in the code.
     */
    public static IEnumerable<ClassDeclarationSyntax> GetClassDeclarations(Compilation compilation, SyntaxTree tree)
    {
        var semanticModel = compilation.GetSemanticModel(tree);
        var root = tree.GetRoot();

        return root.DescendantNodes().OfType<ClassDeclarationSyntax>();
    }
    
    /**
     * Gets a class declaration with the name provided.
     */
    public static ClassDeclarationSyntax GetClassDeclarationByName(Compilation compilation, SyntaxTree tree, string name)
    {
        var semanticModel = compilation.GetSemanticModel(tree);
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

                    return GetTupleType(subTypes);
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

    #region Horrible code

    /**
     * Gets a tuple type based on the generic arguments.
     * It needs to be done for each individual tuple type since it's actually a different class.
     */
    private static Type GetTupleType(IReadOnlyList<Type> types)
    {
        switch (types.Count)
        {
            case 1:
                return GetTupleType1(types[0]);
            case 2:
                return GetTupleType2(types[0],types[1]);
            case 3:
                return GetTupleType3(types[0],types[1], types[2]);
            case 4:
                return GetTupleType4(types[0],types[1], types[2], types[3]);
            case 5:
                return GetTupleType5(types[0],types[1], types[2], types[3], types[4]);
            case 6:
                return GetTupleType6(types[0],types[1], types[2], types[3], types[4], types[5]);
            case 7:
                return GetTupleType7(types[0],types[1], types[2], types[3], types[4], types[5], types[6]);
            case 8:
                return GetTupleType8(types[0],types[1], types[2], types[3], types[4], types[5], types[6], types[7]);
            default:
                return typeof(Tuple);
        }
    }
    
    private static Type GetTupleType1(Type t1)
    {
        var tupleType = typeof (ValueTuple<>);
        return tupleType.MakeGenericType(t1); 
    }
    
    private static Type GetTupleType2(Type t1, Type t2)
    {
        var tupleType = typeof (ValueTuple<,>);
        return tupleType.MakeGenericType(t1, t2); 
    }
    
    private static Type GetTupleType3(Type t1,Type t2,Type t3)
    {
        var tupleType = typeof (ValueTuple<,,>);
        return tupleType.MakeGenericType(t1,t2,t3); 
    }
    
    private static Type GetTupleType4(Type t1,Type t2,Type t3,Type t4)
    {
        var tupleType = typeof (ValueTuple<,,,>);
        return tupleType.MakeGenericType(t1,t2,t3,t4); 
    }
    
    private static Type GetTupleType5(Type t1,Type t2,Type t3,Type t4,Type t5)
    {
        var tupleType = typeof (ValueTuple<,,,,>);
        return tupleType.MakeGenericType(t1,t2,t3,t4,t5); 
    }
    
    private static Type GetTupleType6(Type t1,Type t2,Type t3,Type t4,Type t5, Type t6)
    {
        var tupleType = typeof (ValueTuple<,,,,,>);
        return tupleType.MakeGenericType(t1,t2,t3,t4,t5,t6); 
    }
    
    private static Type GetTupleType7(Type t1,Type t2,Type t3,Type t4,Type t5, Type t6, Type t7)
    {
        var tupleType = typeof (ValueTuple<,,,,,,>);
        return tupleType.MakeGenericType(t1,t2,t3,t4,t5,t6,t7); 
    }
    
    private static Type GetTupleType8(Type t1,Type t2,Type t3,Type t4,Type t5, Type t6, Type t7, Type t8)
    {
        var tupleType = typeof (ValueTuple<,,,,,,,>);
        return tupleType.MakeGenericType(t1,t2,t3,t4,t5,t6,t7,t8); 
    }

    #endregion
}
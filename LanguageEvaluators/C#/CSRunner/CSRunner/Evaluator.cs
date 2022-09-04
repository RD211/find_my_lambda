using System.Collections.Immutable;
using System.Reflection;
using CSRunner.Helpers;
using Microsoft.CodeAnalysis;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using CSRunner.Parser;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Scripting;

namespace CSRunner;

public class Evaluator
{
    private readonly MemoryCache _cache;
    private readonly CacheItemPolicy _cacheItemPolicy;
    private readonly RandomDataFactory _dataFactory;

    public Evaluator(MemoryCache cache, CacheItemPolicy cacheItemPolicy,  RandomDataFactory dataFactory)
    {
        _cache = cache;
        _cacheItemPolicy = cacheItemPolicy;
        _dataFactory = dataFactory;
    }

    public IEnumerable<string> Evaluate(string code, IEnumerable<string> inputs)
    {
        var (assembly, compilation, tree) = AssembleCode(code);
        var parameterType = 
            ReflectionHelpers.GetTupleType(GetParameterTypesOfLambdaFunction(compilation, tree).ToList()); 
        
        var classType = assembly.GetType("Lambda");
        if (classType == null) throw new ArgumentException("Code has wrong structure.");
        
        var instanceOfClass = Activator.CreateInstance(classType);

        var deserializedInputs = inputs.Select(s => InputParser.Parse(s, parameterType))
            .Select(o => ReflectionHelpers.GetElementsOfTuple(o as ITuple));

        var task = Task.Run(() =>
        {
            return deserializedInputs.Select(o => 
                classType.InvokeMember("lambda",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null,
                    instanceOfClass,
                    o.ToArray())).Select(InputParser.Export).Select(s => $"({s})");
        });

        var isCompletedSuccessfully = task.Wait(2000);

        if (isCompletedSuccessfully)
        {
            return task.Result;
        }
        else
        {
            throw new TimeoutException("The lambda has taken longer than the maximum time allowed.");
        }
    }

    public (string, string) GetTypes(string code)
    {
        try
        {
            var (_, compilation, tree) = AssembleCode(code);

            var returnType = $"({InputParser.Export(GetReturnTypeOfLambdaFunction(compilation, tree))})";
            var parameterTypes =
                $"({string.Join(',', GetParameterTypesOfLambdaFunction(compilation, tree).Select(InputParser.Export))})";

            return (parameterTypes, returnType);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public bool Verify(string code, int times = 100)
    {
        try
        {
            var (assembly, compilation, tree) = AssembleCode(code);
            var parameterTypes = GetParameterTypesOfLambdaFunction(compilation, tree).ToList();
            if (parameterTypes.Count > 8)
            {
                Console.WriteLine("Too many arguments provided in function lambda.");
                return false;
            }
            var classType = assembly.GetType("Lambda");
            if (classType == null) throw new ArgumentException("Code has wrong structure.");

            var instanceOfClass = Activator.CreateInstance(classType);
            for (var i = 0; i < times; i++)
            {
                var inputs = _dataFactory.GetRandomValuesOfTypes(parameterTypes);
                
                var task = Task.Run(() =>
                {
                    classType.InvokeMember("lambda", 
                        BindingFlags.Default | BindingFlags.InvokeMethod, 
                        Type.DefaultBinder, 
                        instanceOfClass,
                        inputs);
                });

                var isCompletedSuccessfully = task.Wait(2000);

                if (!isCompletedSuccessfully)
                {
                    throw new TimeoutException("The lambda has taken longer than the maximum time allowed.");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }

        return true;
    }

    private static IEnumerable<Type> GetParameterTypesOfLambdaFunction(Compilation compilation, SyntaxTree tree)
    {
        var classDeclaration = SyntaxAnalysisHelpers.GetClassDeclarationByName(tree, "Lambda");
        var method = SyntaxAnalysisHelpers.GetMethodOfClassByName(classDeclaration, "lambda");
        var parameters = SyntaxAnalysisHelpers.GetFunctionParameters(method);
        var types = 
            parameters.Select(syntax => SyntaxAnalysisHelpers.GetTypeFromNode(compilation, tree, syntax!)).ToList();

        return types;
    }
    
    private static Type GetReturnTypeOfLambdaFunction(Compilation compilation, SyntaxTree tree)
    {
        var classDeclaration = SyntaxAnalysisHelpers.GetClassDeclarationByName(tree, "Lambda");
        var method = SyntaxAnalysisHelpers.GetMethodOfClassByName(classDeclaration, "lambda");
        var returnTypeSyntax = SyntaxAnalysisHelpers.GetFunctionReturn(method);
        var type = SyntaxAnalysisHelpers.GetTypeFromNode(compilation, tree, returnTypeSyntax);

        return type;
    }

    private (Assembly, Compilation, SyntaxTree) AssembleCode(string code)
    {
        var hash = code.GetHashCode().ToString();
        (Assembly, Compilation, SyntaxTree) assemblyCompilationTreeTuple;
        
        if (_cache.Contains(hash))
        {
            assemblyCompilationTreeTuple = ((Assembly, Compilation, SyntaxTree))_cache.Get(hash)!;
        }
        else
        {
            assemblyCompilationTreeTuple = CompileCode(code);
            _cache.Add(new CacheItem(hash, assemblyCompilationTreeTuple), _cacheItemPolicy);
        }

        return assemblyCompilationTreeTuple;
    }

    private static (Assembly, Compilation, SyntaxTree) CompileCode(string code)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(code);
        
        var assemblyName = code.GetHashCode().ToString();

        var refs = new List<MetadataReference>();
        refs.AddRange(((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator).Select(r => MetadataReference.CreateFromFile(r)));

        var compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: new[] { syntaxTree },
            references: refs,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        
        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            var failures = result.Diagnostics.Where(diagnostic => 
                diagnostic.IsWarningAsError || 
                diagnostic.Severity == DiagnosticSeverity.Error);

            var diagnostics = failures.ToImmutableArray();

            foreach (var diagnostic in diagnostics)
            {
                Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
            }

            throw new CompilationErrorException("Code failed to compile.", diagnostics);
        }
        
        ms.Seek(0, SeekOrigin.Begin);
        var assembly = Assembly.Load(ms.ToArray());

        return (assembly, compilation, syntaxTree);
    }
}
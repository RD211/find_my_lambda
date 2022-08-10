using System.Collections.Immutable;
using System.Reflection;
using CSRunner.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Runtime.Caching;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;

namespace CSRunner;

public class Evaluator
{
    private readonly MemoryCache _cache;
    private readonly CacheItemPolicy _cacheItemPolicy;

    public Evaluator(MemoryCache cache, CacheItemPolicy cacheItemPolicy)
    {
        _cache = cache;
        _cacheItemPolicy = cacheItemPolicy;
    }

    public IEnumerable<object?> Evaluate(string code, IEnumerable<string[]> inputs)
    {
        var (assembly, compilation, tree) = AssembleCode(code);
        var returnType = GetReturnTypeOfLambdaFunction(compilation, tree);
        var parameterTypes = GetParameterTypesOfLambdaFunction(compilation, tree);
        
        var classType = assembly.GetType("Lambda");
        if (classType == null) throw new ArgumentException("Code has wrong structure.");
        
        var instanceOfClass = Activator.CreateInstance(classType);

        // Extreme reflection hackery inbound
        var deserializedInputs = 
            inputs.Select(strings => strings.Zip(parameterTypes))
            .Select(tuples => tuples.Select(tuple =>
            {
                return typeof(JsonConvert)
                    .GetMethods()!
                    .Where(info => info.Name == "DeserializeObject")
                    .FirstOrDefault(info => info.IsGenericMethod)!
                    .MakeGenericMethod(tuple.Second)
                    .Invoke(this, new object[] { tuple.First });
            }));

        var results = deserializedInputs.Select(o => 
            classType.InvokeMember("lambda",
            BindingFlags.Default | BindingFlags.InvokeMethod,
            null,
            instanceOfClass,
            o.ToArray()));
        
        return results;
    }

    private static IEnumerable<Type> GetParameterTypesOfLambdaFunction(Compilation compilation, SyntaxTree tree)
    {
        var classDeclaration = SyntaxAnalysisHelpers.GetClassDeclarationByName(compilation, tree, "Lambda");
        var method = SyntaxAnalysisHelpers.GetMethodOfClassByName(classDeclaration, "lambda");
        var parameters = SyntaxAnalysisHelpers.GetFunctionParameters(method);
        var types = 
            parameters.Select(syntax => SyntaxAnalysisHelpers.GetTypeFromNode(compilation, tree, syntax)).ToList();

        return types;
    }
    
    private static Type GetReturnTypeOfLambdaFunction(Compilation compilation, SyntaxTree tree)
    {
        var classDeclaration = SyntaxAnalysisHelpers.GetClassDeclarationByName(compilation, tree, "Lambda");
        var method = SyntaxAnalysisHelpers.GetMethodOfClassByName(classDeclaration, "lambda");
        var functionReturn = SyntaxAnalysisHelpers.GetFunctionReturn(method);
        var type = SyntaxAnalysisHelpers.GetTypeFromNode(compilation, tree, functionReturn);

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

    public static (Assembly, Compilation, SyntaxTree) CompileCode(string code)
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

    private string GenerateInputsFromCode(string code)
    {
        return "";
    }

    private async Task<Optional<object>> EvaluateCode(string code)
    {
        var result = await CSharpScript.EvaluateAsync<object>(code);
        return result ?? new Optional<object>();
    }
}
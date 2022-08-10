namespace CSRunner;

/**
 * This class provides random based on the type.
 * It recursively generates data for tuples and arrays.
 */
public class RandomDataFactory
{
    /**
     * The dictionary that maps the types to the functions.
     */
    private readonly Dictionary<Type, Func<Type, object>> _factory;
    
    /**
     * Constructor that takes a random instance to use in the
     * construction of the Dictionary.
     */
    public RandomDataFactory(Random random)
    {
        _factory = new Dictionary<Type, Func<Type, object>>
        {
            { typeof(int), _ => random.Next(-1000, 1000) },
            { typeof(double), _ => (random.NextDouble() - 0.5) * 2000 },
            { typeof(string), _ => random.Next(1000).ToString() },
            { typeof(char), _ => (char)random.Next(10,128) }, 
            { typeof(ValueTuple<>), GenerateValueTuple},
            { typeof(ValueTuple<,>), GenerateValueTuple},
            { typeof(ValueTuple<,,>), GenerateValueTuple},
            { typeof(ValueTuple<,,,>), GenerateValueTuple},
            { typeof(ValueTuple<,,,,>), GenerateValueTuple},
            { typeof(ValueTuple<,,,,,>), GenerateValueTuple},
            { typeof(ValueTuple<,,,,,,>), GenerateValueTuple},
            { typeof(ValueTuple<,,,,,,,>), GenerateValueTuple},
            // This takes an array and tries to create an array of length 10 of the specified type.
            { typeof(Array), (t) =>
            {
                var innerType = t.GetElementType();
                return typeof(RandomDataFactory)
                    .GetMethods()
                    .Where(info => info.Name == "GenerateArrayType")
                    .FirstOrDefault(info => info.IsGenericMethod)!
                    .MakeGenericMethod(innerType ?? throw new ArgumentException("Wrong type provided."))
                    .Invoke(this, new object[]{})!;
            }},
        };
    }

    /**
     * Returns a random object of the type provided.
     */
    public object GetRandomValueOfType(Type t)
    {
        if (_factory.ContainsKey(t))
        {
            return _factory[t](t);
        }

        if (t.IsGenericType)
        {
            var gt = t.GetGenericTypeDefinition();
            return _factory[gt](t);
        }

        if (t.IsArray)
        {
            return _factory[typeof(Array)](t);
        }

        throw new ArgumentException("Type is not part of allowed set.");
    }
    
    /**
     * Returns objects of types provided.
     */
    public object[] GetRandomValuesOfTypes(IEnumerable<Type> t)
    {
        return t.Select(GetRandomValueOfType).ToArray();
    }

    private object CreateTupleWithRuntimeType(Type[] genericTypes, object[] values)
    {
        return typeof(ValueTuple)
            .GetMethods()
            .Where(info => info.Name == "Create")
            .FirstOrDefault(info => info.IsGenericMethod && info.GetGenericArguments().Length == genericTypes.Length)!
            .MakeGenericMethod(genericTypes)
            .Invoke(this, values)!;
    }

    private object GenerateValueTuple(Type t)
    {
        var objs = t.GenericTypeArguments.Select(GetRandomValueOfType)
            .ToArray();
        return CreateTupleWithRuntimeType(t.GenericTypeArguments, objs);
    }

    public T[] GenerateArrayType<T>()
    {
        return Enumerable.Range(0, 10).Select(_ => GetRandomValueOfType(typeof(T))).Cast<T>().ToArray();
    }
}
namespace CSRunner.Helpers;

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
                if (innerType is null)
                {
                    throw new ArgumentException("Invalid type provided.");
                }
                
                return ReflectionHelpers.CreateArrayWithRuntimeType(
                    innerType,
                    Enumerable.Range(0, 10).Select(_ => GetRandomValueOfType(innerType)).ToArray());
            }},
        };
    }

    /**
     * Returns a random object of the type provided.
     */
    private object GetRandomValueOfType(Type t)
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

    private object GenerateValueTuple(Type t)
    {
        var objs = t.GenericTypeArguments.Select(GetRandomValueOfType)
            .ToArray();
        return ReflectionHelpers.CreateTupleWithRuntimeType(t.GenericTypeArguments, objs);
    }
}
namespace CSRunner;

public class RandomDataFactory
{
    private readonly Dictionary<Type, Func<Type, object>> _factory;
    
    public RandomDataFactory(Random random)
    {
        var random1 = random;
        _factory = new Dictionary<Type, Func<Type, object>>
        {
            { typeof(int), _ => random1.Next(-1000, 1000) },
            { typeof(double), _ => (random1.NextDouble() - 0.5) * 2000 },
            { typeof(string), _ => random1.Next(1000).ToString() },
            { typeof(char), _ => (char)random1.Next(10,128) },
            { typeof(Array), (t) => Array.CreateInstance(t, 
                (int)(_factory?[typeof(int)](typeof(int)) ?? throw new ArgumentException("Wrong type provided."))) },
            { typeof(Tuple<>), (t) =>
            {
                var types = t.GenericTypeArguments.Select(type => _factory?[type](type))
                    .ToList();
                return Tuple.Create(types[0]);
            }},
            { typeof(Tuple<,>), (t) =>
            {
                var types = t.GenericTypeArguments.Select(type => _factory?[type](type))
                    .ToList();
                return Tuple.Create(types[0],types[1]);
            }},
            { typeof(Tuple<,,>), (t) =>
            {
                var types = t.GenericTypeArguments.Select(type => _factory?[type](type))
                    .ToList();
                return Tuple.Create(types[0],types[1],types[2]);
            }},
            { typeof(Tuple<,,,>), (t) =>
            {
                var types = t.GenericTypeArguments.Select(type => _factory?[type](type))
                    .ToList();
                return Tuple.Create(types[0],types[1],types[2],types[3]);
            }},
            { typeof(Tuple<,,,,>), (t) =>
            {
                var types = t.GenericTypeArguments.Select(type => _factory?[type](type))
                    .ToList();
                return Tuple.Create(types[0],types[1],types[2],types[3],types[4]);
            }},
            { typeof(Tuple<,,,,,>), (t) =>
            {
                var types = t.GenericTypeArguments.Select(type => _factory?[type](type))
                    .ToList();
                return Tuple.Create(types[0],types[1],types[2],types[3],types[4],types[5]);
            }},
            { typeof(Tuple<,,,,,,>), (t) =>
            {
                var types = t.GenericTypeArguments.Select(type => _factory?[type](type))
                    .ToList();
                return Tuple.Create(types[0],types[1],types[2],types[3],types[4],types[5],types[6]);
            }},
            { typeof(Tuple<,,,,,,,>), (t) =>
            {
                var types = t.GenericTypeArguments.Select(type => _factory?[type](type))
                    .ToList();
                return Tuple.Create(types[0],types[1],types[2],types[3],types[4],types[5],types[6],types[7]);
            }}
        };
    }

    public object GetRandomValueOfType(Type t)
    {
        return _factory[t](t);
    }
    
    public object[] GetRandomValuesOfTypes(IEnumerable<Type> t)
    {
        return t.Select(GetRandomValueOfType).ToArray();
    }
}
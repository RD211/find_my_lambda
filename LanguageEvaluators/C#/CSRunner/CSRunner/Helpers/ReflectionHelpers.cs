using System.Runtime.CompilerServices;

namespace CSRunner.Helpers;

public static class ReflectionHelpers
{
    public static T Cast<T>(object o)
    {
        return (T)o;
    }
    
    public static object CreateTupleWithRuntimeType(Type[] genericTypes, object[] values)
    {
        return typeof(ValueTuple)
            .GetMethods()
            .Where(info => info.Name == "Create")
            .FirstOrDefault(info => info.IsGenericMethod && info.GetGenericArguments().Length == genericTypes.Length)!
            .MakeGenericMethod(genericTypes)
            .Invoke(null, values)!;
    }
    
    public static Array CreateArrayWithRuntimeType(Type genericType, object[] values)
    {
        var arr = Array.CreateInstance(genericType, values.Length);
        for (var i = 0; i < values.Length; i++)
        {
            arr.SetValue(values[i], i);
        }

        return arr;
    }
    
    public static List<object?> GetElementsOfTuple(ITuple? tuple)
    {
        if (tuple is null)
        {
            throw new ArgumentException("Tuple is null.");
        }
        return Enumerable
            .Range(0, tuple.Length)
            .Select(i => tuple[i])
            .ToList();
    }
    
    /**
     * Gets a tuple type based on the generic arguments.
     * It needs to be done for each individual tuple type since it's actually a different class.
     */
    public static Type GetTupleType(List<Type> types)
    {
        return types.Count switch
        {
            1 => GetTupleType1(types[0]),
            2 => GetTupleType2(types[0], types[1]),
            3 => GetTupleType3(types[0], types[1], types[2]),
            4 => GetTupleType4(types[0], types[1], types[2], types[3]),
            5 => GetTupleType5(types[0], types[1], types[2], types[3], types[4]),
            6 => GetTupleType6(types[0], types[1], types[2], types[3], types[4], types[5]),
            7 => GetTupleType7(types[0], types[1], types[2], types[3], types[4], types[5], types[6]),
            8 => GetTupleType8(types[0], types[1], types[2], types[3], types[4], types[5], types[6], types[7]),
            _ => throw new ArgumentException("Wrong number of elements provided to GetTupleType.")
        };
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
}
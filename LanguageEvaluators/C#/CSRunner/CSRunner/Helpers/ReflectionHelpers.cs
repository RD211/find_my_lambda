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
}
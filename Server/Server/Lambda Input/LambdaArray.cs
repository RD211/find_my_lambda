namespace Server.Lambda_Input;

public class LambdaArray : LambdaInput
{
    private readonly List<LambdaInput>? _values;
    private readonly LambdaInput? _type;


    public LambdaArray(List<LambdaInput> values)
    {
        if (values.Count == 0)
        {
            throw new ArgumentException("Wrong number of elements provided for LambdaArray. " +
                                        "There must be at least 1 to figure out the type.");
        }

        if (values.Any(input => input.IsGeneric()))
        {
            throw new ArgumentException("Bad arguments for lambda array. " +
                                        "Generic type provided in non generic constructor.");
        }
        _values = values;
    }
    
    public LambdaArray(LambdaInput type)
    {
        if (!type.IsGeneric())
        {
            throw new ArgumentException("Bad arguments for lambda array. " +
                                        "Not generic type provided in generic constructor.");
        }
        
        _type = type;
    }

    public override string GetStringRepresentation()
    {
        return _values is null ? $"[{_type!.GetStringRepresentation()}]" :
            $"[{string.Join(',', _values!.Select((t) => t.GetStringRepresentation()))}]";
    }

    public override Type? GetTypeRepresentation()
    {
        if (_values is null)
        {
            return Array.CreateInstance(_type.GetTypeRepresentation(), 1).GetType();
        }
        
        var typ = _values.First().GetTypeRepresentation();

        if (typ is null) return null;
        
        return Array.CreateInstance(typ, 0).GetType();
    }

    public override object? GetValueRepresentation()
    {
        return _values?.Select(input => input.GetValueRepresentation()).ToArray();
    }
    
    public override List<LambdaInput> Decompose()
    {
        return _values is null? new List<LambdaInput>{_type!} : _values!;
    }

    public override bool IsGeneric()
    {
        return _values is null;
    }
    
    public override LambdaInput ToGeneric()
    {
        return IsGeneric() ? this : new LambdaArray(_values!.First().ToGeneric());
    }
}
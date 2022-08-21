namespace Server.Lambda_Input;

public class LambdaTuple : LambdaInput
{
    private readonly List<LambdaInput> _values;

    public LambdaTuple(List<LambdaInput> values)
    {
        if (values.Count is > 8 or < 1)
        {
            throw new ArgumentException("Invalid number of arguments for LambdaTuple.");
        }

        if (values.Any(input => input.IsGeneric()) && values.Any(input => !input.IsGeneric()))
        {
            throw new ArgumentException("Some generic and some not generic parameters provided to Lambda Tuple.");
        }
        
        _values = values;
    }

    public override string GetStringRepresentation()
    {
        return 
            $"({string.Join(',', _values.Select((t) => t.GetStringRepresentation()))})";
    }

    public override Type? GetTypeRepresentation()
    {
        var typ = _values.Select(v => v.GetTypeRepresentation())
            .ToList();
        
        return _values.Count switch
        {
            1 => ValueTuple.Create(typ[0]).GetType(),
            2 => ValueTuple.Create(typ[0],typ[1]).GetType(),
            3 => ValueTuple.Create(typ[0],typ[1],typ[2]).GetType(),
            4 => ValueTuple.Create(typ[0],typ[1],typ[2],typ[3]).GetType(),
            5 => ValueTuple.Create(typ[0],typ[1],typ[2],typ[3],typ[4]).GetType(),
            6 => ValueTuple.Create(typ[0],typ[1],typ[2],typ[3],typ[4],typ[5]).GetType(),
            7 => ValueTuple.Create(typ[0],typ[1],typ[2],typ[3],typ[4],typ[5],typ[6]).GetType(),
            8 => ValueTuple.Create(typ[0],typ[1],typ[2],typ[3],typ[4],typ[5],typ[6],typ[7]).GetType(),
            _ => null
        };
    }

    public override object? GetValueRepresentation()
    {
        var typ = _values.Select(v => v.GetValueRepresentation())
            .ToList();
        
        return _values.Count switch
        {
            1 => ValueTuple.Create(typ[0]),
            2 => ValueTuple.Create(typ[0],typ[1]),
            3 => ValueTuple.Create(typ[0],typ[1],typ[2]),
            4 => ValueTuple.Create(typ[0],typ[1],typ[2],typ[3]),
            5 => ValueTuple.Create(typ[0],typ[1],typ[2],typ[3],typ[4]),
            6 => ValueTuple.Create(typ[0],typ[1],typ[2],typ[3],typ[4],typ[5]),
            7 => ValueTuple.Create(typ[0],typ[1],typ[2],typ[3],typ[4],typ[5],typ[6]),
            8 => ValueTuple.Create(typ[0],typ[1],typ[2],typ[3],typ[4],typ[5],typ[6],typ[7]),
            _ => null
        };
    }
    
    public override List<LambdaInput> Decompose()
    {
        return _values;
    }

    public override bool IsGeneric()
    {
        return _values[0].IsGeneric();
    }
    
    public override LambdaInput ToGeneric()
    {
        return IsGeneric() ? this : new LambdaTuple(_values.Select(input => input.ToGeneric()).ToList());
    }
}
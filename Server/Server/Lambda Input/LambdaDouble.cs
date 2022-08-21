namespace Server.Lambda_Input;

public class LambdaDouble : LambdaInput
{
    private readonly double? _value;
    
    public LambdaDouble(double input)
    {
        _value = input;
    }

    public LambdaDouble()
    {
    }

    public override string GetStringRepresentation()
    {
        return (_value is null? "double": _value.ToString())!;
    }

    public override Type? GetTypeRepresentation()
    {
        return typeof(double);
    }

    public override object? GetValueRepresentation()
    {
        return _value;
    }
    
    public override List<LambdaInput> Decompose()
    {
        return new List<LambdaInput> { this };
    }

    public override bool IsGeneric()
    {
        return _value is null;
    }
    
    public override LambdaInput ToGeneric()
    {
        return new LambdaDouble();
    }
}
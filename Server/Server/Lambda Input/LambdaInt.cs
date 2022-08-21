namespace Server.Lambda_Input;

public class LambdaInt : LambdaInput 
{
    private readonly int? _value;
    
    public LambdaInt(int input)
    {
        _value = input;
    }

    public LambdaInt()
    {
    }

    public override string GetStringRepresentation()
    {
        return (_value is null? "int": _value.ToString())!;
    }

    public override Type? GetTypeRepresentation()
    {
        return typeof(int);
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
        return new LambdaInt();
    }
}
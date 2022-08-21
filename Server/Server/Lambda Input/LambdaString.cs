namespace Server.Lambda_Input;

public class LambdaString : LambdaInput 
{
    private readonly string? _value;
    
    public LambdaString(string input)
    {
        _value = input;
    }

    public LambdaString()
    {
    }

    public override string GetStringRepresentation()
    {
        return _value is null? "string": $"\"{_value}\"";
    }

    public override Type GetTypeRepresentation()
    {
        return typeof(string);
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
        return new LambdaString();
    }
}
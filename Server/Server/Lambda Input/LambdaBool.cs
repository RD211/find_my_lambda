namespace Server.Lambda_Input;

public class LambdaBool : LambdaInput
{
    private readonly bool? _value;
    
    public LambdaBool(bool input)
    {
        _value = input;
    }

    public LambdaBool()
    {
    }

    public override string GetStringRepresentation()
    {
        return (_value is null? "bool": _value.ToString())!;
    }

    public override Type GetTypeRepresentation()
    {
        return typeof(bool);
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
        return new LambdaBool();
    }
}
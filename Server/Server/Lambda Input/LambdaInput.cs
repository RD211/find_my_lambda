namespace Server.Lambda_Input;

public abstract class LambdaInput
{
    public abstract string GetStringRepresentation();
    public abstract Type? GetTypeRepresentation();
    public abstract object? GetValueRepresentation();
    public abstract List<LambdaInput> Decompose();
    public abstract bool IsGeneric();
    public abstract LambdaInput ToGeneric();
}
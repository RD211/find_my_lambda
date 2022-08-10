namespace CSRunner;

public enum Action
{
    Evaluate,
    Verify,
    Ping
}
public class EvaluatePayload
{
    public Action? Action;
    public string? Language;
    public string[][]? Inputs;
    public string? Code;

    public EvaluatePayload(Action? action, string? language, string[][]? inputs, string? code)
    {
        Action = action;
        Language = language;
        Inputs = inputs;
        Code = code;
    }
}
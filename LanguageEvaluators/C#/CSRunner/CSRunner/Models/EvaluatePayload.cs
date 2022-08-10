namespace CSRunner;

public enum Action
{
    Evaluate,
    Verify,
    Ping
}

/**
 * The payload that can be received.
 */
public class EvaluatePayload
{
    public readonly Action? Action;
    public readonly string? Language;
    public readonly string[][]? Inputs;
    public readonly string? Code;

    public EvaluatePayload(Action? action, string? language, string[][]? inputs, string? code)
    {
        Action = action;
        Language = language;
        Inputs = inputs;
        Code = code;
    }
}
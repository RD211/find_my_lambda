namespace Server.Models;

public enum Action
{
    Evaluate,
    Verify,
    Types,
    Ping
}

/**
 * The payload that will be sent.
 */
public class EvaluatePayload
{
    public Action? Action;
    public string? Language;
    public string[]? Inputs;
    public string? Code;

    public EvaluatePayload(Action? action, string? language, string[]? inputs, string? code)
    {
        Action = action;
        Language = language;
        Inputs = inputs;
        Code = code;
    }
}
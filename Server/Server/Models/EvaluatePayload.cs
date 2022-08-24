namespace Server.Models;

/**
 * Action enum.
 * Specifies the action that the
 * language server must execute.
 */
public enum Action
{
    Evaluate,
    Verify,
    Types,
    Ping
}

/**
 * The payload that will be sent
 * to the language evaluation server for the specific language.
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
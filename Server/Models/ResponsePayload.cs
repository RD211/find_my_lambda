namespace Server.Models;

public enum Result
{
    OK,
    FAILURE
}

/**
 * The payload that will be received as response.
 * Result is either OK or FAILURE.
 * If result is OK then payload is the evaluation or true for verify.
 * If result is FAILURE then the payload is the error message.
 */
public class ResponsePayload
{
    public readonly Result? Result;
    public readonly object? Payload;

    public ResponsePayload(Result? result, object? payload)
    {
        Result = result;
        Payload = payload;
    }
}
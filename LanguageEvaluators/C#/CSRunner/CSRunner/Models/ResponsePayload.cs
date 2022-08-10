namespace CSRunner;

public enum Result
{
    OK,
    FAILURE
}

public class ResponsePayload
{
    public Result? Result;
    public object? Payload;

    public ResponsePayload(Result? result, object? payload)
    {
        Result = result;
        Payload = payload;
    }
}
using System.Net;
using System.Net.Sockets;
using System.Runtime.Caching;
using Newtonsoft.Json;

namespace CSRunner;

public static class CsRunner
{
    private static Evaluator _evaluator;
    private static SocketServer? _socketServer;
    
    private static void ReadInput(string jsonInput, Socket socket)
    {
        EvaluatePayload? payload;
        try
        {
            payload = JsonConvert.DeserializeObject<EvaluatePayload>(jsonInput);
        }
        catch (Exception e)
        {
            var responseJson = JsonConvert.SerializeObject(new ResponsePayload(Result.FAILURE, e.Message));
            SocketServer.Send(socket, $"{responseJson}<EOF>");
            return;
        }
        Console.WriteLine("Passed deserialization.");

        if (payload?.Code is null || payload.Action is null)
        {
            var responseJson = JsonConvert.SerializeObject(new ResponsePayload(Result.FAILURE, "JSON is invalid."));
            SocketServer.Send(socket, $"{responseJson}<EOF>");
            return;
        }

        ResponsePayload response;
        switch (payload.Action)
        {
            case Action.Evaluate:
                try
                {
                    if (payload.Inputs is null)
                    {
                        response = new ResponsePayload(Result.FAILURE, "JSON is invalid.");
                        break;
                    }
                    
                    var resultEvaluate = _evaluator.Evaluate(payload.Code, payload.Inputs);
                    response = new ResponsePayload(Result.OK, resultEvaluate);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    response = new ResponsePayload(Result.FAILURE, e.Message);
                }
                break;
            case Action.Verify:
                try
                {
                    var resultEvaluate = _evaluator.Verify(payload.Code);
                    if (resultEvaluate)
                        response = new ResponsePayload(Result.OK, true);
                    else
                        response = new ResponsePayload(Result.FAILURE, "Something went wrong while verifying the program.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    response = new ResponsePayload(Result.FAILURE, "Something went wrong while verifying the program.");
                }
                break;
            case Action.Ping:
                response = new ResponsePayload(Result.OK, "Pong");
                break;
            default:
                response = new ResponsePayload(Result.FAILURE, "Action not recognized.");
                break;
        }
        
        SocketServer.Send(socket, $"{JsonConvert.SerializeObject(response)}<EOF>");
        
    }
    
    public static void Main(string[] args)
    {
        _evaluator = new Evaluator(
            new MemoryCache("compilation_memory"), 
            new CacheItemPolicy(), 
            new RandomDataFactory(new Random()));
        
        _socketServer = new SocketServer(new IPAddress(new byte[] { 127, 0, 0, 1 }), 11000, ReadInput);
        _socketServer.StartListening();
    }
}
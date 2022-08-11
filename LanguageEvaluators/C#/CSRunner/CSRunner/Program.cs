using System.Net;
using System.Net.Sockets;
using System.Runtime.Caching;
using CSRunner.Communication;
using CSRunner.Helpers;
using CSRunner.Models;
using CSRunner.Parser;
using Newtonsoft.Json;
using Action = CSRunner.Models.Action;

namespace CSRunner;

public static class CsRunner
{
    private static readonly Evaluator Evaluator = new(
        new MemoryCache("compilation_memory"), 
        new CacheItemPolicy(), 
        new RandomDataFactory(new Random()));
    
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

                    var resultEvaluate = Evaluator.Evaluate(payload.Code, payload.Inputs);
                    response = new ResponsePayload(Result.OK, resultEvaluate.Select(InputParser.Export).ToArray());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    response = new ResponsePayload(Result.FAILURE, "Code failed to evaluate.");
                }
                
                break;
            case Action.Verify:
                try
                {
                    var resultEvaluate = Evaluator.Verify(payload.Code);
                    response = resultEvaluate ? 
                        new ResponsePayload(Result.OK, "true") : 
                        new ResponsePayload(Result.FAILURE, "Something went wrong while verifying the program.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    response = new ResponsePayload(Result.FAILURE, "Something went wrong while verifying the program.");
                }
                break;
            case Action.Ping:
                SocketServer.Send(socket, $"Pong<EOF>");
                return;
            default:
                response = new ResponsePayload(Result.FAILURE, "Action not recognized.");
                break;
        }

        string resultingPayload;
        
        try
        {
            resultingPayload = JsonConvert.SerializeObject(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            var serializedResponse = JsonConvert.SerializeObject(new ResponsePayload(Result.FAILURE,
                "Something went wrong."));
            SocketServer.Send(socket, $"{serializedResponse}<EOF>");
            return;
        }
        
        SocketServer.Send(socket, $"{resultingPayload}<EOF>");
        
    }
    
    public static void Main(string[] args)
    {

        var ip = new byte[] { 127, 0, 0, 1 };
        var port = 11000;
        try
        {
            ip = args[0].Split(".").Select(byte.Parse).ToArray();
            port = int.Parse(args[1]);
        }
        catch (Exception _)
        {
            Console.WriteLine("Running defaults.");
        }
        
        Console.WriteLine($"Running on ip: {ip[0]}.{ip[1]}.{ip[2]}.{ip[3]} port: {port}");

        _socketServer = new SocketServer(new IPAddress(ip), port, ReadInput);
        _socketServer.StartListening();
    }
}
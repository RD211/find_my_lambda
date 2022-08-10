using System.Net;
using System.Net.Sockets;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Newtonsoft.Json;

namespace CSRunner;

public static class CsRunner
{
    private static SocketServer? _socketServer;
    
    private static async void ReadInput(string jsonInput, Socket socket)
    {
        var payload = JsonConvert.DeserializeObject<EvaluatePayload>(jsonInput);
        if (payload is null)
        {
            SocketServer.Send(socket, "ERROR: JSON is invalid.");
            return;
        }
        
        switch (payload.Action)
        {
            case "EVALUATE":
                //var resultEvaluate = 
                    //await Evaluator.(payload.Code, payload.Inputs);
                
                //SocketServer.Send(socket,
                //    resultEvaluate.HasValue
                //        ? $"SUCCESS: {resultEvaluate.Value}"
                //        : $"CODE_ERROR: Something went wrong while evaluating the program.");
                break;
            case "VERIFY":
                //var resultVerify = await EvaluateCode(payload.code, );
                //SocketServer.Send(socket,
                //    resultVerify.HasValue
                //        ? $"SUCCESS: true"
                //        : $"CODE_ERROR: Something went wrong while verifying the program.");
                break;
            default:
                SocketServer.Send(socket, "ERROR: action not recognized.");
                break;
        }
        
    }
    
    public static void Main(string[] args)
    {
        _socketServer = new SocketServer(new IPAddress(new byte[] { 127, 0, 0, 1 }), 11000, ReadInput);
        _socketServer.StartListening();
    }
}
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Server.Models;

namespace Server.Communication;

public static class LanguageServer
{
    public static ResponsePayload Send(EvaluatePayload evaluatePayload)
    {
        var bytes = new byte[2048];
        var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        var ipAddress = ipHostInfo.AddressList[0];
        var remoteEp = new IPEndPoint(new IPAddress(new byte[]{127,0,0,1}), 11000);

        var sender = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        sender.Connect(remoteEp);

        var msg = Encoding.ASCII.GetBytes($"{JsonConvert.SerializeObject(evaluatePayload)}<EOF>");

        sender.Send(msg);

        var bytesRec = sender.Receive(bytes);
        var response =
            JsonConvert.DeserializeObject<ResponsePayload>(Encoding.ASCII.GetString(bytes, 0, bytesRec));

        if (response is null)
        {
            throw new Exception("Something went wrong while serializing the response.");
        }

        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
        return response;
    }
}
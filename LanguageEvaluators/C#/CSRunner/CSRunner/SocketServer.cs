using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CSRunner;

public class StateObject
{
    public const int BufferSize = 1024;
    public readonly byte[] Buffer = new byte[BufferSize];
    public readonly StringBuilder Sb = new();
    public Socket? WorkSocket;
}  

public class SocketServer
{
    private readonly IPAddress _ipAddress;
    private readonly int _port;
    private Action<string, Socket> _receiveLambda;
    private readonly ManualResetEvent _allDone = new(false);

    public SocketServer(IPAddress ipAddress, int port, Action<string, Socket> receiveLambda)
    {
        _ipAddress = ipAddress;
        _port = port;
        _receiveLambda = receiveLambda;
    }

    public void StartListening()
    {
        var localEndPoint = new IPEndPoint(_ipAddress, _port);  

        var listener = new Socket(_ipAddress.AddressFamily,  
            SocketType.Stream, ProtocolType.Tcp );  
  
        try {  
            listener.Bind(localEndPoint);
            listener.Listen(100);
            
            while (true)
            {
                _allDone.Reset();
                
                Console.WriteLine("Waiting for a connection...");  
                listener.BeginAccept(
                    AcceptCallback,  
                    listener );  

                _allDone.WaitOne();  
            }
        } catch (Exception e) {  
            Console.WriteLine(e.ToString());  
        }
    }

    private void AcceptCallback(IAsyncResult ar)
    {
        _allDone.Set();  
  
        var listener = (Socket) ar.AsyncState!;  
        var handler = listener.EndAccept(ar);  
  
        var state = new StateObject
        {
            WorkSocket = handler
        };
        
        handler.BeginReceive( state.Buffer, 0, StateObject.BufferSize, 0,  
            ReadCallback, state);  
    }

    private void ReadCallback(IAsyncResult ar)
    {
        
        var state = (StateObject) ar.AsyncState!;  
        var handler = state.WorkSocket;  
  
        var bytesRead = handler.EndReceive(ar);

        if (bytesRead <= 0) return;
        
        state.Sb.Append(Encoding.ASCII.GetString(  
            state.Buffer, 0, bytesRead));  
        
        var content = state.Sb.ToString();  
        if (content.IndexOf("<EOF>", StringComparison.Ordinal) > -1) {
            Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",  
                content.Length, content );
            _receiveLambda(content, handler);
            Send(handler, content);
        } else {  
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,  
                ReadCallback, state);  
        }
    }

    public static void Send(Socket handler, string data)
    {
        var byteData = Encoding.ASCII.GetBytes(data);  
  
        handler.BeginSend(byteData, 0, byteData.Length, 0,  
            SendCallback, handler);  
    }

    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            var handler = (Socket) ar.AsyncState;  
  
            if (handler != null)
            {
                var bytesSent = handler.EndSend(ar);  
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);
            }

            handler.Shutdown(SocketShutdown.Both);  
            handler.Close();  
  
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());  
        }  
    }
}
using System.Net;
using System.Net.Sockets;
using OryxEngine.Networking.Sockets;
using Serilog;

namespace Tester;

public class Server(int port)
{
    private readonly CancellationTokenSource _cts = new ();
    private readonly SocketBase _listener = new(SocketType.Stream, ProtocolType.Tcp);
    private readonly int _port = port;
    

    private readonly List<SocketBase> _connectedSockets = [];
    public void Start(int backlog = 10)
    {
        _cts.Token.Register(OnCancel);
        
        _listener.Bind(new IPEndPoint(IPAddress.Any, _port));
        _listener.Listen(backlog);

        Task.Run(Listen, _cts.Token);
    }

    private void OnCancel() {
        Log.Information("Disconnecting: {0} sockets", _connectedSockets.Count);
        for (var i = 0; i < _connectedSockets.Count; i++) {
            _connectedSockets[i].Disconnect(false);
        }
        
        _connectedSockets.Clear();
    }

    private int _counter = 10;
    
    public void Tick() 
    {
        _counter--;
        if (_counter > 0)
            return;

        if(_cts.IsCancellationRequested)
            return;
        
        Log.Information("Closing server");
        _cts.Cancel();
    }
    
    private Task Listen()
    {
        Thread.CurrentThread.IsBackground = true;
        Log.Information("Server.Listener Started");
        
        while (!_cts.IsCancellationRequested) {
            Thread.Sleep(1); //sleep the shortest possible time
            var result = _listener.Accept();
            if(!result.IsSuccess) //Some exception happened
                continue;
            
            var socket = result.Value;
            _connectedSockets.Add(socket);
            Log.Information("New client: {ip}", socket.RemoteEndPoint?.ToString() ?? "Ip is null");

        }
        
        Log.Information("Server.Listener Stopped");
        return Task.CompletedTask;
    }
}
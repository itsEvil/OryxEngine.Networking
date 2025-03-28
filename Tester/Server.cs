using System.Net;
using System.Net.Sockets;
using OryxEngine.Memory.Optionals;
using OryxEngine.Networking.Configuration;
using OryxEngine.Networking.PacketHandler;
using OryxEngine.Networking.Sockets;
using OryxEngine.Networking.Statuses;
using OryxEngine.Networking.Validators;
using Serilog;
using TcpClient = OryxEngine.Networking.TcpClient;

namespace Tester;

public class Server
{
    private readonly CancellationTokenSource _cts = new ();
    private readonly SocketBase _listener = new(SocketType.Stream, ProtocolType.Tcp);
    private readonly int _port;
    

    private readonly List<SocketBase> _connectedSockets = [];
    private readonly List<TcpClient> _connectedClients = [];
    private readonly List<TcpClient> _toRemove = [];
    private readonly List<TcpClient> _toAdd = [];

    private readonly SocketOptions _options = new()
    {
        Blocking = true,
        NoDelay = false,
        ReceiveThreadSleepTime = 8,
        PacketHandler = new PacketLogger(),
        Validator = new ValidatorLogger(),
        BufferSize = 4192,
        EndianMode = EndianMode.Big,
    };

    private readonly ActionOptions _actionOptions = new();
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

    public Server(int port)
    {
        _port = port;
        _actionOptions.OnDisconnect += OnDisconnect;
    }

    private static void OnDisconnect(TcpClient client, string message)
    {
        Log.Information("Client disconnected: {message}", message);
    }
    public void Tick()
    {
        foreach (var c in _toAdd)
            _connectedClients.Add(c);
        
        foreach(var c in _toRemove)
            _connectedClients.Remove(c);

        for (var i = 0; i < _connectedClients.Count; i++)
        {
            var c = _connectedClients[i];
            var result = c.Tick();
            if (!result.IsSuccess && result.Value != SocketStatus.Disconnected)
                continue;

            _toRemove.Add(c);
        }

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
            var client = new TcpClient(socket, _options, _actionOptions);
            
            _connectedSockets.Add(socket);
            _toAdd.Add(client);
            Log.Information("New client: {ip}", socket.RemoteEndPoint?.ToString() ?? "Ip is null");
        }
        
        Log.Information("Server.Listener Stopped");
        return Task.CompletedTask;
    }
}
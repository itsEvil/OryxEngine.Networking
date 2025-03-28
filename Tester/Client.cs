using OryxEngine.Memory.Optionals;
using OryxEngine.Networking;
using OryxEngine.Networking.PacketHandler;
using OryxEngine.Networking.Statuses;
using OryxEngine.Networking.Validators;
using OryxEngine.Optionals;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Tester;

public class Client(TcpClient client)
{
    private readonly TcpClient _client = client;
    public static Client Create(ILogger logger)
    {
        var builder = Builder.Create();

        builder.ConfigureOptions(o => {
            o.Blocking = true;
            o.NoDelay = false;
            o.ReceiveThreadSleepTime = 8;
            o.PacketHandler = new BaseHandler();
            o.Validator = new BaseValidator();
            o.BufferSize = 4192;
            o.EndianMode = EndianMode.Big;
        });

        builder.ConfigureActions(o =>
        {
            //Called when TcpClient.Disconnect(string) is called
            o.OnDisconnect += OnDisconnect;
        });
        
        return new Client(builder.Build());
    }
    
    public void Connect(string host, int port) {
        var result = _client.Connect(host, port);
        result.Handle(OnConnect, OnBadConnect);
    }

    public void Tick()
    {
        var result = _client.Tick();
        result.Handle(OnTick, OnTickFailure);
    }
    //Handlers
    private static void OnTick(SocketStatus result)
    {
        Log.Error("Client Tick Result: {result}", result);

        if (result != SocketStatus.Disconnected) 
            return;
        
        Entry.Stop();
    }
    private static void OnTickFailure(Error error)
    {
        Log.Error("Tick Error: {error}", error);
    }
    private static void OnConnect(SocketStatus value)
    {
        Log.Information("Connected, value: {value}", value);
    }
    private static void OnDisconnect(TcpClient client, string reason)
    {
        Log.Information("Client: {client} disconnected, reason: {reason}", client, reason);
    }
    private static void OnBadConnect(Error error)
    {
        Log.Information("Failed connection, error: {Error}", error);
    }
}
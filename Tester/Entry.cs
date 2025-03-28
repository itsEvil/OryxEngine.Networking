using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.ILogger;

namespace Tester;

// ReSharper disable once ClassNeverInstantiated.Global
internal class Entry
{
    private static bool _terminate = false;
    public static void Main(string[] args)
    {
        var serilogLoggerFactory = new SerilogLoggerFactory();
        var iLogger = serilogLoggerFactory.CreateLogger("Tester");

        var logger = new LoggerConfiguration().WriteTo.ILogger(
            iLogger).WriteTo.Console().CreateLogger();
        
        Log.Logger = logger;

        var server = new Server(8080);
        server.Start();
        
        var client = Client.Create(iLogger);
        client.Tick();
        
        Thread.Sleep(1_000); //Wait one second beforing trying to connect
        client.Connect("127.0.0.1", 8080);
        while (!_terminate) {
            Thread.Sleep(1000 / 20);
            server.Tick();
            client.Tick();
        }
    }
    public static void Stop() 
    {
        Log.Information("Stopping...");
        _terminate = true;
    }
}
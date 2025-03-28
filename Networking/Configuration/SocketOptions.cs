using Microsoft.Extensions.Logging;
using OryxEngine.Memory.Optionals;
using OryxEngine.Networking.PacketHandler;
using OryxEngine.Networking.Validators;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Configuration;

public class SocketOptions
{
    /// <summary>
    /// Sets SocketBase.Blocking
    /// </summary>
    public bool Blocking = true;
    /// <summary>
    /// Sets SocketBase.NoDelay
    /// </summary>
    public bool NoDelay = false;
    /// <summary>
    /// Sets how long the Receive Thread for each TcpClient sleeps
    /// </summary>
    public int ReceiveThreadSleepTime = 8;
    /// <summary>
    /// The Endian Mode of the Reader and Writer in the TcpClient
    /// </summary>
    public EndianMode EndianMode = EndianMode.Big;
    /// <summary>
    /// The Buffer Size that gets created for Reader and Writer 
    /// </summary>
    public int BufferSize = 8192;
    
    /// <summary>
    /// The Validator to use to check if the packet header is valid
    /// </summary>
    public IValidator Validator = new BaseValidator();
    /// <summary>
    /// The Handler to use to get an instance of a packet and handle it
    /// </summary>
    public IPacketHandler PacketHandler = new BaseHandler();
}
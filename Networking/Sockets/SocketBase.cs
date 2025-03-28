using System.Net.Sockets;
using OryxEngine.Networking.Statuses;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Sockets;
public partial class SocketBase : Socket
{
    public SocketBase(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : base(addressFamily, socketType, protocolType) { }
    public SocketBase(SafeSocketHandle handle) : base(handle) { }
    /// <summary>
    /// Only supported on Windows
    /// </summary>
#pragma warning disable CA1416
    public SocketBase(SocketInformation socketInformation) : base(socketInformation) { }
#pragma warning restore CA1416

    public SocketBase(SocketType socketType, ProtocolType protocolType) : base(socketType, protocolType) { }
}
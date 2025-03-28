using System.Net.Sockets;
using OryxEngine.Networking.Statuses;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Sockets;
public partial class SocketBase : Socket
{
    public SocketBase(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : base(addressFamily, socketType, protocolType)
    {
        
    }

    public SocketBase(SafeSocketHandle handle) : base(handle)
    {
    }

    public SocketBase(SocketInformation socketInformation) : base(socketInformation)
    {
    }

    public SocketBase(SocketType socketType, ProtocolType protocolType) : base(socketType, protocolType)
    {
    }
}
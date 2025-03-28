using System.Net.Sockets;
using System.Runtime.CompilerServices;
using OryxEngine.Optionals;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Sockets;
//Done
//All Socket.Accept(...) methods
public partial class SocketBase
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<SocketBase> Accept()
    {
        try
        {
            var socket = base.Accept();
            return new SocketBase(socket.SafeHandle);
        }
        catch (Exception e)
        {
            return e;
        }
    }
}
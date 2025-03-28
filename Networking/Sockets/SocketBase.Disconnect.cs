using System.Runtime.CompilerServices;
using OryxEngine.Networking.Statuses;
using OryxEngine.Optionals;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Sockets;
//Done
//All Socket.Disconnect(...) methods
public partial class SocketBase
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<SocketStatus> Disconnect(bool reuseSocket)
    {
        try
        {
            base.Disconnect(reuseSocket);
        }
        catch (Exception e)
        {
            return e;
        }
        
        return SocketStatus.Disconnected;
    }
}
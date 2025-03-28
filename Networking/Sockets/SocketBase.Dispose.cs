using System.Runtime.CompilerServices;
using OryxEngine.Networking.Statuses;
using OryxEngine.Optionals;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Sockets;
//Done
//All Socket.Dispose(...) methods
public partial class SocketBase
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<SocketStatus> Dispose(bool disposing)
    {
        try
        {
            base.Dispose(disposing);
        }
        catch (Exception e)
        {
            return e;
        }

        return SocketStatus.Connected;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<SocketStatus> Dispose()
    {
        try
        {
            base.Dispose();
        }
        catch (Exception e)
        {
            return e;
        }

        return SocketStatus.Connected;
    }
}
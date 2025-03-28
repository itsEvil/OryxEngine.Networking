using System.Net;
using System.Runtime.CompilerServices;
using OryxEngine.Networking.Statuses;
using OryxEngine.Optionals;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Sockets;
//Done
//All Socket.Bind(...) methods
public partial class SocketBase
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<SocketStatus> Bind(EndPoint endPoint)
    {
        try
        {
            base.Bind(endPoint);
        }
        catch (Exception e)
        {
            return e;
        }

        return SocketStatus.Bound;
    }
}
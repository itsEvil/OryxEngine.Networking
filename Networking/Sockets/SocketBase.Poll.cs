using System.Net.Sockets;
using System.Runtime.CompilerServices;
using OryxEngine.Optionals;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Sockets;
//Done
//All Socket.Poll(...) methods
public partial class SocketBase
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<bool> Poll(int microSeconds, SelectMode mode)
    {
        try
        {
            return base.Poll(microSeconds, mode);
        }
        catch (SocketException e)
        {
            return e;
        }
    }
}
using System.Net;
using System.Runtime.CompilerServices;
using OryxEngine.Networking.Statuses;
using OryxEngine.Optionals;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Sockets;
//Done
//All Socket.Connect(...) methods
public partial class SocketBase
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<SocketStatus> Connect(string host, int port)
    {
        try
        {
            base.Connect(host, port);
        }
        catch (Exception e)
        {
            return e;
        }
        
        return SocketStatus.Connected;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<SocketStatus> Connect(EndPoint endpoint)
    {
        try
        {
            base.Connect(endpoint);
        }
        catch (Exception e)
        {
            return e;
        }
        
        return SocketStatus.Connected;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<SocketStatus> Connect(IPAddress[] ipAddresses, int port)
    {
        try
        {
            base.Connect(ipAddresses, port);
        }
        catch (Exception e)
        {
            return e;
        }
        
        return SocketStatus.Connected;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<SocketStatus> Connect(IPAddress ipAddress, int port)
    {
        try
        {
            base.Connect(ipAddress, port);
        }
        catch (Exception e)
        {
            return e;
        }
        
        return SocketStatus.Connected;
    }
    
    
}
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using OryxEngine.Networking.Statuses;
using OryxEngine.Optionals;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Sockets;

//Done
//All Socket.Send(byte[]... ); methods

//TODO
//Finish adding rest of the Socket.Send methods
public partial class SocketBase 
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<SocketStatus> Send(byte[] data)
    {
        try
        {
            base.Send(data);
        }
        catch (Exception e)
        {
            return e;
        }

        return SocketStatus.Connected;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<SocketStatus> Send(byte[] data, SocketFlags socketFlags)
    {
        try
        {
            base.Send(data, socketFlags);
        }
        catch (Exception e)
        {
            return e;
        }
        
        return SocketStatus.Connected;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<SocketStatus> Send(byte[] data, int offset, SocketFlags socketFlags)
    {
        try
        {
            base.Send(data, offset, socketFlags);
        }
        catch (Exception e)
        {
            return e;
        }
        
        return SocketStatus.Connected;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<SocketStatus> Send(byte[] data, int offset, int size, SocketFlags socketFlags)
    {
        try
        {
            base.Send(data, offset, size, socketFlags);
        }
        catch (Exception e)
        {
            return e;
        }
        
        return SocketStatus.Connected;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<SocketStatus> Send(byte[] data, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
    {
        errorCode = default;
        try
        {
            base.Send(data, offset, size, socketFlags, out errorCode);
        }
        catch (Exception e)
        {
            return e;
        }
        
        return SocketStatus.Connected;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsConnected()
    {
        var result = Poll(1, SelectMode.SelectRead);
        if (!result.IsSuccess)
            return false;

        return !(Available == 0 && result.Value);
    }
}
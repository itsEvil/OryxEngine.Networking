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
    public new Option<int> Receive(byte[] buffer)
    {
        try
        {
            return base.Receive(buffer);
        }
        catch (Exception e)
        {
            return e;
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<int> Receive(byte[] buffer, SocketFlags socketFlags)
    {
        try
        {
            return base.Receive(buffer, socketFlags);
        }
        catch (Exception e)
        {
            return e;
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<int> Receive(byte[] buffer, int size, SocketFlags socketFlags)
    {
        try
        {
            return base.Receive(buffer, size, socketFlags);
        }
        catch (Exception e)
        {
            return e;
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<int> Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
    {
        try
        {
            return base.Receive(buffer, offset, size, socketFlags);
        }
        catch (Exception e)
        {
            return e;
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new Option<int> Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError error)
    {
        error = default;
        try
        {
            return base.Receive(buffer, offset, size, socketFlags, out error);
        }
        catch (Exception e)
        {
            return e;
        }
    }
}
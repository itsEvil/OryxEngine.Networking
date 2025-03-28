using OryxEngine.Memory.Optionals;
using OryxEngine.Networking.PacketHandler;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Packets;

//  Packet header
// ---------------- | ----------- | ------------
// length (ushort) | id (ushort) | data (byte[])  
// -------------- | ----------- | -------------
public interface IPacket
{
    /// <exception cref="NotImplementedException"></exception>
    public void Write(Writer writer) { throw new NotImplementedException(); }
    /// <exception cref="NotImplementedException"></exception>
    public void Handle(IPacketHandler handler) { throw new NotImplementedException(); }
}
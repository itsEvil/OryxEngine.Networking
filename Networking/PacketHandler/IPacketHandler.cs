using OryxEngine.Memory.Optionals;
using OryxEngine.Networking.Packets;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.PacketHandler;

public interface IPacketHandler
{
    public IPacket? GetAndReadPacket(int id, Reader r) { throw new NotImplementedException(); }
    public void HandlePacket(IPacket packet) { throw new NotImplementedException(); }
}
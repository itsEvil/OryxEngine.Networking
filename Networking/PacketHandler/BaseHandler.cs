using OryxEngine.Memory.Optionals;
using OryxEngine.Networking.Packets;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.PacketHandler;
public class BaseHandler : IPacketHandler
{
    /// <summary>
    /// Creates an instance of IPacket for a valid packet id or returns null
    /// </summary>
    public IPacket? GetAndReadPacket(ushort id, Reader r) {
        return id switch {
            ushort.MaxValue => new PingExample(r),
            _ => null,
        };
    }
    /// <summary>
    /// Middleware for handling packets
    /// </summary>
    public void HandlePacket(IPacket packet) {
        packet.Handle(this);
    }
}
using OryxEngine.Memory.Optionals;
using OryxEngine.Networking.PacketHandler;
using OryxEngine.Networking.Packets;
using Serilog;

namespace Tester;

public class PacketLogger : IPacketHandler
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
        Log.Information("Handling packet {0}", packet.GetType().Name);
        packet.Handle(this);
    }
}
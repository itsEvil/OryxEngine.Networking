using OryxEngine.Memory.Optionals;
using OryxEngine.Networking.PacketHandler;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Packets;
/// <summary>
/// Packets can be either receive or send packets so we need to implement both cases
/// </summary>
public sealed class PingExample : IPacket
{
    private readonly DateTime _time;
    public PingExample(Reader r) {
        _time = DateTime.FromBinary(r.ReadInt64());
    }
    public PingExample(DateTime time) {
        _time = time;
    }
    /// <summary>
    /// Wrote on the main thread
    /// </summary>
    /// <param name="writer"></param>
    public void Write(Writer writer) {
        writer.Write(DateTime.UtcNow.ToBinary());
    }
    /// <summary>
    /// Handled on the main thread
    /// </summary>
    /// <param name="handler"></param>
    public void Handle(IPacketHandler handler) {
        Console.WriteLine("Ping Time: {0}", _time);
    }
}
// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Statuses;

public enum SocketStatus
{
    Failure,
    Connected,
    AlreadyConnected,
    CancellationRequested,
    NotConnected,
    Disconnected,
    Bound,
}
// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Configuration;

public class ActionOptions
{
    /// <summary>
    /// string parameter is the reason of disconnection, by default its 'unknown'
    /// </summary>
    public Action<TcpClient, string>? OnDisconnect = null;
}
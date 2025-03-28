// ReSharper disable once CheckNamespace

using OryxEngine.Networking.Configuration;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking;

public class Builder {
    private readonly SocketOptions _socketOptions = new SocketOptions();
    private readonly ActionOptions _actionOptions = new ActionOptions();
    private Builder() { }
    public static Builder Create() => new Builder();
    
    public void ConfigureOptions(Action<SocketOptions> configureOptions = null) {
        configureOptions?.Invoke(_socketOptions);
    }
    public void ConfigureActions(Action<ActionOptions> configureOptions = null) {
        configureOptions?.Invoke(_actionOptions);
    }
    public TcpClient Build() {
        return new TcpClient(_socketOptions, _actionOptions);
    }
}
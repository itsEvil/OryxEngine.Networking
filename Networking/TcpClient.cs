using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using OryxEngine.Memory.Optionals;
using OryxEngine.Networking.Configuration;
using OryxEngine.Networking.PacketHandler;
using OryxEngine.Networking.Packets;
using OryxEngine.Networking.Sockets;
using OryxEngine.Networking.Statuses;
using OryxEngine.Networking.Validators;
using OryxEngine.Optionals;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking;
public class TcpClient 
{
    private readonly SocketBase _socketBase;
    private readonly SocketOptions _options;
    private readonly ActionOptions _actionOptions;
    private readonly CancellationTokenSource _cts;

    private readonly Reader _reader;
    private readonly Writer _writer;

    private readonly IValidator _validator;
    private readonly IPacketHandler _packetHandler;
    
    private Task? _receiveTask;
    
    private readonly ConcurrentQueue<IPacket> _receivedQueue = new();
    private readonly ConcurrentQueue<IPacket> _sendQueue = new();

    public TcpClient(SocketOptions options, ActionOptions actionOptions)
    {
        _options = options;
        _actionOptions = actionOptions;
        _socketBase = new SocketBase(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _cts = new CancellationTokenSource();
        _socketBase.Blocking = _options.Blocking;
        _socketBase.NoDelay = _options.NoDelay;
        _reader = new Reader(_options.EndianMode, new byte[_options.BufferSize], OnPacketFailure);
        _writer = new Writer(_options.EndianMode, new byte[_options.BufferSize], OnPacketFailure);
        _socketBase.SendBufferSize = _writer.Buffer.Length;
        _socketBase.ReceiveBufferSize = _reader.Buffer.Length;
        _validator = _options.Validator;
        _packetHandler = _options.PacketHandler;
    }

    public Option<SocketStatus> Tick()
    {
        if (_cts.IsCancellationRequested)
            return _socketBase.Disconnect(false);
        
        return _socketBase.IsConnected() ? SocketStatus.Connected : SocketStatus.Disconnected;
    }

    /// <summary>
    /// Calls <see cref="SocketBase"/>.<see cref="SocketBase.Connect(string, int)"/>. If this result is success it creates a new receive task
    /// </summary>
    /// <param name="host">IPAddress to connect to</param>
    /// <param name="port">Port to connect to</param>
    /// <returns><see cref="SocketStatus"/>.<see cref="SocketStatus.Connected"/> if successful otherwise <see cref="ErrorDetailed"/> containing exception data</returns>
    public Option<SocketStatus> Connect(string host, int port)
    {
        var ip = IPAddress.Parse(host);

        var result = _socketBase.Connect(ip, port);
        if (!result.IsSuccess)
            return result;
        
        _receiveTask = Task.Run(ReceiveThread, _cts.Token);
        return result;
    }
    /// <summary>
    /// Calls Cancel on the <see cref="CancellationTokenSource"/> to start the disconnect procedure. <br/>
    /// On Next <see cref="Tick"/>
    /// </summary>
    /// <param name="message"></param>
    public void Disconnect(string message = "unknown") {
        _cts.Cancel();
        if (!_socketBase.Connected)
            return;
        
        //Don't call disconnect action if we arnt connected...
        _actionOptions.OnDisconnect?.Invoke(this, message);
    }
    
    public void Send(IPacket packet) => _sendQueue.Enqueue(packet);
    
    public void Send(List<IPacket> packets) {
        var s = CollectionsMarshal.AsSpan(packets);
        foreach(var p in s)
            _sendQueue.Enqueue(p);
    }
    public void Send(IPacket[] packets) {
        var s = packets.AsSpan();
        foreach(var p in s)
            _sendQueue.Enqueue(p);
    }

    public void HandlePackets() {
        while(_receivedQueue.TryDequeue(out var pkt))
            _packetHandler.HandlePacket(pkt);
    }
    /// <summary>
    /// Call this to send any packets in the queue 
    /// </summary>
    public Option<SocketStatus> Flush() {
        if(!_socketBase.Connected)
            _cts.Cancel();
        
        if (_cts.IsCancellationRequested) {
            return SocketStatus.CancellationRequested;
        }
        
        //Don't need to write anything if queue is empty
        if (_sendQueue.IsEmpty) 
            return SocketStatus.Connected;
        
        _writer.Buffer.AsSpan().Clear();
        _writer.Reset();
        while (_sendQueue.TryDequeue(out var pkt)) {
            //store length position 
            var lengthPosition = _writer.Position;
            //skip to the packet id position aka jump to the length position + length size
            _writer.SetPosition(_writer.Position + sizeof(ushort));
            //write the packet data
            pkt.Write(_writer);
            //save the end position
            var newPosition = _writer.Position;
            //jump back to the length position
            _writer.SetPosition(lengthPosition);
            //find the difference between start of packet position and end packet position aka length
            var length = (ushort)(newPosition - lengthPosition - sizeof(ushort));
            //write length
            _writer.Write(length);
            //jump back to end of packet
            _writer.SetPosition(newPosition);
        }
        
        return _socketBase.Send(_writer.Buffer, 0, _writer.Position, SocketFlags.None);
    }
    
    private Task ReceiveThread() {
        Thread.CurrentThread.IsBackground = true;
        var sleepTime = _options.ReceiveThreadSleepTime;
        while (!_cts.IsCancellationRequested) {
            //Sleep at the start so we can run continue at any point and still get the sleep 
            Thread.Sleep(sleepTime);
            if(!_socketBase.Connected)
                return Task.CompletedTask;

            _reader.Buffer.AsSpan().Clear();
            
            int totalLength;
            try
            {
                totalLength = _socketBase.Receive(_reader.Buffer);
            }
            catch (Exception e)
            {
                Disconnect($"Caught exception when receiving data: {e.Message}\n{e.StackTrace}");
                return Task.CompletedTask;
            }

            if (totalLength == -1)
            {
                Disconnect("Length is not initialized");
                return Task.CompletedTask;
            }

            _reader.Reset(totalLength);
            
            //While there is still data in the buffer
            while (_reader.Position < totalLength)
            {
                if (!_validator.IsValid(_reader))
                {
                    Disconnect("Header is not valid");
                    return Task.CompletedTask;
                }

                //Don't need the packet length?
                var length = _validator.GetLength();
                var id = _validator.GetId();
                
                var packet = _packetHandler.GetAndReadPacket(id, _reader);
                if (packet is null) {
                    Disconnect("Packet with id is not valid");
                    return Task.CompletedTask;
                }
                
                _receivedQueue.Enqueue(packet);
            }
        }
        
        return Task.CompletedTask;
    }
    private void OnPacketFailure()
    {
        Disconnect("Packet Read / Write Failure");
    }
}
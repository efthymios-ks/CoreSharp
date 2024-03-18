using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.WebSockets.Interfaces;

public interface IWebSocketClient : IDisposable
{
    public const int DefaultBatchSize = 1024;

    bool IsConnected { get; }

    Task ConnectAsync(Uri uri, CancellationToken cancellationToken = default);
    Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription = null, CancellationToken cancellationToken = default);
    Task SendAsync(byte[] buffer, int batchSize = DefaultBatchSize, CancellationToken cancellationToken = default);

    event Action<byte[]> MessageReceived;
    event Action<byte[]> MessageSent;
    event Action Connected;
    event Action<WebSocketCloseStatus> Disconnected;
    event Action<Exception> ListenerErrorOccured;

    void StartListening(int batchSize = DefaultBatchSize, CancellationToken cancellationToken = default);
}

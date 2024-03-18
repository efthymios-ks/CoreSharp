using CoreSharp.WebSockets.Interfaces;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.WebSockets;

public sealed class WebSocketClient : IWebSocketClient
{
    private ClientWebSocket _clientWebSocket;
    private bool _isDisposed;
    private bool _isListening;

    public WebSocketClient()
        => _clientWebSocket = new ClientWebSocket();

    public event Action<byte[]> MessageReceived;
    public event Action<byte[]> MessageSent;
    public event Action Connected;
    public event Action<WebSocketCloseStatus> Disconnected;
    public event Action<Exception> ListenerErrorOccured;

    public bool IsConnected
        => _clientWebSocket?.State == WebSocketState.Open;

    public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        await _clientWebSocket.ConnectAsync(uri, cancellationToken);
        Connected?.Invoke();
    }

    public Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription = null, CancellationToken cancellationToken = default)
        => _clientWebSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);

    public async Task SendAsync(byte[] buffer, int batchSize = 1024, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(buffer);
        ValidateBufferSize(batchSize);
        if (buffer.Length == 0)
        {
            return;
        }

        var offset = 0;
        while (offset < buffer.Length)
        {
            var count = Math.Min(batchSize, buffer.Length - offset);
            var chunk = new ArraySegment<byte>(buffer, offset, count);
            var isEndOfMessage = offset + count == buffer.Length;

            await _clientWebSocket.SendAsync(chunk, WebSocketMessageType.Text, isEndOfMessage, cancellationToken);

            offset += count;
        }

        MessageSent?.Invoke(buffer);
    }

    public void StartListening(int batchSize = 1024, CancellationToken cancellationToken = default)
    {
        ValidateBufferSize(batchSize);

        if (_isListening)
        {
            return;
        }

        _isListening = true;
#pragma warning disable CA2016 // Forward the 'CancellationToken' parameter to methods
        _ = Task.Run(() => ListenInternalAsync(batchSize, cancellationToken));
#pragma warning restore CA2016 // Forward the 'CancellationToken' parameter to methods
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _clientWebSocket.Dispose();
        _clientWebSocket = null;
        _isDisposed = true;
    }

    private static void ValidateBufferSize(int batchSize, [CallerArgumentExpression(nameof(batchSize))] string paramName = null)
    {
        if (batchSize < 1)
        {
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} has to be >= 1.");
        }
    }

    private async Task ListenInternalAsync(int batchSize, CancellationToken cancellationToken)
    {
        await using var memoryStream = new MemoryStream();
        var buffer = new byte[batchSize];

        try
        {
            while (_clientWebSocket.State == WebSocketState.Open && _isListening)
            {
                var arraySegment = new ArraySegment<byte>(buffer);
                var result = await _clientWebSocket.ReceiveAsync(arraySegment, cancellationToken);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    var closeStatus = result.CloseStatus ?? WebSocketCloseStatus.Empty;
                    await CloseAsync(closeStatus, result.CloseStatusDescription, cancellationToken);
                    Disconnected?.Invoke(closeStatus);
                    break;
                }

                if (result.Count > 0)
                {
                    await memoryStream.WriteAsync(arraySegment.AsMemory(0, result.Count), cancellationToken);
                }

                if (result.EndOfMessage)
                {
                    var completBuffer = memoryStream.ToArray();
                    await memoryStream.FlushAsync(cancellationToken);
                    memoryStream.Position = 0;

                    if (completBuffer.Length > 0)
                    {
                        MessageReceived?.Invoke(memoryStream.ToArray());
                    }
                }
            }
        }
        catch (Exception exception)
        {
            ListenerErrorOccured?.Invoke(exception);
        }
    }
}
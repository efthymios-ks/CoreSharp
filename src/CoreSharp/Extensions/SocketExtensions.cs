using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="Socket"/> extensions.
/// </summary>
public static class SocketExtensions
{
    /// <summary>
    /// Check if <see cref="Socket"/> is connected using simple flag polling and pinging.
    /// </summary>
    public static async Task<bool> IsConnectedAsync(this Socket socket, int timeoutMillis = 5000)
    {
        _ = socket ?? throw new ArgumentNullException(nameof(socket));
        return timeoutMillis <= 0
            ? throw new ArgumentOutOfRangeException(nameof(timeoutMillis), $"{nameof(timeoutMillis)} has to be positive and non-zero.")
            : await socket.IsConnectedInternalAsync(timeoutMillis);
    }

    private static async Task<bool> IsConnectedInternalAsync(this Socket socket, int timeoutMillis = 5000)
    {
        if (!socket.Connected)
        {
            return false;
        }
        else
        {
            // Poll 
            if (socket.Poll(10, SelectMode.SelectRead) && socket.Available == 0)
                return false;

            // Ping 
            return socket.RemoteEndPoint is not IPEndPoint endPoint || await endPoint.PingAsync(timeoutMillis);
        }
    }
}

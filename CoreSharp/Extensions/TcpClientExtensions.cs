using System.Net.Sockets;
using System.Threading.Tasks;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="TcpClient"/> extensions.
    /// </summary>
    public static class TcpClientExtensions
    {
        /// <inheritdoc cref="IsConnectedAsync(TcpClient, int)"/>
        public static bool IsConnected(this TcpClient client, int timeoutMillis = 5000)
            => client.IsConnectedAsync(timeoutMillis).GetAwaiter().GetResult();

        /// <inheritdoc cref="SocketExtensions.IsConnected(Socket, int)"/>
        public static async Task<bool> IsConnectedAsync(this TcpClient client, int timeoutMillis = 5000)
            => await (client?.Client).IsConnectedAsync(timeoutMillis);
    }
}

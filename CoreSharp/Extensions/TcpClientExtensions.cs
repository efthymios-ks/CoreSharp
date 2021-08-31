using System;
using System.Net.Sockets;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// TcpClient extensions. 
    /// </summary>
    public static partial class TcpClientExtensions
    {
        /// <inheritdoc cref="SocketExtensions.IsConnected(Socket, int)"/>
        public static bool IsConnected(this TcpClient client, int timeoutMillis = 5000)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.Client.IsConnected(timeoutMillis);
        }
    }
}

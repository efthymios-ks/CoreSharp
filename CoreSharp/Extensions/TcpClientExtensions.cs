using System;
using System.Net.Sockets;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// TcpClient extensions. 
    /// </summary>
    public static partial class TcpClientExtensions
    {
        /// <summary>
        /// Check if TcpClient is Connected. 
        /// Performs Flag checking, Polling and Pinging. 
        /// </summary>
        public static bool IsConnected(this TcpClient client, int timeoutMillis = 5000)
        {
            client = client ?? throw new ArgumentNullException(nameof(client));

            return client.Client.IsConnected(timeoutMillis);
        }
    }
}

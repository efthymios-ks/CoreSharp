using System;
using System.Net;
using System.Net.Sockets;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Socket extensions.
    /// </summary>
    public static class SocketExtensions
    {
        /// <summary>
        /// Check if socket is connected using simple flag polling and pinging.
        /// </summary>
        public static bool IsConnected(this Socket socket, int timeoutMillis = 5000)
        {
            _ = socket ?? throw new ArgumentNullException(nameof(socket));
            if (timeoutMillis <= 0)
                throw new ArgumentOutOfRangeException(nameof(timeoutMillis), $"{nameof(timeoutMillis)} has to be positive and non-zero.");

            if (!socket.Connected)
            {
                return false;
            }
            else
            {
                //Poll 
                if (socket.Poll(10, SelectMode.SelectRead) && socket.Available == 0)
                    return false;

                //Ping 
                if (socket.RemoteEndPoint is IPEndPoint endPoint && !endPoint.Ping(timeoutMillis))
                    return false;

                return true;
            }
        }
    }
}

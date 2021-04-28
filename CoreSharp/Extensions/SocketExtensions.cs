using System;
using System.Net;
using System.Net.Sockets;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Socket extensions. 
    /// </summary>
    public static partial class SocketExtensions
    {
        /// <summary>
        /// Check if socket is Connected using simple flag polling and pinging. 
        /// </summary>
        public static bool IsConnected(this Socket socket, int timeoutMillis = 5000)
        {
            socket = socket ?? throw new ArgumentNullException(nameof(socket));
            if (timeoutMillis <= 0)
                throw new ArgumentOutOfRangeException(nameof(timeoutMillis));

            if (!socket.Connected)
                return false;
            else
            {
                //Poll 
                if (socket.Poll(10, SelectMode.SelectRead))
                    if (socket.Available == 0)
                        return false;

                //Ping 
                if (socket.RemoteEndPoint is IPEndPoint endPoint)
                    if (!endPoint.Ping(timeoutMillis))
                        return false;

                return true;
            }
        }
    }
}

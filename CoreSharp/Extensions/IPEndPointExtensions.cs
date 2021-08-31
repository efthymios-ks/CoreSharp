using System;
using System.Net;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// IPEndPoint extensions. 
    /// </summary>
    public static partial class IPEndPointExtensions
    {
        /// <inheritdoc cref="IPAddressExtensions.Ping(IPAddress, int)"/> 
        public static bool Ping(this IPEndPoint endPoint, int timeoutMillis = 5000)
        {
            _ = endPoint ?? throw new ArgumentNullException(nameof(endPoint));

            return endPoint.Address.Ping(timeoutMillis);
        }
    }
}

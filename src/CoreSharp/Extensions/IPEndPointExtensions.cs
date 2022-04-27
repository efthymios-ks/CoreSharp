using System.Net;
using System.Threading.Tasks;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="IPEndPoint"/> extensions.
    /// </summary>
    public static class IPEndPointExtensions
    {
        /// <inheritdoc cref="IPAddressExtensions.PingAsync(IPAddress, int)"/>
        public static async Task<bool> PingAsync(this IPEndPoint endPoint, int timeoutMillis = 5000)
            => await (endPoint?.Address).PingAsync(timeoutMillis);
    }
}

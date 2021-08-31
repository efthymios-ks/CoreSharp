using System.Collections.Generic;
using System.Net;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// IPAddress utilities. 
    /// </summary>
    public static class IPAddress
    {
        /// <summary>
        /// Get current machines ip collection.
        /// </summary>
        public static IEnumerable<System.Net.IPAddress> GetCurrentIpCollection()
        {
            var hostName = Dns.GetHostName();
            var iphe = Dns.GetHostEntry(hostName);
            return iphe.AddressList;
        }
    }
}

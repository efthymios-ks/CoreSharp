using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// IPAddress extensions. 
    /// </summary>
    public static partial class IPAddressExtensions
    {
        /// <summary>
        /// Ping host. 
        /// </summary>
        public static bool Ping(this IPAddress address, int timeoutMillis = 5000)
        {
            _ = address ?? throw new ArgumentNullException(nameof(address));
            if (timeoutMillis <= 0)
                throw new ArgumentOutOfRangeException(nameof(timeoutMillis));

            try
            {
                using var ping = new Ping();
                var reply = ping.Send($"{address}", timeoutMillis);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get current machines ip collection.
        /// </summary>
        public static IEnumerable<IPAddress> GetCurrentIpCollection()
        {
            string hostName = Dns.GetHostName();
            var iphe = Dns.GetHostEntry(hostName);
            return iphe.AddressList;
        }
    }
}

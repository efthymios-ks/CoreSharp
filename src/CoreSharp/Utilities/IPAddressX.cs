using System.Collections.Generic;
using System.Net;

namespace CoreSharp.Utilities;

/// <summary>
/// <see cref="IPAddress"/> utilities.
/// </summary>
public static class IPAddressX
{
    /// <summary>
    /// Get current machines ip collection.
    /// </summary>
    public static IEnumerable<IPAddress> GetCurrentIpCollection()
    {
        var hostName = Dns.GetHostName();
        var iphe = Dns.GetHostEntry(hostName);
        return iphe.AddressList;
    }
}

using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="IPAddress"/> extensions.
/// </summary>
public static class IPAddressExtensions
{
    /// <inheritdoc cref="Ping.SendPingAsync(string, int)"/>
    public static async Task<bool> PingAsync(this IPAddress address, int timeoutMillis = 5000)
    {
        ArgumentNullException.ThrowIfNull(address);
        return timeoutMillis <= 0
            ? throw new ArgumentOutOfRangeException(nameof(timeoutMillis))
            : await address.PingInternalAsync(timeoutMillis);
    }

    private static async Task<bool> PingInternalAsync(this IPAddress address, int timeoutMillis = 5000)
    {
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync($"{address}", timeoutMillis);
            return reply is { Status: IPStatus.Success };
        }
        catch
        {
            return false;
        }
    }
}

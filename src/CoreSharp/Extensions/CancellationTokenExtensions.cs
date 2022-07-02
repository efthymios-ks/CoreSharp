using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="CancellationToken"/> extensions.
/// </summary>
public static class CancellationTokenExtensions
{
    /// <summary>
    /// <para>
    /// Add timeout to provided <see cref="CancellationToken"/>.<br />
    /// If given <see cref="TimeSpan"/> is negative or
    /// <see cref="Timeout.Infinite"/> then it is ignored.<br />
    /// Timing starts immediately after this call,
    /// so it should be called right next to the
    /// actual async call.
    /// </para>
    /// <code>
    /// using var cancellationTokenSource = cancellationToken.ToTimeoutCancellationTokenSource(timeout);
    /// try
    /// {
    ///     await httpClient.SendAsync(httpRequestMessage, cancellationTokenSource.Token);
    /// }
    /// //Cancelled by user
    /// catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
    /// {
    /// }
    /// //Timeout
    /// catch (TaskCanceledException)
    /// {
    /// }
    /// </code>
    /// </summary>
    /// <exception cref="TaskSchedulerException">
    /// When the timeout elapses.
    /// </exception>
    public static CancellationTokenSource ToTimeoutCancellationTokenSource(this CancellationToken cancellationToken, TimeSpan timeout)
    {
        var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        if (timeout.TotalMilliseconds > 0 && timeout != Timeout.InfiniteTimeSpan)
            cancellationTokenSource.CancelAfter(timeout);
        return cancellationTokenSource;
    }
}

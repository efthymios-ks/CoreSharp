using CoreSharp.Delegates;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="AsyncDelegate"/> extensions.
/// </summary>
public static class AsyncDelegateExtensions
{
    /// <summary>
    /// Raise async event for all subscribers.
    /// </summary>
    public static async Task InvokeAsync(this AsyncDelegate handler, CancellationToken cancellationToken = default)
    {
        var delegates = handler?.GetInvocationList();
        if (delegates?.Length is not > 0)
            return;

        var tasks = delegates.Cast<AsyncDelegate>()
                             .Select(e => e.Invoke(cancellationToken));
        await Task.WhenAll(tasks);
    }
}

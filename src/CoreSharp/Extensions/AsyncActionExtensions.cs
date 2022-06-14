using CoreSharp.Delegates;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="AsyncAction{TEventArgs}"/> extensions.
    /// </summary>
    public static class AsyncActionExtensions
    {
        /// <inheritdoc cref="AsyncDelegateExtensions.InvokeAsync(AsyncDelegate, CancellationToken)"/>
        public static async Task InvokeAsync<TEventArgs>(this AsyncAction<TEventArgs> handler, TEventArgs args, CancellationToken cancellationToken = default)
        {
            var delegates = handler?.GetInvocationList();
            if (delegates?.Length is not > 0)
                return;

            var tasks = delegates.Cast<AsyncAction<TEventArgs>>()
                                 .Select(e => e.Invoke(args, cancellationToken));
            await Task.WhenAll(tasks);
        }
    }
}

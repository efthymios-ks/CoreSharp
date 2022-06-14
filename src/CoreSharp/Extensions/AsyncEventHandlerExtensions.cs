using CoreSharp.Delegates;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="AsyncEventHandler{TEventArgs}"/> extensions.
    /// </summary>
    public static class AsyncEventHandlerExtensions
    {
        /// <inheritdoc cref="AsyncDelegateExtensions.InvokeAsync(AsyncDelegate, CancellationToken)"/>
        public static async Task InvokeAsync<TEventArgs>(this AsyncEventHandler<TEventArgs> handler, object sender, TEventArgs args, CancellationToken cancellationToken = default)
        {
            var delegates = handler?.GetInvocationList();
            if (delegates?.Length is not > 0)
                return;

            var tasks = delegates.Cast<AsyncEventHandler<TEventArgs>>()
                                 .Select(e => e.Invoke(sender, args, cancellationToken));
            await Task.WhenAll(tasks);
        }
    }
}

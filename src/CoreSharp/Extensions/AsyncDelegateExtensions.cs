using CoreSharp.Delegates;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="AsyncDelegate"/> extensions.
    /// </summary>
    public static class AsyncDelegateExtensions
    {
        /// <inheritdoc cref="AsyncEventHandlerExtensions.InvokeAsync{TEventArgs}(AsyncEventHandler{TEventArgs}, object, TEventArgs, CancellationToken)"/>
        public static async Task InvokeAsync(this AsyncDelegate handler, CancellationToken cancellationToken = default)
        {
            var delegates = handler?.GetInvocationList();
            if (delegates?.Length > 0)
            {
                var tasks = delegates.Cast<AsyncDelegate>()
                                     .Select(e => e.Invoke(cancellationToken));
                await Task.WhenAll(tasks);
            }
        }
    }
}

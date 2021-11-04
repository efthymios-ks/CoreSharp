using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// <see cref="ValueTask"/> utilities.
    /// </summary>
    public static class ValueTaskX
    {
        /// <inheritdoc cref="WithAll(ValueTask[])"/>
        public static async Task WithAll(IEnumerable<ValueTask> valueTasks)
            => await WithAll(valueTasks?.ToArray());

        /// <inheritdoc cref="Task.WhenAll(Task[])"/>
        public static async Task WithAll(params ValueTask[] valueTasks)
        {
            _ = valueTasks ?? throw new ArgumentNullException(nameof(valueTasks));

            var tasks = valueTasks.Select(vt => vt.AsTask());
            await Task.WhenAll(tasks);
        }
    }
}

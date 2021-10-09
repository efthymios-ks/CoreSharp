using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="Task"/> extensions.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Chain calls <see cref="Task{TResult}.GetAwaiter"/> and <see cref="TaskAwaiter.GetResult"/>
        /// </summary>
        public static TResult AwaitResult<TResult>(this Task<TResult> task)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));
            return task.GetAwaiter().GetResult();
        }
    }
}

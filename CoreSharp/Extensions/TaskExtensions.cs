using System;
using System.Runtime.CompilerServices;
using System.Threading;
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

        /// <summary>
        /// Aggregates all inner <see cref="Exception" />(s) into a single <see cref="AggregateException"/>.
        /// Best used in conjuction with <see cref="Task.WaitAll(Task[])"/>.
        /// </summary>
        public static async Task WithAggregateException(this Task task)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));

            static Task ContinuationFunction(Task t)
            {
                //static bool HasInnerExceptions(AggregateException exception)
                //    => exception?.InnerException is AggregateException || exception.InnerExceptions.Count > 1;

                if (!t.IsFaulted)
                    return t;
                else if (t.Exception is not AggregateException aggregateException)
                    return t;
                //else if (!HasInnerExceptions(aggregateException))
                //    return t;
                else
                    return Task.FromException(aggregateException.Flatten());
            }

            await task.ContinueWith(
                ContinuationFunction,
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default).Unwrap();
        }
    }
}

﻿using System;
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

        /// <inheritdoc cref="TimeoutAfter{TResult}(Task{TResult}, TimeSpan)" />
        public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));

            var genericTask = Task.Run(async () =>
            {
                await task;
                return true;
            });
            await genericTask.TimeoutAfter(timeout);
        }

        /// <summary>
        /// Run a <see cref="Task"/> within a given time frame.
        /// </summary>
        /// <exception cref="TimeoutException">When then given task is not completed within the specified time frame.</exception>
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));

            var timeoutTask = Task.Delay(timeout);
            var completedTask = await Task.WhenAny(task, timeoutTask);
            if (completedTask == task)
                return await task;
            else
                throw new TimeoutException();
        }

        /// <summary>
        /// Return <see cref="Task.CompletedTask"/>
        /// if provided <see cref="Task"/> is <see langword="null" />.
        /// <code>
        /// // Make sure you wrap the actual task in parenthesis.
        /// await (stream1?.CopyToAsync(stream2)).OrDefault();
        /// </code>
        /// </summary>
        public static async Task OrDefault(this Task task)
            => await (task ?? Task.CompletedTask);

        /// <inheritdoc cref="Or{TResult}(Task{TResult}, TResult)"/>
        public static async Task<TResult> OrDefault<TResult>(this Task<TResult> task)
            => await task.Or(default);

        /// <summary>
        /// Return <see cref="Task.FromResult{TResult}"/>
        /// if provided <see cref="Task"/> is <see langword="null" />.
        /// <code>
        /// // Make sure you wrap the actual task in parenthesis.
        /// var bytes = new byte[2048];
        /// var byteCount = await (stream?.ReadAsync(bytes)).OrDefault();
        /// </code>
        /// </summary>
        public static async Task<TResult> Or<TResult>(this Task<TResult> task, TResult defaultValue)
            => await (task ?? Task.FromResult(defaultValue));

        /// <summary>
        /// 'Try-catch'ing providing <see cref="Task{TResult}"/>.
        /// Will return result if succeeds, else the exception.
        /// Usefull for one-liners.
        /// <code>
        /// // Get value and error
        /// var (value1, exception) = await GetValueAsync().TryGetAsync();
        /// // Get value and skip error
        /// var (value2, _) = await GetValueAsync().TryGetAsync();
        /// </code>
        /// </summary>
        public async static Task<(TResult result, Exception exception)> TryGetAsync<TResult>(this Task<TResult> task)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));

            try
            {
                var result = await task;
                return (result, null);
            }
            catch (Exception exception)
            {
                return (default, exception);
            }
        }

        /// <summary>
        /// 'Try-catch'ing providing <see cref="Task"/>.
        /// Usefull for one-liners.
        /// <code>
        /// // Run and get error
        /// var exception = await PostAsync().TryRunAsync();
        /// // Run and skip error
        /// await PostAsync().TryRunAsync();
        /// </code>
        /// </summary>
        public async static Task<Exception> TryRunAsync(this Task task)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));

            try
            {
                await task;
                return null;
            }
            catch (Exception exception)
            {
                return exception;
            }
        }
    }
}

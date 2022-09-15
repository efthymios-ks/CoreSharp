using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Extensions;

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
            // static bool HasInnerExceptions(AggregateException exception)
            //    => exception?.InnerException is AggregateException || exception.InnerExceptions.Count > 1;

            if (!t.IsFaulted)
                return t;

            if (t.Exception is not AggregateException aggregateException)
                return t;

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
        return completedTask == task ? await task : throw new TimeoutException();
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

    /// <inheritdoc cref="IgnoreError{TException}(Task)"/>
    public static async Task IgnoreError(this Task task)
        => await task.IgnoreError<Exception>();

    /// <summary>
    /// 'Try-catch'ing given <see cref="Task"/>.
    /// Usefull for one-liners.
    /// Can be chained.
    /// <code>
    /// // Catch all exceptions.
    /// await PostAsync().IgnoreError();
    ///
    /// // Catch specific exceptions.
    /// await PostAsync().IgnoreError&lt;DivideByZeroException&gt;()
    ///                  .IgnoreError&lt;InvalidCastException&gt;();
    /// </code>
    /// </summary>
    public static async Task IgnoreError<TException>(this Task task)
        where TException : Exception
    {
        _ = task ?? throw new ArgumentNullException(nameof(task));

        try
        {
            await task;
        }
        catch (Exception exception) when (exception is TException)
        {
            // This is the point of the method. Ignore error.
        }
    }

    /// <inheritdoc cref="IgnoreError{TResult, TException}(Task{TResult})"/>
    public static async Task<TResult> IgnoreError<TResult>(this Task<TResult> task)
        => await task.IgnoreError<TResult, Exception>();

    /// <summary>
    /// 'Try-catch'ing given <see cref="Task{TResult}"/>.
    /// Usefull for one-liners.
    /// Can be chained.
    /// <code>
    /// // Catch all exceptions.
    /// var result = await GetAsync().IgnoreError();
    ///
    /// // Catch specific exceptions.
    /// var result = await GetAsync().IgnoreError&lt;DivideByZeroException&gt;()
    ///                              .IgnoreError&lt;InvalidCastException&gt;();
    /// </code>
    /// </summary>
    public static async Task<TResult> IgnoreError<TResult, TException>(this Task<TResult> task)
        where TException : Exception
    {
        _ = task ?? throw new ArgumentNullException(nameof(task));

        try
        {
            return await task;
        }
        catch (Exception exception) when (exception is TException)
        {
            return default;
        }
    }
}

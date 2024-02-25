using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Utilities;

/// <summary>
/// <see cref="Task"/> utilities.
/// </summary>
public static class TaskUtils
{
    /// <inheritdoc cref="WhenAll(int, Task[])"/>
    public static async Task WhenAll(int batchSize, IEnumerable<Task> tasks)
        => await WhenAll(batchSize, tasks?.ToArray());

    /// <inheritdoc cref="Task.WhenAll(Task[])"/>
    public static async Task WhenAll<TResult>(int batchSize, params Task[] tasks)
      => await WhenAll(batchSize, tasks?.Select(task => task.MakeGeneric<bool>())
                                        .ToArray());

    /// <inheritdoc cref="WhenAll{TResult}(int, Task{TResult}[])"/>
    public static async Task WhenAll<TResult>(int batchSize, IEnumerable<Task<TResult>> tasks)
        => await WhenAll(batchSize, tasks?.ToArray());

    /// <inheritdoc cref="Task.WhenAll(Task[])"/>
    public static async Task<IEnumerable<TResult>> WhenAll<TResult>(int batchSize, params Task<TResult>[] tasks)
    {
        ArgumentNullException.ThrowIfNull(tasks);

        if (batchSize < 1)
        {
            return await Task.WhenAll(tasks);
        }

        using var semaphore = new SemaphoreSlim(batchSize);
        var pendingTasks = tasks.Select(async task =>
        {
            try
            {
                semaphore.Wait();
                return await task;
            }
            finally
            {
                semaphore.Release();
            }
        });

        return await Task.WhenAll(pendingTasks);
    }
}

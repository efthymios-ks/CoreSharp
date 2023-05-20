using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSharp.Utilities;

/// <summary>
/// <see cref="ValueTask"/> utilities.
/// </summary>
public static class ValueTaskX
{
    /// <inheritdoc cref="WhenAll(ValueTask[])"/>
    public static async Task WhenAll(IEnumerable<ValueTask> valueTasks)
        => await WhenAll(valueTasks?.ToArray());

    /// <inheritdoc cref="Task.WhenAll(Task[])"/>
    public static async Task WhenAll(params ValueTask[] valueTasks)
    {
        ArgumentNullException.ThrowIfNull(valueTasks);

        var tasks = valueTasks.Select(vt => vt.AsTask());
        await Task.WhenAll(tasks);
    }
}

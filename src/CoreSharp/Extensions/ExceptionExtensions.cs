using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="Exception"/> extensions.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Get all <see cref="Exception.Message"/>,
    /// including nested <see cref="Exception.InnerException"/>.
    /// </summary>
    public static string UnwrapMessages(this Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var messages = exception.Unwrap()
                                .Where(ex => !string.IsNullOrWhiteSpace(ex.Message))
                                .Select(ex => ex.Message);

        return string.Join(Environment.NewLine, messages);
    }

    /// <summary>
    /// Return unfolded list of <see cref="Exception"/> including nested ones.
    /// </summary>
    public static IEnumerable<Exception> Unwrap(this Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return exception.UnwrapInternal();
    }

    private static IEnumerable<Exception> UnwrapInternal(this Exception exception)
    {
        yield return exception;

        if (exception is AggregateException aggregateException)
        {
            foreach (var innerException in aggregateException.InnerExceptions.SelectMany(ex => ex.Unwrap()))
            {
                yield return innerException;
            }
        }
        else if (exception.InnerException is not null)
        {
            foreach (var innerException in exception.InnerException.Unwrap())
            {
                yield return innerException;
            }
        }
    }

    /// <summary>
    /// Get innermost <see cref="Exception"/>.
    /// Uses <see cref="Exception.InnerException"/>.
    /// </summary>
    public static Exception GetInnermostException(this Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        if (exception.InnerException is null)
        {
            return exception;
        }
        else
        {
            return GetInnermostException(exception.InnerException);
        }
    }
}

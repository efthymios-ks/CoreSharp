using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="Exception"/> extensions.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Get all messages, including nested exceptions.
        /// </summary>
        public static string FlattenMessages(this Exception exception)
        {
            _ = exception ?? throw new ArgumentNullException(nameof(exception));

            var exceptions = exception.Flatten();
            var messages = exceptions
                                .Where(e => !string.IsNullOrWhiteSpace(e.Message))
                                .Select(e => e.Message);

            return string.Join(Environment.NewLine, messages);
        }

        /// <summary>
        /// Return unfolded list of exceptions including nested ones.
        /// </summary>
        public static IEnumerable<Exception> Flatten(this Exception exception)
        {
            _ = exception ?? throw new ArgumentNullException(nameof(exception));

            return exception.FlattenInternal();
        }

        private static IEnumerable<Exception> FlattenInternal(this Exception exception)
        {
            yield return exception;

            if (exception is AggregateException aggregateEx)
            {
                foreach (var innerEx in aggregateEx.InnerExceptions.SelectMany(e => e.Flatten()))
                    yield return innerEx;
            }
            else if (exception.InnerException is not null)
            {
                foreach (var innerEx in exception.InnerException.Flatten())
                    yield return innerEx;
            }
        }
    }
}

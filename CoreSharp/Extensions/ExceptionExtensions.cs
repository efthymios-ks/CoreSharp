using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Exception extensions.
    /// </summary>
    public static partial class ExceptionExtensions
    {
        /// <summary>
        /// Get all messages including nested exceptions. 
        /// </summary> 
        public static string FlattenMessages(this Exception exception)
        {
            exception = exception ?? throw new ArgumentNullException(nameof(exception));

            var exceptions = exception.GetExceptions();
            var messages = exceptions
                                .Where(e => !string.IsNullOrWhiteSpace(e.Message))
                                .Select(e => e.Message);

            return string.Join(Environment.NewLine, messages);
        }

        /// <summary>
        /// Return list of exceptions including nested ones. 
        /// </summary> 
        public static IEnumerable<Exception> GetExceptions(this Exception exception)
        {
            exception = exception ?? throw new ArgumentNullException(nameof(exception));

            return exception.GetExceptionsInternal();
        }

        private static IEnumerable<Exception> GetExceptionsInternal(this Exception exception)
        {
            yield return exception;

            if (exception is AggregateException aggregateEx)
            {
                foreach (Exception innerEx in aggregateEx.InnerExceptions.SelectMany(e => e.GetExceptions()))
                    yield return innerEx;
            }
            else if (exception.InnerException != null)
            {
                foreach (Exception innerEx in exception.InnerException.GetExceptions())
                    yield return innerEx;
            }
        }
    }
}

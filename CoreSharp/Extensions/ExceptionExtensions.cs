﻿using System;
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
        /// Get all <see cref="Exception.Message"/>,
        /// including nested <see cref="Exception"/>.
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
        /// Return unfolded list of <see cref="Exception"/> including nested ones.
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

        /// <summary>
        /// Get innermost <see cref="Exception"/>.
        /// Uses <see cref="Exception.InnerException"/>.
        /// </summary>
        public static Exception GetInnermostException(this Exception exception)
        {
            _ = exception ?? throw new ArgumentNullException(nameof(exception));

            if (exception.InnerException is null)
                return exception;
            else
                return GetInnermostException(exception.InnerException);
        }
    }
}

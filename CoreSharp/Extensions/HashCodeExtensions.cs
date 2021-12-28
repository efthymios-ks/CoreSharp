using System;
using System.Collections.Generic;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="HashCode"/> extensions.
    /// </summary>
    public static class HashCodeExtensions
    {
        /// <inheritdoc cref="AddRange{TEntity}(HashCode, IEnumerable{TEntity}, IEqualityComparer{TEntity})"/>
        public static void AddRange<TEntity>(this HashCode hashCode, IEnumerable<TEntity> source)
            => hashCode.AddRange(source, EqualityComparer<TEntity>.Default);

        /// <summary>
        /// Add multiple values to <see cref="HashCode"/>.
        /// </summary>
        public static void AddRange<TEntity>(this HashCode hashCode, IEnumerable<TEntity> source, IEqualityComparer<TEntity> equalityComparer)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = equalityComparer ?? throw new ArgumentNullException(nameof(equalityComparer));

            foreach (var entity in source)
                hashCode.Add(entity, equalityComparer);
        }
    }
}

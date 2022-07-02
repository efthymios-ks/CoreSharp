using System;
using System.Collections.Generic;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="HashCode"/> extensions.
/// </summary>
public static class HashCodeExtensions
{
    /// <inheritdoc cref="AddRange{TElement}(HashCode, IEnumerable{TElement}, IEqualityComparer{TElement})"/>
    public static void AddRange<TElement>(this HashCode hashCode, IEnumerable<TElement> source)
        => hashCode.AddRange(source, EqualityComparer<TElement>.Default);

    /// <summary>
    /// Add multiple values to <see cref="HashCode"/>.
    /// </summary>
    public static void AddRange<TElement>(this HashCode hashCode, IEnumerable<TElement> source, IEqualityComparer<TElement> equalityComparer)
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = equalityComparer ?? throw new ArgumentNullException(nameof(equalityComparer));

        foreach (var element in source)
            hashCode.Add(element, equalityComparer);
    }
}

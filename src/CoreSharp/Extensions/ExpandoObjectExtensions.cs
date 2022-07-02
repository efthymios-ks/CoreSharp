using System;
using System.Collections.Generic;
using System.Dynamic;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="ExpandoObject"/> extensions.
/// </summary>
public static class ExpandoObjectExtensions
{
    /// <summary>
    /// Convert to <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    public static IDictionary<string, object> ToDictionary(this ExpandoObject source)
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));

        return source;
    }

    /// <summary>
    /// Try get specific value by key.
    /// </summary>
    public static TElement GetValue<TElement>(this ExpandoObject source, string key)
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));

        var dictionary = source.ToDictionary();
        return (TElement)dictionary[key];
    }
}

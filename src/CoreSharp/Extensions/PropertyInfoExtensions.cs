using System;
using System.Reflection;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="PropertyInfo"/> extensions.
/// </summary>
public static class PropertyInfoExtensions
{
    /// <inheritdoc cref="PropertyInfo.GetValue(object?)" />
    public static TElement GetValue<TElement>(this PropertyInfo property, object element)
    {
        ArgumentNullException.ThrowIfNull(property);

        return (TElement)property.GetValue(element);
    }
}

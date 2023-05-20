using System;
using System.ComponentModel.DataAnnotations;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="Enum"/> extensions.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Get given <see cref="Attribute"/> from an <see cref="Enum"/>.
    /// </summary>
    public static TAttribute GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
    {
        ArgumentNullException.ThrowIfNull(value);

        var enumField = value.GetType().GetField($"{value}");
        return enumField.GetAttribute<TAttribute>();
    }

    /// <summary>
    /// Get <see cref="DisplayAttribute"/> attribute from an <see cref="Enum"/>.
    /// </summary>
    public static DisplayAttribute GetDisplayAttribute(this Enum value)
        => value.GetAttribute<DisplayAttribute>();

    /// <summary>
    /// Get <see cref="DisplayAttribute.Name"/> attribute from an <see cref="Enum"/>.
    /// </summary>
    public static string GetDisplayAttributeName(this Enum value)
        => value.GetDisplayAttribute()?.Name ?? $"{value}";

    /// <summary>
    /// Get <see cref="DisplayAttribute.ShortName"/> attribute from an <see cref="Enum"/>.
    /// </summary>
    public static string GetDisplayAttributeShortName(this Enum value)
        => value.GetDisplayAttribute()?.ShortName ?? $"{value}";

    /// <summary>
    /// Get <see cref="DisplayAttribute.Description"/> attribute from an <see cref="Enum"/>.
    /// </summary>
    public static string GetDisplayAttributeDescription(this Enum value)
        => value.GetDisplayAttribute()?.Description ?? $"{value}";
}

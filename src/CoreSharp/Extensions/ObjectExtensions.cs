using System;
using System.Globalization;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="object"/> extensions.
/// </summary>
public static class ObjectExtensions
{
    /// <inheritdoc cref="GetValueOr{TElement}(object, TElement)"/>
    public static TElement GetValue<TElement>(this object input)
        => input.GetValueOr(default(TElement));

    /// <summary>
    /// Try casting input to given type and
    /// return default if null or of different type.
    /// </summary>
    public static TElement GetValueOr<TElement>(this object input, TElement defaultValue)
        => input is TElement value ? value : defaultValue;

    /// <inheritdoc cref="ChangeType{TResult}(object, CultureInfo)"/>
    public static TResult ChangeType<TResult>(this object value)
        => value.ChangeType<TResult>(CultureInfo.CurrentCulture);

    /// <inheritdoc cref="Convert.ChangeType(object?, Type, IFormatProvider)" />
    public static TResult ChangeType<TResult>(this object value, CultureInfo cultureInfo)
    {
        if (value is null)
            return default;

        var baseType = typeof(TResult);
        baseType = Nullable.GetUnderlyingType(baseType) ?? baseType;
        return (TResult)Convert.ChangeType(value, baseType, cultureInfo);
    }
}

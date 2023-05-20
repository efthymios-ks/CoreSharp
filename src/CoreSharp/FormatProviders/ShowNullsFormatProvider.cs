using CoreSharp.CustomFormatters;
using System;

namespace CoreSharp.FormatProviders;

internal sealed class ShowNullsFormatProvider : IFormatProvider
{
    // Fields
    private readonly ShowNullsCustomFormatter _showNullsCustomFormatter = new();
    private static ShowNullsFormatProvider _showNullsFormatProvider;

    // Properties
    public static ShowNullsFormatProvider Default
        => _showNullsFormatProvider ??= new();

    // Methods
    public object GetFormat(Type formatType)
    {
        if (formatType == typeof(ICustomFormatter))
        {
            return _showNullsCustomFormatter;
        }

        return null;
    }
}

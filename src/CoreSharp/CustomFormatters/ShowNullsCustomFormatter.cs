using System;

namespace CoreSharp.CustomFormatters;

internal sealed class ShowNullsCustomFormatter : ICustomFormatter
{
    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
        if (arg is null)
            return "{null}";

        var argAsString = Convert.ToString(arg, formatProvider);
        if (string.IsNullOrWhiteSpace(argAsString))
            return "{empty}";

        return argAsString;
    }
}
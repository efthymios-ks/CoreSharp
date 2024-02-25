using CoreSharp.FormatProviders;
using System;

namespace CoreSharp.Utilities;

public static class FormattabeStringUtils
{
    /// <summary>
    /// Print {null} for nulls and {empty} for empty or whitespace strings.
    /// <code>
    /// using static CoreSharp.Utilities.FormattabeStringX;
    /// 
    /// // Will output "{null} - {empty} - {empty} - 1"
    /// var output = ShowNulls($"{null} - {""} - {" "} - {1}");
    /// </code>
    /// </summary>
    public static string ShowNulls(FormattableString formattableString)
    {
        ArgumentNullException.ThrowIfNull(formattableString);

        return formattableString.ToString(ShowNullsFormatProvider.Default);
    }
}

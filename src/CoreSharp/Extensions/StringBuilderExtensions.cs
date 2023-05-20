using System;
using System.Globalization;
using System.Text;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="StringBuilderExtensions"/> extensions.
/// </summary>
public static class StringBuilderExtensions
{
    /// <inheritdoc cref="AppendFormatLine(StringBuilder, IFormatProvider, string, object[])"/>
    public static StringBuilder AppendFormatLine(this StringBuilder builder, string format, params object[] arguments)
        => builder.AppendFormatLine(CultureInfo.CurrentCulture, format, arguments);

    /// <summary>
    /// Chain calls <see cref="StringBuilder.AppendFormat(IFormatProvider?, string, object?[])"/> + <see cref="StringBuilder.AppendLine(string?)"/>.
    /// </summary>
    public static StringBuilder AppendFormatLine(this StringBuilder builder, IFormatProvider formatProvider, string format, params object[] arguments)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder.AppendFormat(formatProvider, format, arguments)
                      .AppendLine();
    }

    /// <inheritdoc cref="AppendFormatLine(StringBuilder, IFormatProvider, string, object[])"/>
    public static StringBuilder AppendFormatLineCI(this StringBuilder builder, string format, params object[] arguments)
       => builder.AppendFormatLine(CultureInfo.InvariantCulture, format, arguments);
}

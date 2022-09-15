using System;
using System.Globalization;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="ulong"/> extensions.
/// </summary>
public static class UlongExtensions
{
    /// <inheritdoc cref="ToComputerSize(ulong, string, IFormatProvider)"/>
    public static string ToComputerSize(this ulong byteSize)
        => byteSize.ToComputerSize("G");

    /// <inheritdoc cref="ToComputerSize(ulong, string, IFormatProvider)"/>
    public static string ToComputerSize(this ulong byteSize, string format)
        => byteSize.ToComputerSize(format, CultureInfo.CurrentCulture);

    /// <inheritdoc cref="ToComputerSize(ulong, string, IFormatProvider)"/>
    public static string ToComputerSize(this ulong byteSize, IFormatProvider formatProvider)
        => byteSize.ToComputerSize("G", formatProvider);

    /// <summary>
    /// Downsizes bytes and adds appropriate prefix.
    /// </summary>
    public static string ToComputerSize(this ulong byteSize, string format, IFormatProvider formatProvider)
    {
        // Scale down bytes  
        const int thousand = 1024;
        var thousandCounter = 0;

        // Integral division 
        var integralLimit = (ulong)Math.Pow(thousand, 2);
        while (byteSize >= integralLimit)
        {
            thousandCounter++;
            byteSize /= thousand;
        }

        // Double division 
        double scaledValue = byteSize;
        while (scaledValue >= thousand)
        {
            thousandCounter++;
            scaledValue /= thousand;
        }

        // Get prefix
        var prefixes = new[] { "", "K", "M", "G", "T", "P", "E", "Z", "Y" };
        var prefix = prefixes[thousandCounter];

        return scaledValue.ToString(format, formatProvider) + prefix + "B";
    }

    /// <inheritdoc cref="ToComputerSize(ulong, string, IFormatProvider)"/>
    public static string ToComputerSizeCI(this ulong byteSize)
        => byteSize.ToComputerSize("0.###", CultureInfo.InvariantCulture);
}

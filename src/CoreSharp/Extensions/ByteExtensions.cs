namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="byte"/> extensions.
/// </summary>
public static class ByteExtensions
{
    /// <inheritdoc cref="DecimalExtensions.Map(decimal, decimal, decimal, decimal, decimal)"/>
    public static byte Map(this byte value, byte fromLow, byte fromHigh, byte toLow, byte toHigh)
    {
        var dValue = (decimal)value;
        var dFromLow = (decimal)fromLow;
        var dFromHigh = (decimal)fromHigh;
        var dToLow = (decimal)toLow;
        var dToHigh = (decimal)toHigh;
        var dResult = dValue.Map(dFromLow, dFromHigh, dToLow, dToHigh);
        return (byte)dResult;
    }
}

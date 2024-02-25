namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="int"/> extensions.
/// </summary>
public static class IntExtensions
{
    /// <inheritdoc cref="DecimalExtensions.Map(decimal, decimal, decimal, decimal, decimal)"/>
    public static int Map(this int value, int fromLow, int fromHigh, int toLow, int toHigh)
    {
        var dValue = (decimal)value;
        var dFromLow = (decimal)fromLow;
        var dFromHigh = (decimal)fromHigh;
        var dToLow = (decimal)toLow;
        var dToHigh = (decimal)toHigh;
        var dResult = dValue.Map(dFromLow, dFromHigh, dToLow, dToHigh);
        return (int)dResult;
    }
}

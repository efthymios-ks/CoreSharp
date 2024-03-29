﻿namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="float"/> extensions.
/// </summary>
public static class FloatExtensions
{
    /// <inheritdoc cref="DecimalExtensions.Map(decimal, decimal, decimal, decimal, decimal)"/>
    public static float Map(this float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        var dValue = (decimal)value;
        var dFromLow = (decimal)fromLow;
        var dFromHigh = (decimal)fromHigh;
        var dToLow = (decimal)toLow;
        var dToHigh = (decimal)toHigh;
        var dResult = dValue.Map(dFromLow, dFromHigh, dToLow, dToHigh);
        return (float)dResult;
    }
}

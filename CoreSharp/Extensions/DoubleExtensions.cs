using System;
using System.Globalization;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Double extensions.
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Re-maps a number from one range to another.
        /// </summary>
        public static double Map(this double value, double fromLow, double fromHigh, double toLow, double toHigh)
        {
            var dValue = (decimal)value;
            var dFromLow = (decimal)fromLow;
            var dFromHigh = (decimal)fromHigh;
            var dToLow = (decimal)toLow;
            var dToHigh = (decimal)toHigh;
            var dResult = dValue.Map(dFromLow, dFromHigh, dToLow, dToHigh);
            return (double)dResult;
        }

        /// <inheritdoc cref="ToMetricSize(double, string, IFormatProvider)"/>
        public static string ToMetricSize(this double value)
            => value.ToMetricSize("G");

        /// <inheritdoc cref="ToMetricSize(double, string, IFormatProvider)"/>
        public static string ToMetricSize(this double value, string format)
            => value.ToMetricSize(format, CultureInfo.CurrentCulture);

        /// <inheritdoc cref="ToMetricSize(double, string, IFormatProvider)"/>
        public static string ToMetricSize(this double value, IFormatProvider formatProvider)
            => value.ToMetricSize("G", formatProvider);

        /// <inheritdoc cref="ToMetricSize(double, string, IFormatProvider)"/>
        public static string ToMetricSizeCI(this double value)
            => value.ToMetricSize("0.###", CultureInfo.InvariantCulture);

        /// <summary>
        /// Convert value to SI string with appropriate prefix.
        /// </summary>
        public static string ToMetricSize(this double value, string format, IFormatProvider formatProvider)
        {
            var incPrefixes = new[] { 'k', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y' };
            var decPrefixes = new[] { 'm', 'u', 'n', 'p', 'f', 'a', 'z', 'y' };

            var degree = 0;
            if (value != 0)
                degree = (int)Math.Floor(Math.Log10(Math.Abs(value)) / 3);
            var scaledValue = value * Math.Pow(1000, -degree);

            //Get prefix 
            var degreeSign = Math.Sign(degree);
            char? prefix = degreeSign switch
            {
                1 => incPrefixes[degree - 1],
                -1 => decPrefixes[-degree - 1],
                _ => null
            };

            return scaledValue.ToString(format, formatProvider) + prefix;
        }
    }
}

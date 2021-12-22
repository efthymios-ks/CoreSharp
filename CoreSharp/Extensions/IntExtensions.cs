using System;

namespace CoreSharp.Extensions
{
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

        /// <summary>
        /// Returns a <see cref="DateTime"/> representing the specified
        /// <see cref="DateTime.Day"/> in January in the specified <see cref="DateTime.Year"/>.
        /// </summary>
        public static DateTime January(this int day, int year)
            => new(year, 1, day);

        /// <summary>
        /// Returns a <see cref="DateTime"/> representing the specified
        /// <see cref="DateTime.Day"/> in February in the specified <see cref="DateTime.Year"/>.
        /// </summary>
        public static DateTime February(this int day, int year)
            => new(year, 2, day);

        /// <summary>
        /// Returns a <see cref="DateTime"/> representing the specified
        /// <see cref="DateTime.Day"/> in March in the specified <see cref="DateTime.Year"/>.
        /// </summary>
        public static DateTime March(this int day, int year)
            => new(year, 3, day);

        /// <summary>
        /// Returns a <see cref="DateTime"/> representing the specified
        /// <see cref="DateTime.Day"/> in April in the specified <see cref="DateTime.Year"/>.
        /// </summary>
        public static DateTime April(this int day, int year)
            => new(year, 4, day);

        /// <summary>
        /// Returns a <see cref="DateTime"/> representing the specified
        /// <see cref="DateTime.Day"/> in May in the specified <see cref="DateTime.Year"/>.
        /// </summary>
        public static DateTime May(this int day, int year)
            => new(year, 5, day);

        /// <summary>
        /// Returns a <see cref="DateTime"/> representing the specified
        /// <see cref="DateTime.Day"/> in June in the specified <see cref="DateTime.Year"/>.
        /// </summary>
        public static DateTime June(this int day, int year)
            => new(year, 6, day);

        /// <summary>
        /// Returns a <see cref="DateTime"/> representing the specified
        /// <see cref="DateTime.Day"/> in July in the specified <see cref="DateTime.Year"/>.
        /// </summary>
        public static DateTime July(this int day, int year)
            => new(year, 7, day);

        /// <summary>
        /// Returns a <see cref="DateTime"/> representing the specified
        /// <see cref="DateTime.Day"/>in August in the specified <see cref="DateTime.Year"/>.
        /// </summary>
        public static DateTime August(this int day, int year)
            => new(year, 8, day);

        /// <summary>
        /// Returns a <see cref="DateTime"/> representing the specified
        /// <see cref="DateTime.Day"/> in September in the specified <see cref="DateTime.Year"/>.
        /// </summary>
        public static DateTime September(this int day, int year)
            => new(year, 9, day);

        /// <summary>
        /// Returns a <see cref="DateTime"/> representing the specified
        /// <see cref="DateTime.Day"/> in October in the specified <see cref="DateTime.Year"/>.
        /// </summary>
        public static DateTime October(this int day, int year)
            => new(year, 10, day);

        /// <summary>
        /// Returns a <see cref="DateTime"/> representing the specified
        /// <see cref="DateTime.Day"/> in November in the specified <see cref="DateTime.Year"/>.
        /// </summary>
        public static DateTime November(this int day, int year)
            => new(year, 11, day);

        /// <summary>
        /// Returns a <see cref="DateTime"/> representing the
        /// specified in December in the specified <see cref="DateTime.Year"/>.
        /// </summary>
        public static DateTime December(this int day, int year)
            => new(year, 12, day);
    }
}

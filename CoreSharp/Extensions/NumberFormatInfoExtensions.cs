using CoreSharp.Enums;
using System;
using System.Globalization;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// NumberFormatInfo extensions.
    /// </summary>
    public static class NumberFormatInfoExtensions
    {
        /// <summary>
        /// Set CurrencyPositivePattern from given enum.
        /// </summary>
        public static NumberFormatInfo SetCurrencyPositivePattern(this NumberFormatInfo info, CurrencyPositivePattern pattern)
        {
            _ = info ?? throw new ArgumentNullException(nameof(info));

            info.CurrencyPositivePattern = (int)pattern;

            return info;
        }

        /// <summary>
        /// Set CurrencyNegativePattern from given enum.
        /// </summary>
        public static NumberFormatInfo SetCurrencyNegativePattern(this NumberFormatInfo info, CurrencyNegativePattern pattern)
        {
            _ = info ?? throw new ArgumentNullException(nameof(info));

            info.CurrencyNegativePattern = (int)pattern;

            return info;
        }

        /// <summary>
        /// Set NumberNegativePattern from given enum.
        /// </summary>
        public static NumberFormatInfo SetNumberNegativePattern(this NumberFormatInfo info, NumberNegativePattern pattern)
        {
            _ = info ?? throw new ArgumentNullException(nameof(info));

            info.NumberNegativePattern = (int)pattern;

            return info;
        }
    }
}

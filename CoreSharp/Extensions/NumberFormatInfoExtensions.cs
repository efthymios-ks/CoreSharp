using CoreSharp.Enums;
using System;
using System.Globalization;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="NumberFormatInfo"/> extensions.
    /// </summary>
    public static class NumberFormatInfoExtensions
    {
        /// <summary>
        /// Map given <see cref="CurrencyPositivePattern"/> to <see cref="NumberFormatInfo.CurrencyPositivePattern"/>.
        /// </summary>
        public static NumberFormatInfo SetCurrencyPositivePattern(this NumberFormatInfo info, CurrencyPositivePattern pattern)
        {
            _ = info ?? throw new ArgumentNullException(nameof(info));

            info.CurrencyPositivePattern = (int)pattern;

            return info;
        }

        /// <summary>
        /// Map given <see cref="CurrencyNegativePattern"/> to <see cref="NumberFormatInfo.CurrencyNegativePattern"/>.
        /// </summary>
        public static NumberFormatInfo SetCurrencyNegativePattern(this NumberFormatInfo info, CurrencyNegativePattern pattern)
        {
            _ = info ?? throw new ArgumentNullException(nameof(info));

            info.CurrencyNegativePattern = (int)pattern;

            return info;
        }

        /// <summary>
        /// Map given <see cref="NumberNegativePattern"/> to <see cref="NumberFormatInfo.NumberNegativePattern"/>.
        /// </summary>
        public static NumberFormatInfo SetNumberNegativePattern(this NumberFormatInfo info, NumberNegativePattern pattern)
        {
            _ = info ?? throw new ArgumentNullException(nameof(info));

            info.NumberNegativePattern = (int)pattern;

            return info;
        }
    }
}

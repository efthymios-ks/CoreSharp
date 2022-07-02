using CoreSharp.Enums;
using System;
using System.Globalization;

namespace CoreSharp.Extensions;

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

    /// <summary>
    /// Map given <see cref="NumberFormatInfo.CurrencyPositivePattern"/> to <see cref="CurrencyPositivePattern"/>.
    /// </summary>
    public static CurrencyPositivePattern GetCurrencyPositivePattern(this NumberFormatInfo info)
    {
        _ = info ?? throw new ArgumentNullException(nameof(info));

        return (CurrencyPositivePattern)info.CurrencyPositivePattern;
    }

    /// <summary>
    /// Map given <see cref="NumberFormatInfo.CurrencyNegativePattern"/> to <see cref="CurrencyNegativePattern"/>.
    /// </summary>
    public static CurrencyNegativePattern GetCurrencyNegativePattern(this NumberFormatInfo info)
    {
        _ = info ?? throw new ArgumentNullException(nameof(info));

        return (CurrencyNegativePattern)info.CurrencyNegativePattern;
    }

    /// <summary>
    /// Map given <see cref="NumberFormatInfo.NumberNegativePattern"/> to <see cref="NumberNegativePattern"/>.
    /// </summary>
    public static NumberNegativePattern GetNumberNegativePattern(this NumberFormatInfo info)
    {
        _ = info ?? throw new ArgumentNullException(nameof(info));

        return (NumberNegativePattern)info.NumberNegativePattern;
    }
}

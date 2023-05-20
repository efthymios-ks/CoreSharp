using CoreSharp.Enums;
using System;
using System.Globalization;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="DateTime"/> extensions.
/// </summary>
public static class DateTimeExtensions
{
    /// <inheritdoc cref="GetElapsedTime(DateTime, DateTime)"/>
    public static TimeSpan GetElapsedTime(this DateTime endDate)
        => endDate.GetElapsedTime(DateTime.Now);

    /// <summary>
    /// Get the elapsed time between two <see cref="DateTime"/>
    /// using their UTC values.
    /// </summary>
    public static TimeSpan GetElapsedTime(this DateTime endDate, DateTime startDate)
    {
        static void ThrowDateTimeKindException(string paramName)
            => throw new ArgumentException($"{nameof(DateTime)}.{nameof(DateTime.Kind)} cannot be {DateTimeKind.Unspecified}.", paramName);

        if (endDate.Kind == DateTimeKind.Unspecified)
        {
            ThrowDateTimeKindException(nameof(endDate));
        }
        else if (startDate.Kind == DateTimeKind.Unspecified)
        {
            ThrowDateTimeKindException(nameof(endDate));
        }

        var universalStart = startDate.ToUniversalTime();
        var universalEnd = endDate.ToUniversalTime();
        return universalEnd - universalStart;
    }

    /// <inheritdoc cref="HasExpired(DateTime, DateTime, TimeSpan)"/>
    public static bool HasExpired(this DateTime endDate, TimeSpan duration)
        => endDate.HasExpired(DateTime.Now, duration);

    /// <summary>
    /// Check if specific <see cref="TimeSpan"/> has passed
    /// starting from provided <see cref="DateTime"/>.
    /// </summary>
    public static bool HasExpired(this DateTime endDate, DateTime startDate, TimeSpan duration)
    {
        static void ThrowDateTimeKindException(string paramName)
            => throw new ArgumentException($"{nameof(DateTime)}.{nameof(DateTime.Kind)} cannot be {DateTimeKind.Unspecified}.", paramName);

        if (endDate.Kind == DateTimeKind.Unspecified)
        {
            ThrowDateTimeKindException(nameof(endDate));
        }
        else if (startDate.Kind == DateTimeKind.Unspecified)
        {
            ThrowDateTimeKindException(nameof(endDate));
        }

        var universalStart = startDate.ToUniversalTime();
        var universalEnd = endDate.ToUniversalTime();
        return universalEnd < universalStart.Add(duration);
    }

    /// <summary>
    /// Check if <see cref="DateTime"/> is in a weekend.
    /// </summary>
    public static bool IsWeekend(this DateTime date)
        => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    /// <summary>
    /// Check if <see cref="DateTime"/> is in a leap year.
    /// </summary>
    public static bool IsInLeapYear(this DateTime date)
        => DateTime.IsLeapYear(date.Year);

    /// <summary>
    /// Trim part of <see cref="DateTime"/>.
    /// <example>
    /// <code>
    /// var date = DateTime.Now.Trim(DateTimePrecision.Milliseconds | DateTimePrecision.Seconds);
    /// </code>
    /// </example>
    /// </summary>
    public static DateTime Trim(this DateTime date, DateTimeParts parts)
    {
        if (parts.HasFlag(DateTimeParts.Milliseconds))
        {
            date = date.AddMilliseconds(-date.Millisecond);
        }

        if (parts.HasFlag(DateTimeParts.Seconds))
        {
            date = date.AddSeconds(-date.Second);
        }

        if (parts.HasFlag(DateTimeParts.Minutes))
        {
            date = date.AddMinutes(-date.Minute);
        }

        if (parts.HasFlag(DateTimeParts.Hours))
        {
            date = date.AddHours(-date.Hour);
        }

        if (parts.HasFlag(DateTimeParts.Days))
        {
            date = date.AddDays(-date.Day + 1);
        }

        if (parts.HasFlag(DateTimeParts.Months))
        {
            date = date.AddMonths(-date.Month + 1);
        }

        if (parts.HasFlag(DateTimeParts.Years))
        {
            date = date.AddYears(-date.Year + 1);
        }

        return date;
    }

    /// <summary>
    /// Convert <see cref="DateTime"/> to sortable format using "u" specifier.
    /// </summary>
    public static string ToStringSortable(this DateTime date)
        => date.ToString("u", CultureInfo.InvariantCulture);

    /// <summary>
    /// Convert <see cref="DateTime"/> to UTC sortable format using "ο" specifier.
    /// </summary>
    public static string ToStringSortableUtc(this DateTime date)
        => date.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture);

    /// <inheritdoc cref="DateTime.ToShortDateString"/>
    public static string ToShortDateString(this DateTime date, CultureInfo culture)
        => date.ToString(culture.DateTimeFormat.ShortDatePattern, culture);

    /// <inheritdoc cref="DateTime.ToLongDateString"/>
    public static string ToLongDateString(this DateTime date, CultureInfo culture)
        => date.ToString(culture.DateTimeFormat.LongDatePattern, culture);

    /// <inheritdoc cref="DateTime.ToShortTimeString"/>
    public static string ToShortTimeString(this DateTime date, CultureInfo culture)
        => date.ToString(culture.DateTimeFormat.ShortTimePattern, culture);

    /// <inheritdoc cref="DateTime.ToLongTimeString"/>
    public static string ToLongTimeString(this DateTime date, CultureInfo culture)
        => date.ToString(culture.DateTimeFormat.LongTimePattern, culture);
}

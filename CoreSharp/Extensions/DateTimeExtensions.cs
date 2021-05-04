using System;
using System.Globalization;
using CoreSharp.Enums;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// DateTime extensions. 
    /// </summary>
    public static partial class DateTimeExtensions
    {
        //TODO: Add mock test to `DateTime.GetElapsedTime()`. 
        /// <summary>
        /// Get the elapsed time since the input value. 
        /// </summary>
        public static TimeSpan GetElapsedTime(this DateTime from)
        {
            return DateTime.Now.Subtract(from);
        }

        /// <summary>
        /// Check if specific TimeSpan has passed since a DateTime.
        /// </summary>
        public static bool HasExpired(this DateTime from, TimeSpan duration)
        {
            var elapsed = from.GetElapsedTime();
            return Math.Abs(elapsed.TotalMilliseconds) > duration.TotalMilliseconds;
        }

        /// <summary>
        /// Check if date is in a weekend. 
        /// </summary> 
        public static bool IsWeekend(this DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday)
                return true;
            else if (date.DayOfWeek == DayOfWeek.Sunday)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if date is in a leap year. 
        /// </summary> 
        public static bool IsInLeapYear(this DateTime date)
        {
            return DateTime.IsLeapYear(date.Year);
        }

        /// <summary>
        /// Trim part of DateTime. 
        /// </summary>
        /// <example>
        /// var date = DateTime.Now.Trim(DateTimePrecision.Milliseconds | DateTimePrecision.Seconds); 
        /// </example>
        public static DateTime Trim(this DateTime date, DateTimePrecision trimFlags)
        {
            if (trimFlags.HasFlag(DateTimePrecision.Milliseconds))
                date = date.AddMilliseconds(-date.Millisecond);
            if (trimFlags.HasFlag(DateTimePrecision.Seconds))
                date = date.AddSeconds(-date.Second);
            if (trimFlags.HasFlag(DateTimePrecision.Minutes))
                date = date.AddMinutes(-date.Minute);
            if (trimFlags.HasFlag(DateTimePrecision.Hours))
                date = date.AddHours(-date.Hour);
            if (trimFlags.HasFlag(DateTimePrecision.Days))
                date = date.AddDays(-date.Day + 1);
            if (trimFlags.HasFlag(DateTimePrecision.Months))
                date = date.AddMonths(-date.Month + 1);
            if (trimFlags.HasFlag(DateTimePrecision.Years))
                date = date.AddYears(-date.Year + 1);

            return date;
        }

        /// <summary>
        /// Convert DateTime to sortable format using "u" specifier. 
        /// </summary> 
        public static string ToStringSortable(this DateTime date)
        {
            return date.ToString("u", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Convert DateTime to UTC sortable format using "u" specifier. 
        /// </summary> 
        public static string ToStringSortableUtc(this DateTime date)
        {
            return date.ToUniversalTime().ToStringSortable();
        }
    }
}

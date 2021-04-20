using System;

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
    }
}

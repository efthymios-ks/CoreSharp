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

        /// <summary>
        /// Returns a DateTime representing the specified day 
        /// in January in the specified year.
        /// </summary>
        public static DateTime January(this int day, int year)
        {
            return new DateTime(year, 1, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day 
        /// in February in the specified year.
        /// </summary>
        public static DateTime February(this int day, int year)
        {
            return new DateTime(year, 2, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day 
        /// in March in the specified year.
        /// </summary>
        public static DateTime March(this int day, int year)
        {
            return new DateTime(year, 3, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day 
        /// in April in the specified year.
        /// </summary>
        public static DateTime April(this int day, int year)
        {
            return new DateTime(year, 4, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day 
        /// in May in the specified year.
        /// </summary>
        public static DateTime May(this int day, int year)
        {
            return new DateTime(year, 5, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day 
        /// in June in the specified year.
        /// </summary>
        public static DateTime June(this int day, int year)
        {
            return new DateTime(year, 6, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day 
        /// in July in the specified year.
        /// </summary>
        public static DateTime July(this int day, int year)
        {
            return new DateTime(year, 7, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day 
        /// in August in the specified year.
        /// </summary>
        public static DateTime August(this int day, int year)
        {
            return new DateTime(year, 8, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day 
        /// in September in the specified year.
        /// </summary>
        public static DateTime September(this int day, int year)
        {
            return new DateTime(year, 9, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day 
        /// in October in the specified year.
        /// </summary>
        public static DateTime October(this int day, int year)
        {
            return new DateTime(year, 10, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day 
        /// in November in the specified year.
        /// </summary>
        public static DateTime November(this int day, int year)
        {
            return new DateTime(year, 11, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in 
        /// December in the specified year.
        /// </summary>
        public static DateTime December(this int day, int year)
        {
            return new DateTime(year, 12, day);
        }
    }
}

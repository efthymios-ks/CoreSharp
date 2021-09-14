using System;
using System.Collections.Generic;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// TimeSpan extensions.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Convert TimeSpan to human readable string.
        /// </summary>
        public static string ToStringReadable(this TimeSpan time)
        {
            var fields = new List<string>();

            void Add(int value, string unit)
            {
                if (value > 0)
                    fields.Add($"{value}{unit}");
            }

            Add(time.Days, "d");
            Add(time.Hours, "h");
            Add(time.Minutes, "m");
            Add(time.Seconds, "s");
            Add(time.Milliseconds, "ms");

            return string.Join(" ", fields);
        }
    }
}

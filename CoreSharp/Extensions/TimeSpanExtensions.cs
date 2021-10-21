using System;
using System.Collections.Generic;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="TimeSpan"/> extensions.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <inheritdoc cref="ToStringReadable(TimeSpan)" />
        public static string ToStringReadable(this TimeSpan? time)
            => (time ?? TimeSpan.Zero).ToStringReadable();

        /// <summary>
        /// Convert TimeSpan to human readable string.
        /// </summary>
        public static string ToStringReadable(this TimeSpan time)
        {
            if (time == TimeSpan.Zero)
                return "0ms";

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

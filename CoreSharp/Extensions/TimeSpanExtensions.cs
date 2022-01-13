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
        public static string ToStringReadable(this TimeSpan? timeSpan)
            => (timeSpan ?? TimeSpan.Zero).ToStringReadable();

        /// <summary>
        /// Convert <see cref="TimeSpan"/> to human readable string.
        /// </summary>
        public static string ToStringReadable(this TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero)
                return "0ms";

            var fields = new List<string>();

            //Negate, if negative 
            if (timeSpan < TimeSpan.Zero)
            {
                fields.Add("-");
                if (timeSpan == TimeSpan.MinValue)
                    timeSpan = TimeSpan.MaxValue;
                else
                    timeSpan = timeSpan.Negate();
            }

            //Add values 
            void Add(int value, string unit)
            {
                if (value != 0)
                    fields.Add($"{value}{unit}");
            }

            Add(timeSpan.Days, "d");
            Add(timeSpan.Hours, "h");
            Add(timeSpan.Minutes, "m");
            Add(timeSpan.Seconds, "s");
            Add(timeSpan.Milliseconds, "ms");

            //Join and return
            return string.Join(" ", fields);
        }
    }
}

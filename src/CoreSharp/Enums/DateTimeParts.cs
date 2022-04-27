using System;

namespace CoreSharp.Enums
{
    [Flags]
    public enum DateTimeParts
    {
        None = 0,
        Milliseconds = 1,
        Seconds = 2,
        Minutes = 4,
        Hours = 8,
        Time = Hours | Minutes | Seconds | Milliseconds,
        Days = 16,
        Months = 32,
        Years = 64,
        Date = Years | Months | Days
    }
}

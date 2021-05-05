using System;

namespace CoreSharp.Enums
{
    [Flags]
    public enum DateTimeParts
    {
        Milliseconds = 1,
        Seconds = 2,
        Minutes = 4,
        Hours = 8,
        Days = 16,
        Months = 32,
        Years = 64,
        Time = Hours | Minutes | Seconds | Milliseconds,
        Date = Years | Months | Days
    }
}

using System;
using System.Globalization;

namespace CoreSharp.Enums;

/// <summary>
/// <see cref="Enum"/> used with <see cref="NumberFormatInfo.NumberNegativePattern"/>.
/// Can be cast to <see cref="int"/>.
/// </summary>
public enum NumberNegativePattern
{
    /// <summary>
    /// (n)
    /// </summary>
    ParenthesisNumber = 0,

    /// <summary>
    /// -n
    /// </summary>
    SignNumber = 1,

    /// <summary>
    /// - n
    /// </summary>
    SignSpaceNumber = 2,

    /// <summary>
    /// n-
    /// </summary>
    NumberSign = 3,

    /// <summary>
    /// n -
    /// </summary>
    NumberSpaceSign = 4
}

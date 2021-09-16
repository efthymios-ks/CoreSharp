using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// String utilities.
    /// </summary>
    public static class String
    {
        /// <inheritdoc cref="FirstNotEmpty(string[])"/>
        public static string FirstNotEmpty(IEnumerable<string> values)
            => FirstNotEmpty(values?.ToArray());

        /// <summary>
        /// Return first value not null or whitespace.
        /// </summary>
        public static string FirstNotEmpty(params string[] values)
            => values?.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));
    }
}

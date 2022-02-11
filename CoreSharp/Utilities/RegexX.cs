using CoreSharp.Regexs;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// <see cref="Regex"/> utilities.
    /// </summary>
    public static class RegexX
    {
        //Methods
        /// <summary>
        /// Reduces many consequtive occurences of a char to single one.
        /// <code>
        /// var input = "11.2.3.4";
        /// var result = RegexX.ReduceMany(input, '1'); // 1.2.3.4
        /// </code>
        /// </summary>
        public static string ReduceMany(string input, char occurence)
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));

            var escapedOccurence = Regex.Escape($"{occurence}");
            var pattern = $"{escapedOccurence}{{2,}}";
            return Regex.Replace(input, pattern, $"{occurence}");
        }

        /// <inheritdoc cref="IsInteger(string, CultureInfo)"/>
        public static bool IsInteger(string input)
            => IsInteger(input, CultureInfo.CurrentCulture);

        /// <inheritdoc cref="IntegerRegex"/>
        public static bool IsInteger(string input, CultureInfo cultureInfo)
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));
            _ = cultureInfo ?? throw new ArgumentNullException(nameof(cultureInfo));

            return new IntegerRegex(input, cultureInfo).IsMatch;
        }
    }
}

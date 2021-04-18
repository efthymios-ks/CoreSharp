using CoreSharp.Sources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// String extensions.
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// Truncates string.
        /// </summary>
        /// <param name="input">String to truncate.</param>
        /// <param name="length">Maximum string length.</param>
        public static string Truncate(this string input, int length)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            int maxLength = Math.Min(input.Length, length);
            return input.Substring(0, maxLength);
        }

        /// <summary>
        /// Replace each ASCII control character with its corresponding abbreviation.
        /// </summary> 
        public static string FormatAsciiControls(this string input, char openBracket = '<', char closeBracked = '>')
        {
            input = input ?? throw new ArgumentNullException(nameof(input));

            var formatedControls = new Dictionary<string, string>();
            foreach (var control in AsciiControls.Dictionary)
                formatedControls.Add(control.Key.ToString(), $"{openBracket}{control.Value}{closeBracked}");

            foreach (var control in formatedControls)
                input = input.Replace(control.Key, control.Value);

            return input;
        }

        /// <summary>
        /// Split text into fixed-length chuncks.
        /// </summary>
        public static IEnumerable<string> SplitChunks(this string input, int chunkSize)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            if (chunkSize < 1)
                throw new ArgumentOutOfRangeException($"{nameof(chunkSize)} has to be greater than 0.");

            return input.SplitChunksInternal(chunkSize);
        }

        private static IEnumerable<string> SplitChunksInternal(this string input, int chuckSize)
        {
            int index = 0;

            while ((index + chuckSize) < input.Length)
            {
                yield return input.Substring(index, chuckSize);
                index += chuckSize;
            }

            yield return input.Substring(index);
        }

        /// <summary>
        /// Center align text.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="totalWidth"></param>
        /// <param name="paddingChar"></param>
        /// <returns></returns>
        public static string PadCenter(this string input, int totalWidth, char paddingChar = ' ')
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            if (totalWidth < 0)
                throw new ArgumentOutOfRangeException($"{nameof(totalWidth)} has to be zero or greater.");

            int padding = totalWidth - input.Length;
            int padLeft = padding / 2 + input.Length;

            return input.PadLeft(padLeft, paddingChar).PadRight(totalWidth, paddingChar);
        }

        /// <summary>
        /// Removes the first occurence of a given value. 
        /// </summary>
        public static string RemoveFirst(this string input, string remove)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            remove = remove ?? throw new ArgumentNullException(nameof(remove));

            int index = input.IndexOf(remove);
            if (index >= 0)
                input = input.Remove(index, remove.Length);

            return input;
        }

        /// <summary>
        /// Remove all the occurences of a given value. 
        /// </summary>
        public static string RemoveAll(this string input, string remove)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            remove = remove ?? throw new ArgumentNullException(nameof(remove));

            return input.Replace(remove, string.Empty);
        }

        /// <summary>
        /// Take left N characters. Similar to Sql.Functions.Left.
        /// </summary>
        public static string Left(this string input, int length)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            if (length <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(length)} has to be greater than 0.");

            if (length <= input.Length)
                return input.Substring(0, length);
            else
                return input;
        }

        /// <summary>
        /// Take right N characters. Similar to Sql.Functions.Right.
        /// </summary>
        public static string Right(this string input, int length)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            if (length <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(length)} has to be greater than 0.");

            if (length <= input.Length)
            {
                int start = input.Length - length;
                return input.Substring(start, length);
            }
            else
                return input;
        }

        /// <summary>
        /// Take substring from given index. Similar to Sql.Functions.Mid.
        /// </summary>
        public static string Mid(this string input, int start)
        {
            return input.Substring(start, input.Length);
        }

        /// <summary>
        /// Take N characters from given index. Similar to Sql.Functions.Mid.
        /// </summary>
        public static string Mid(this string input, int start, int length)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            if (start <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(start)} has to be greater than 0.");
            else if (start > input.Length)
                throw new ArgumentOutOfRangeException($"{nameof(start)} cannot be greater than {nameof(input)}.Length ({input.Length}).");
            else if (length <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(length)} has to be greater than 0.");

            if ((start + length) > input.Length)
                return input.Substring(start);
            else
                return input.Substring(start, length);
        }

        /// <summary>
        /// String.Format with InvariantCulture.
        /// </summary>
        public static string FormatWithCI(this string format, params object[] parameters)
        {
            return format.FormatWith(CultureInfo.InvariantCulture, parameters);
        }

        /// <summary>
        /// String.Format with custom IFormatProvider setting.
        /// </summary>
        public static string FormatWith(this string format, params object[] parameters)
        {
            return string.Format(CultureInfo.CurrentCulture, format, parameters);
        }

        /// <summary>
        /// String.Format with custom IFormatProvider setting.
        /// </summary>
        public static string FormatWith(this string format, IFormatProvider formatProvider, params object[] parameters)
        {
            format = format ?? throw new ArgumentNullException(nameof(format));
            formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
            parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));

            return string.Format(formatProvider, format, parameters);
        }

        /// <summary>
        /// Check if string contains substring with StringComparison settings.
        /// </summary>
        public static bool Contains(this string input, string value, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            value = value ?? throw new ArgumentNullException(nameof(value));

            int index = input.IndexOf(value, comparison);
            return index >= 0;
        }

        /// <summary>
        /// Check if two Strings match with StringComparison setting.
        /// </summary>
        public static bool Matches(this string input, string compareWith, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            compareWith = compareWith ?? throw new ArgumentNullException(nameof(compareWith));

            return string.Equals(input, compareWith, comparison);
        }

        /// <summary>
        /// Check if given input equals to any of the given values (StringComparison.InvariantCultureIgnoreCase).
        /// </summary>
        public static bool EqualsAny(this string input, params string[] values)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            values = values ?? throw new ArgumentNullException(nameof(values));

            return values.Any(i => input.Equals(i, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Check if given input starts with any of the given values (StringComparison.InvariantCultureIgnoreCase).
        /// </summary>
        public static bool StartsWithAny(this string input, params string[] values)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            values = values ?? throw new ArgumentNullException(nameof(values));

            return values.Any(i => input.StartsWith(i, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Check if given input ends with any of the given values (StringComparison.InvariantCultureIgnoreCase).
        /// </summary>
        public static bool EndsWithAny(this string input, params string[] values)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            values = values ?? throw new ArgumentNullException(nameof(values));

            return values.Any(i => input.EndsWith(i, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Check if given Input contains any of the given Values (StringComparison.InvariantCultureIgnoreCase).
        /// </summary>
        public static bool ContainsAny(this string input, params string[] values)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            values = values ?? throw new ArgumentNullException(nameof(values));

            return values.Any(i => input.Contains(i, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Check if Input string contains specific character.
        /// </summary>
        public static bool Contains(this string input, char value)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));

            return input.IndexOf(value) >= 0;
        }

        /// <summary>
        /// Check if input is null or empty. 
        /// </summary>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// Check if input is null or whitespace. 
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// Reverse a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Reverse(this string input)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));

            var array = input.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        /// <summary>
        /// Erase given value from String.
        /// </summary>
        public static string Erase(this string input, char value)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));

            return input.Erase($"{value}");
        }

        /// <summary>
        /// Erase given value from string.
        /// </summary>
        public static string Erase(this string input, string value)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));

            return input.Replace(value, string.Empty);
        }

        /// <summary>
        /// Trim with null check. 
        /// </summary> 
        public static string SafeTrim(this string input)
        {
            return (input ?? string.Empty).Trim();
        }
    }
}
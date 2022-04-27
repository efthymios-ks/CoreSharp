using System;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// <see cref="Exception"/> utilities.
    /// </summary>
    internal static class ExceptionX
    {
        /// <summary>
        /// Reusable error messages.
        /// </summary>
        public static class Messages
        {
            public static string Null(string paramName)
                => $"`{paramName}` must be null.";

            public static string NotNull(string paramName)
                => $"`{paramName}` cannot be null.";

            public static string Equal<TValue>(string paramName, TValue value)
                => $"`{paramName}` must be `{value}`.";

            public static string NotEqual<TValue>(string paramName, TValue value)
                => $"`{paramName}` cannot not be `{value}`.";

            public static string Zero(string paramName)
                => $"`{paramName}` must be zero.";

            public static string NotZero(string paramName)
                => $"`{paramName}` cannot be zero.";

            public static string Min<TValue>(string paramName, TValue min)
                => $"`{paramName}` cannot be less than `{min}`.";

            public static string Max<TValue>(string paramName, TValue max)
                => $"`{paramName}` cannot be greater than `{max}`.";

            public static string GreaterThan<TValue>(string paramName, TValue value)
                => $"`{paramName}` must be greater than `{value}`.";

            public static string LessThan<TValue>(string paramName, TValue value)
                => $"`{paramName}` must be less than `{value}`.";

            public static string Positive(string paramName)
                => $"{paramName} must be greater than zero.";

            public static string Negative(string paramName)
                => $"{paramName} must be less than zero.";

            public static string NotPositive(string paramName)
                => $"{paramName} must be zero less.";

            public static string NotNegative(string paramName)
                => $"{paramName} must zeror or greater.";

            public static string InRange<TValue>(string paramName, TValue from, TValue to)
                => $"{paramName} must be between {from} and {to}.";

            public static string Empty(string paramName)
                => $"{paramName} must be empty.";

            public static string NotEmpty(string paramName)
                => $"{paramName} cannot be empty.";
        }
    }
}

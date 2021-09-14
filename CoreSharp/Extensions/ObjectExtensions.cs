using System;
using System.Globalization;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Object extensions.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Check if the runtime type of an expression is
        /// compatible with a given type.
        /// </summary>
        public static bool Is<T>(this object input) => input is T;

        /// <summary>
        /// Explicitly convert an expression to a given type
        /// if its runtime type is compatible with that type.
        /// </summary>
        public static T As<T>(this object input) where T : class => input as T;

        /// <inheritdoc cref="AsOrDefault{T}(object, T)"/>
        public static T AsOrDefault<T>(this object input)
        {
            T defaultValue = default;
            if (typeof(T) == typeof(string))
                defaultValue = (T)(object)string.Empty;
            return input.AsOrDefault(defaultValue);
        }

        /// <summary>
        /// Try casting input to given type and
        /// return default if null or of different type.
        /// </summary>
        public static T AsOrDefault<T>(this object input, T defaultValue)
        {
            if (input is null)
                return defaultValue;
            else if (DBNull.Value.Equals(input))
                return defaultValue;
            else if (input.GetType() != typeof(T))
                return defaultValue;
            else if (input is not T t)
                return defaultValue;
            else
                return t;
        }

        //TODO: Add unit tests 
        /// <inheritdoc cref="ChangeType{TResult}(object, CultureInfo)"/>
        public static TResult ChangeType<TResult>(this object value)
            => value.ChangeType<TResult>(CultureInfo.CurrentCulture);

        /// <summary>
        /// Shortcut for (TResult)Convert.ChangeType(CultureInfo).
        /// </summary>
        public static TResult ChangeType<TResult>(this object value, CultureInfo cultureInfo)
        {
            if (value is null)
                return default;

            var baseType = typeof(TResult);
            baseType = Nullable.GetUnderlyingType(baseType) ?? baseType;
            return (TResult)Convert.ChangeType(value, baseType, cultureInfo);
        }
    }
}

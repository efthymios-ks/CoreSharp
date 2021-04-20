using System;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Object extensions. 
    /// </summary>
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Check if the runtime type of an expression is 
        /// compatible with a given type. 
        /// </summary>
        public static bool Is<T>(this object input)
        {
            return input is T;
        }

        /// <summary>
        /// Explicitly convert an expression to a given type 
        /// if its runtime type is compatible with that type. 
        /// </summary>
        public static T As<T>(this object input) where T : class
        {
            return input as T;
        }

        /// <summary>
        /// Try casting input to given type and 
        /// return default if null or of different type. 
        /// </summary>
        public static T TryCast<T>(this object input)
        {
            T defaultValue = default;
            if (typeof(T) == typeof(string))
                defaultValue = (T)(object)string.Empty;
            return input.TryCast(defaultValue);
        }

        /// <summary>
        /// Try casting input to given type and 
        /// return default if null or of different type. 
        /// </summary>
        public static T TryCast<T>(this object input, T defaultValue)
        {
            if (input == null)
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
    }
}

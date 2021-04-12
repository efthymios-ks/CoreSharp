using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Enum extensions. 
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Get enum values. 
        /// </summary>
        public static IEnumerable<TEnum> GetValues<TEnum>() where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException($"{typeof(TEnum).FullName} is not an enum.");
            else
                return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        /// <summary>
        /// Get enum dictionary (Text-Value). 
        /// </summary>
        public static IDictionary<string, TEnum> GetDictionary<TEnum>() where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException($"{typeof(TEnum).FullName} is not an enum.");
            else
            {
                var dictionary = new Dictionary<string, TEnum>();
                var values = GetValues<TEnum>();
                foreach (var value in values)
                    dictionary.Add($"{value}", value);
                return dictionary;
            }
        }
    }
}

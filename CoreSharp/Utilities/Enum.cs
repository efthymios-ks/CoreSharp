using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// Enum utilities.
    /// </summary>
    public static class Enum
    {
        /// <summary>
        /// Get enum values.
        /// </summary>
        public static IEnumerable<TEnum> GetValues<TEnum>() where TEnum : System.Enum
            => System.Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

        /// <summary>
        /// Get enum dictionary (Text-Value).
        /// </summary>
        public static IDictionary<string, TEnum> GetDictionary<TEnum>() where TEnum : System.Enum
        {
            var values = GetValues<TEnum>();
            return values.ToDictionary(key => $"{key}", value => value);
        }
    }
}

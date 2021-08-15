using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
            var dictionary = GetDictionary<TEnum>();
            return dictionary.Select(i => i.Value);
        }

        /// <summary>
        /// Get enum dictionary (Text-Value). 
        /// </summary>
        public static IDictionary<string, TEnum> GetDictionary<TEnum>() where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException($"{typeof(TEnum).FullName} is not an enum.", nameof(TEnum));
            else
            {
                var dictionary = new Dictionary<string, TEnum>();
                var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
                foreach (var value in values)
                    dictionary.Add($"{value}", value);
                return dictionary;
            }
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var fieldInfo = value.GetType().GetField($"{value}");
            var attributes = fieldInfo?.GetCustomAttributes(typeof(TAttribute), true)?.Cast<TAttribute>();
            return attributes?.FirstOrDefault();
        }

        /// <summary>
        /// Get Description attribute from an enum. 
        /// </summary> 
        public static string GetDescription(this Enum value)
        {
            var descriptionAttribute = value.GetAttribute<DescriptionAttribute>();
            return descriptionAttribute?.Description ?? $"{value}";
        }

        /// <summary>
        /// Get Display.Name attribute from an enum. 
        /// </summary> 
        public static string GetDisplayName(this Enum value)
        {
            var displayAttribute = value.GetAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? $"{value}";
        }
    }
}

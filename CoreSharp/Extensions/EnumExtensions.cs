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

        /// <summary>
        /// Get Description attribute from an enum. 
        /// </summary> 
        public static string GetDescription<TEnum>(this TEnum value) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException($"{typeof(TEnum).FullName} is not an enum.", nameof(TEnum));

            string valueString = $"{value}";

            //Get first attribute of type 'DescriptionAttribute'
            var fieldInfo = value.GetType().GetField(valueString);
            var descriptionAttributes = fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), true)?.Cast<DescriptionAttribute>();
            var descriptionAttribute = descriptionAttributes?.FirstOrDefault();

            //Return attribute or enum itself as description 
            return descriptionAttribute?.Description ?? valueString;
        }

        /// <summary>
        /// Get Display.Name attribute from an enum. 
        /// </summary> 
        public static string GetDisplayName<TEnum>(this TEnum value) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException($"{typeof(TEnum).FullName} is not an enum.", nameof(TEnum));

            string valueString = $"{value}";

            //Get first attribute of type 'DisplayAttribute'
            var fieldInfo = value.GetType().GetField(valueString);
            var displayAttributes = fieldInfo?.GetCustomAttributes(typeof(DisplayAttribute), true)?.Cast<DisplayAttribute>();
            var displayAttribute = displayAttributes?.FirstOrDefault();

            //Return attribute or enum itself as description 
            return displayAttribute?.Name ?? valueString;
        }
    }
}

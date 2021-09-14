using System;
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
        /// Get specific attribute from enum.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));
            var fieldInfo = value.GetType().GetField($"{value}");
            var attributes = fieldInfo?.GetCustomAttributes(typeof(TAttribute), true)?.Cast<TAttribute>();
            return attributes?.FirstOrDefault();
        }

        /// <summary>
        /// Get <see cref="DescriptionAttribute"/> attribute from an enum.
        /// </summary>
        public static string GetDescription(this Enum value)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));
            var descriptionAttribute = value.GetAttribute<DescriptionAttribute>();
            return descriptionAttribute?.Description ?? $"{value}";
        }

        /// <summary>
        /// Get <see cref="DisplayAttribute.Name"/> attribute from an enum.
        /// </summary>
        public static string GetDisplayName(this Enum value)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));
            var displayAttribute = value.GetAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? $"{value}";
        }
    }
}

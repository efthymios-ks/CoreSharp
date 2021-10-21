using System;
using System.ComponentModel.DataAnnotations;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="Enum"/> extensions.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Get given <see cref="Attribute"/> from an <see cref="Enum"/>.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));

            var enumField = value.GetType().GetField($"{value}");
            return enumField.GetAttribute<TAttribute>();
        }

        /// <summary>
        /// Get <see cref="DisplayAttribute"/> attribute from an <see cref="Enum"/>.
        /// </summary>
        public static DisplayAttribute GetDisplay(this Enum value)
            => value.GetAttribute<DisplayAttribute>();

        /// <summary>
        /// Get <see cref="DisplayAttribute.Name"/> attribute from an <see cref="Enum"/>.
        /// </summary>
        public static string GetDisplayName(this Enum value)
            => value.GetDisplay()?.Name ?? $"{value}";

        /// <summary>
        /// Get <see cref="DisplayAttribute.ShortName"/> attribute from an <see cref="Enum"/>.
        /// </summary>
        public static string GetDisplayShortName(this Enum value)
            => value.GetDisplay()?.ShortName ?? $"{value}";

        /// <summary>
        /// Get <see cref="DisplayAttribute.Description"/> attribute from an <see cref="Enum"/>.
        /// </summary>
        public static string GetDisplayDescription(this Enum value)
            => value.GetDisplay()?.Description ?? $"{value}";
    }
}

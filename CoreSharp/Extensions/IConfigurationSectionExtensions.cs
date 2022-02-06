using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="IConfigurationSection"/> extensions.
    /// </summary>
    public static class IConfigurationSectionExtensions
    {
        /// <inheritdoc cref="Get{TResult}(IConfigurationSection, string)"/>
        public static TResult Get<TResult>(this IConfigurationSection section)
            where TResult : class
            => section.Get<TResult>(null);

        /// <summary>
        /// Get the <see cref="IConfigurationSection.Value"/>
        /// and map it provided generic.
        /// </summary>
        public static TResult Get<TResult>(this IConfigurationSection section, string key)
            where TResult : class
        {
            _ = section ?? throw new ArgumentNullException(nameof(section));

            var json = section.Get(key);
            return JsonSerializer.Deserialize<TResult>(json);
        }

        /// <inheritdoc cref="Get(IConfigurationSection, string)"/>
        public static string Get(this IConfigurationSection section)
            => section.Get(null);

        /// <summary>
        /// Get the <see cref="IConfigurationSection.Value"/> value as <see cref="string"/>.
        /// </summary>
        public static string Get(this IConfigurationSection section, string key)
        {
            _ = section ?? throw new ArgumentNullException(nameof(section));

            if (!string.IsNullOrWhiteSpace(key))
                section = section.GetSection(key);

            return section.Value;
        }
    }
}

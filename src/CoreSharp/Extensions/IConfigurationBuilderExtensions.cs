using CoreSharp.ConfigurationProviders;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="IConfigurationBuilder"/> extensions.
    /// </summary>
    public static class IConfigurationBuilderExtensions
    {
        /// <inheritdoc cref="AddEmbeddedFileConfiguration(IConfigurationBuilder, Action{EmbeddedFileConfigurationOptions})"/>
        public static IConfigurationBuilder AddEmbeddedFileConfiguration(this IConfigurationBuilder builder)
            => builder.AddEmbeddedFileConfiguration(options => options.ScanAssembly = Assembly.GetEntryAssembly());

        /// <summary>
        /// Adds a new <see cref="IConfigurationProvider"/> layer
        /// from <see langword="appsettings.json"/> stored as <see langword="EmbeddedResource"/> file.
        /// </summary>
        public static IConfigurationBuilder AddEmbeddedFileConfiguration(this IConfigurationBuilder builder, Action<EmbeddedFileConfigurationOptions> configure)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));
            _ = configure ?? throw new ArgumentNullException(nameof(configure));

            var options = new EmbeddedFileConfigurationOptions();
            configure(options);
            var source = new EmbeddedFileConfigurationSource(options);
            return builder.Add(source);
        }
    }
}

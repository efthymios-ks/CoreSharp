using Microsoft.Extensions.Configuration;
using System;

namespace CoreSharp.ConfigurationProviders;

public sealed class EmbeddedFileConfigurationSource : IConfigurationSource
{
    // Fields
    private readonly EmbeddedFileConfigurationOptions _options;

    // Constructors
    public EmbeddedFileConfigurationSource(EmbeddedFileConfigurationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options;
    }

    // Methods
    public IConfigurationProvider Build(IConfigurationBuilder builder)
      => new EmbeddedFileConfigurationProvider(builder, _options);
}

using CoreSharp.Options;
using Microsoft.Extensions.Configuration;
using System;

namespace CoreSharp.ConfigurationProviders
{
    public class EmbeddedFileConfigurationSource : IConfigurationSource
    {
        //Fields
        private readonly EmbeddedFileConfiguration _options;

        //Constructors
        public EmbeddedFileConfigurationSource(EmbeddedFileConfiguration options)
            => _options = options ?? throw new ArgumentNullException(nameof(options));

        //Methods
        public IConfigurationProvider Build(IConfigurationBuilder builder)
          => new EmbeddedFileConfigurationProvider(builder, _options);
    }
}

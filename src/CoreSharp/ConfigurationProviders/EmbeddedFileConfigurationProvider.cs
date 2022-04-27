using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoreSharp.ConfigurationProviders
{
    public class EmbeddedFileConfigurationProvider : ConfigurationProvider
    {
        //Fields 
        private readonly IConfigurationBuilder _builder;
        private readonly IFileProvider _fileProvider;
        private readonly EmbeddedFileConfigurationOptions _options;

        //Constructors
        public EmbeddedFileConfigurationProvider(IConfigurationBuilder builder, EmbeddedFileConfigurationOptions options)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _ = _options.ScanAssembly ?? throw new NullReferenceException(nameof(options.ScanAssembly));

            _fileProvider = new EmbeddedFileProvider(_options.ScanAssembly);
        }

        //Methods
        public override void Load()
        {
            Data.Clear();

            var locations = new HashSet<string>
            {
                GetAppsettingsPath(_options.Location, null),
                GetAppsettingsPath(_options.Location, _options.Environment)
            };

            foreach (var location in locations)
                AddFile(location);
        }

        private static string GetAppsettingsPath(string location, string environmentName)
        {
            location ??= string.Empty;
            location = location.Replace('\\', '/').Trim('/');

            var fileName = string.IsNullOrWhiteSpace(environmentName)
                            ? "appsettings.json"
                            : $"appsettings.{environmentName}.json";

            return Path.Combine(location, fileName);
        }

        private void AddFile(string appSettingsPath)
        {
            if (string.IsNullOrWhiteSpace(appSettingsPath))
                throw new ArgumentNullException(nameof(appSettingsPath));

            var file = _fileProvider.GetFileInfo(appSettingsPath);
            if (!file.Exists)
                return;

            _builder.AddJsonFile(_fileProvider, appSettingsPath, optional: false, reloadOnChange: true);
        }
    }
}

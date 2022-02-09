using CoreSharp.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace CoreSharp.ConfigurationProviders
{
    public class EmbeddedFileConfigurationProvider : ConfigurationProvider
    {
        //Fields 
        private readonly IConfigurationBuilder _builder;
        private readonly IFileProvider _fileProvider;
        private readonly EmbeddedFileConfiguration _options;

        //Constructors
        public EmbeddedFileConfigurationProvider(IConfigurationBuilder builder, EmbeddedFileConfiguration options)
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

            AddFile(GetAppsettingsPath(_options.Location, null));

            if (!string.IsNullOrWhiteSpace(_options.Environment))
                AddFile(GetAppsettingsPath(_options.Location, _options.Environment));
        }

        private static string GetAppsettingsPath(string location, string environmentName)
        {
            location ??= string.Empty;
            location = location.Replace('\\', '/').Trim('/');

            environmentName ??= environmentName;
            var fileName = $"appsettings.{environmentName}.json".Replace("..", ".");

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

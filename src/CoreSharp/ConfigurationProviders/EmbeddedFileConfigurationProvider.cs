using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoreSharp.ConfigurationProviders;

public sealed class EmbeddedFileConfigurationProvider : ConfigurationProvider
{
    // Fields 
    private readonly IConfigurationBuilder _builder;
    private readonly EmbeddedFileConfigurationOptions _options;
    private readonly IFileProvider _fileProvider;

    // Constructors
    public EmbeddedFileConfigurationProvider(IConfigurationBuilder builder, EmbeddedFileConfigurationOptions options)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(options);
        _ = _options.ScanAssembly ?? throw new ArgumentException(nameof(options.ScanAssembly));

        _builder = builder;
        _options = options;
        _fileProvider = new EmbeddedFileProvider(_options.ScanAssembly);
    }

    // Methods
    public override void Load()
    {
        Data.Clear();

        var locations = new HashSet<string>
        {
            GetAppsettingsPath(_options.Location, null),
            GetAppsettingsPath(_options.Location, _options.Environment)
        };

        foreach (var location in locations)
        {
            AddFile(location);
        }
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
        ArgumentException.ThrowIfNullOrEmpty(appSettingsPath);

        var fileInfo = _fileProvider.GetFileInfo(appSettingsPath);
        if (!fileInfo.Exists)
        {
            return;
        }

        _builder.AddJsonFile(_fileProvider, appSettingsPath, optional: false, reloadOnChange: true);
    }
}

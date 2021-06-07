using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using CoreSharp.Interfaces.Localize;
using Microsoft.Extensions.FileProviders;

namespace CoreSharp.Implementations.TextLocalizer
{
    public class EmbeddedJsonTextLocalizer : ITextLocalizer
    {
        //Fields 
        private const string DefaultResourcesPath = "Resources";
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IFileProvider fileProvider;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IEnumerable<string> lookupPaths;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentDictionary<string, string> source = new();

        //Constructors
        public EmbeddedJsonTextLocalizer(IFileProvider fileProvider, CultureInfo culture, string resourcesPath, string resourceName)
        {
            this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            Culture = culture ?? throw new ArgumentNullException(nameof(culture));
            resourcesPath ??= string.Empty;
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentException($"{nameof(resourceName)} cannot be empty.", nameof(resourceName));

            lookupPaths = BuildLookupPaths(resourceName, resourcesPath, culture);
        }

        //Indexers  
        public string this[string key] => GetValue(key);

        //Properties
        public CultureInfo Culture { get; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool ShouldCache => source.IsEmpty;

        //Methods 
        private static string BuildLookupPath(string resourceName, string resourcesPath = null, CultureInfo culture = null)
        {
            var builder = new StringBuilder();

            //Relative path 
            if (string.IsNullOrWhiteSpace(resourcesPath))
                builder.Append(resourceName);
            else
                builder.Append(Path.Combine(resourcesPath, $"{resourceName}"));

            //Culture 
            if (culture != null)
            {
                var language = culture.TwoLetterISOLanguageName;
                int dashIndex = language.IndexOf('-');
                if (dashIndex >= 0)
                    language = language.Substring(0, dashIndex);
                builder.Append($"-{language}");
            }

            //Extension 
            builder.Append(".json");

            return builder.ToString();
        }

        private static IEnumerable<string> BuildLookupPaths(string typeName, string resourcesPath, CultureInfo culture)
        {
            var lookupPaths = new List<string>();

            //With culture 
            {
                //Requester directory 
                lookupPaths.Add(BuildLookupPath(typeName, null, culture));

                //Custom resources directory 
                if (!string.IsNullOrWhiteSpace(resourcesPath))
                    lookupPaths.Add(BuildLookupPath(typeName, resourcesPath, culture));

                //Default resources directory 
                if (!resourcesPath.Equals(DefaultResourcesPath, StringComparison.InvariantCultureIgnoreCase))
                    lookupPaths.Add(BuildLookupPath(typeName, DefaultResourcesPath, culture));
            }

            //Without culture 
            {
                //Requester directory 
                lookupPaths.Add(BuildLookupPath(typeName, null, null));

                //Custom resources directory 
                if (!string.IsNullOrWhiteSpace(resourcesPath))
                    lookupPaths.Add(BuildLookupPath(typeName, resourcesPath, null));

                //Default resources directory 
                if (!resourcesPath.Equals(DefaultResourcesPath, StringComparison.InvariantCultureIgnoreCase))
                    lookupPaths.Add(BuildLookupPath(typeName, DefaultResourcesPath, null));
            }

            return lookupPaths;
        }

        private void CacheDictionary()
        {
            //Get file 
            var lookupFiles = lookupPaths.Select(p => fileProvider.GetFileInfo(p));
            var resourceFile = lookupFiles.FirstOrDefault(f => f.Exists);
            if (resourceFile == null)
                return;

            //Deserialize file into dictionary
            using var stream = resourceFile.CreateReadStream();
            var dictionary = JsonSerializer
                                .DeserializeAsync<Dictionary<string, string>>(stream)
                                .AsTask()
                                .GetAwaiter()
                                .GetResult();
            foreach (var pair in dictionary)
                source.AddOrUpdate(pair.Key, pair.Value, (key, value) => pair.Value);
        }

        private string GetValue(string key)
        {
            key = key ?? throw new ArgumentNullException(nameof(key));

            if (ShouldCache)
                CacheDictionary();

            if (source.TryGetValue(key, out string value))
                return value;
            else
                return key;
        }
    }
}

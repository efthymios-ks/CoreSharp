using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using CoreSharp.Interfaces.CultureLocalizer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;

namespace CoreSharp.Implementations.EmbeddedJsonCultureLocalizer
{
    public class EmbeddedJsonCultureLocalizer : ICultureLocalizer
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
        public EmbeddedJsonCultureLocalizer(IFileProvider fileProvider, string resourcesPath, Type resourceType, CultureInfo culture)
        {
            this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            ResourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));
            Culture = culture ?? throw new ArgumentNullException(nameof(culture));
            resourcesPath ??= string.Empty;

            var resourceName = EmbeddedJsonCultureLocalizerFactory.GetResourceName(ResourceType);
            lookupPaths = BuildLookupPaths(resourceName, resourcesPath, culture);
        }

        //Indexers 
        LocalizedString IStringLocalizer.this[string name] => GetValue(name);

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var pair = GetValue(name);
                var value = string.Format(pair.Value, arguments);
                return new LocalizedString(name, value);
            }
        }

        //Properties
        public Type ResourceType { get; }

        public CultureInfo Culture { get; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool ShouldCache => source.IsEmpty;

        //Methods 
        private static string BuildLookupPath(string resourceName, string resourcesPath, string language)
        {
            var builder = new StringBuilder();

            //Relative path 
            if (string.IsNullOrWhiteSpace(resourcesPath))
                builder.Append(resourceName);
            else
                builder.Append(Path.Combine(resourcesPath, $"{resourceName}"));

            //Language 
            if (!string.IsNullOrWhiteSpace(language))
                builder.Append($"-{language}");

            //Extension 
            builder.Append(".json");

            return builder.ToString();
        }

        private static IEnumerable<string> BuildLookupPaths(string resourceName, string resourcesPath, CultureInfo culture)
        {
            //Setup languages 
            var languages = new HashSet<string>
            {
                //en-US 
                culture.Name,
                //en (fallback) 
                culture.TwoLetterISOLanguageName,
                //No language (fallback) 
                string.Empty
            };

            //Setup paths
            var paths = new HashSet<string>
            {
                //Next to requester 
                string.Empty,
                //Custom path 
                resourcesPath ?? string.Empty,
                //Resources 
                DefaultResourcesPath?? string.Empty
            };

            //Build paths 
            var lookupPaths = new HashSet<string>();
            foreach (var language in languages)
                foreach (var path in paths)
                    lookupPaths.Add(BuildLookupPath(resourceName, path, language));

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

        private LocalizedString GetValue(string name)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));

            if (ShouldCache)
                CacheDictionary();

            if (source.TryGetValue(name, out string value))
                return new LocalizedString(name, value);
            else
                return new LocalizedString(name, name);
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return source.Select(i => new LocalizedString(i.Key, i.Value));
        }
    }
}
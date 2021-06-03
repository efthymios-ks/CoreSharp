using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using CoreSharp.Interfaces.Localize;
using Microsoft.Extensions.FileProviders;

namespace CoreSharp.Implementations.TextLocalizer
{
    public class JsonTextLocalizer : ITextLocalizer
    {
        //Fields
        private readonly CultureInfo culture;
        private readonly IFileProvider fileProvider;
        private readonly string resourcesPath;
        private readonly string jsonName;
        private readonly ConcurrentDictionary<string, string> source = new ConcurrentDictionary<string, string>();

        //Constructors
        public JsonTextLocalizer(CultureInfo culture, IFileProvider fileProvider, string resourcesPath, string jsonName)
        {
            this.culture = culture ?? throw new ArgumentNullException(nameof(culture));
            this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            this.resourcesPath = resourcesPath;
            this.jsonName = jsonName;
        }

        //Indexers  
        public string this[string name]
        {
            get
            {
                return GetValue(name);
            }
        }

        public string this[string name, params object[] arguments]
        {
            get
            {
                var value = GetValue(name);
                return string.Format(value, arguments);
            }
        }

        //Properties
        private bool ShouldCache => source.IsEmpty;

        //Methods
        private string GetValue(string key)
        {
            if (ShouldCache)
                CacheDictionary();
            return source[key] ?? key;
        }

        private void CacheDictionary()
        {
            //Get file 
            var fileInfo = fileProvider.GetFileInfo(BuildJsonFileName(culture.TwoLetterISOLanguageName));
            if (!fileInfo.Exists)
                fileInfo = fileProvider.GetFileInfo(BuildJsonFileName());

            //Deserialize file into dictionary
            using var stream = fileInfo.CreateReadStream();
            var dictionary = JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream).AsTask().Result;
            foreach (var pair in dictionary)
                source.AddOrUpdate(pair.Key, pair.Value, (key, value) => pair.Value);
        }

        private string BuildJsonFileName(string cultureName = null)
        {
            var builder = new StringBuilder();
            builder.Append(Path.Combine(resourcesPath, $"{jsonName}"));
            if (!string.IsNullOrWhiteSpace(cultureName))
                builder.Append($"-{cultureName}");
            builder.Append(".json");
            return builder.ToString();
        }
    }
}

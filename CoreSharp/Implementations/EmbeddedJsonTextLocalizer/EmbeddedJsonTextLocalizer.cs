using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CoreSharp.Interfaces.Localize;
using Microsoft.Extensions.FileProviders;

namespace CoreSharp.Implementations.TextLocalizer
{
    public class EmbeddedJsonTextLocalizer : ITextLocalizer
    {
        //Fields 
        private readonly IFileProvider fileProvider;
        private readonly IEnumerable<string> lookupPaths;
        private readonly ConcurrentDictionary<string, string> source = new ConcurrentDictionary<string, string>();

        //Constructors
        public EmbeddedJsonTextLocalizer(IFileProvider fileProvider, IEnumerable<string> lookupPaths)
        {
            this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            this.lookupPaths = lookupPaths ?? throw new ArgumentNullException(nameof(lookupPaths));
        }

        //Indexers  
        public string this[string key] => GetValue(key);

        //Properties
        private bool ShouldCache => source.IsEmpty;

        //Methods 
        private void CacheDictionary()
        {
            //Get file 
            var lookupFiles = lookupPaths.Select(p => fileProvider.GetFileInfo(p));
            var resourceFile = lookupFiles.FirstOrDefault(f => f.Exists) ?? lookupFiles.LastOrDefault();

            //Deserialize file into dictionary
            using var stream = resourceFile.CreateReadStream();
            var dictionary = JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream).AsTask().Result;
            foreach (var pair in dictionary)
                source.AddOrUpdate(pair.Key, pair.Value, (key, value) => pair.Value);
        }

        private string GetValue(string key)
        {
            if (ShouldCache)
                CacheDictionary();

            if (source.TryGetValue(key, out string value))
                return value;
            else
                return key;
        }
    }
}

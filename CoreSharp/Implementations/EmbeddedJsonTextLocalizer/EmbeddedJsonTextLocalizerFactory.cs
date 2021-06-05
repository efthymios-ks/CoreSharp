using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using CoreSharp.Interfaces.Localize;
using Microsoft.Extensions.FileProviders;

namespace CoreSharp.Implementations.TextLocalizer
{
    public class EmbeddedJsonTextLocalizerFactory : ITextLocalizerFactory
    {
        //Properties
        private const string DefaultResourcesPath = "Resources";
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string resourcesPath;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentDictionary<string, ITextLocalizer> localizers = new();

        //Constructors
        public EmbeddedJsonTextLocalizerFactory(string resourcesPath)
        {
            this.resourcesPath = resourcesPath ?? string.Empty;
        }

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
                builder.Append($"-{culture.TwoLetterISOLanguageName}");

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

        private static string GetLocalizerKey(CultureInfo culture, string name)
        {
            culture = culture ?? throw new ArgumentNullException(nameof(culture));

            return $"Culture={culture.TwoLetterISOLanguageName}, Name={name}";
        }

        public ITextLocalizer Create<TResource>(CultureInfo culture)
        {
            culture = culture ?? throw new ArgumentNullException(nameof(culture));

            //Get type name 
            var resourceType = typeof(TResource);
            var assemblyName = resourceType.Assembly.GetName().Name;
            var resourceName = resourceType.FullName;
            if (resourceName.StartsWith(assemblyName))
                resourceName = resourceName[(assemblyName.Length + 1)..];

            //Build localizer key for caching  
            string localizerKey = GetLocalizerKey(culture, resourceName);

            //Cache
            if (!localizers.ContainsKey(localizerKey))
            {
                var lookupPaths = BuildLookupPaths(resourceName, resourcesPath, culture);
                var resourceProvider = new EmbeddedFileProvider(resourceType.Assembly);
                var localizer = new EmbeddedJsonTextLocalizer(resourceProvider, lookupPaths);
                localizers.AddOrUpdate(localizerKey, localizer, (key, value) => localizer);
            }

            //Return 
            return localizers[localizerKey];
        }
    }
}

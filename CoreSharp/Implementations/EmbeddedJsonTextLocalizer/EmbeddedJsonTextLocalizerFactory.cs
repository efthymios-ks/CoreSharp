using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private readonly string resourcesPath;
        private readonly ConcurrentDictionary<string, ITextLocalizer> localizers = new ConcurrentDictionary<string, ITextLocalizer>();

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

            //Extensions
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
            var typeName = resourceType.FullName;
            if (typeName.StartsWith(assemblyName))
                typeName = typeName[(assemblyName.Length + 1)..];

            //Build look-up paths 
            var lookupPaths = BuildLookupPaths(typeName, resourcesPath, culture);

            //Build localizer key for caching  
            string localizerKey = GetLocalizerKey(culture, typeName);

            //Cache and return 
            var resourceProvider = new EmbeddedFileProvider(resourceType.Assembly);
            var localizer = new EmbeddedJsonTextLocalizer(resourceProvider, lookupPaths);
            return localizers.GetOrAdd(localizerKey, localizer);
        }
    }
}

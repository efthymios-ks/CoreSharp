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
        private static string GetResourceName(Type resourceType)
        {
            resourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));

            //Get type name  
            var assemblyName = resourceType.Assembly.GetName().Name;
            var resourceName = resourceType.FullName;
            if (resourceName.StartsWith(assemblyName))
                resourceName = resourceName[(assemblyName.Length + 1)..];
            return resourceName;
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
            var resourceName = GetResourceName(resourceType);

            //Build localizer key for caching 
            string localizerKey = GetLocalizerKey(culture, resourceName);

            //Cache 
            if (!localizers.ContainsKey(localizerKey))
            {
                var resourceProvider = new EmbeddedFileProvider(resourceType.Assembly);
                var localizer = new EmbeddedJsonTextLocalizer(resourceProvider, culture, resourcesPath, resourceName);
                localizers.AddOrUpdate(localizerKey, localizer, (key, value) => localizer);
            }

            //Return 
            return localizers[localizerKey];
        }
    }
}

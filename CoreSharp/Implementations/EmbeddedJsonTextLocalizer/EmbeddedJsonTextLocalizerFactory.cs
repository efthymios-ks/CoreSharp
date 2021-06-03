using System;
using System.Collections.Concurrent;
using System.Globalization;
using CoreSharp.Interfaces.Localize;
using Microsoft.Extensions.FileProviders;

namespace CoreSharp.Implementations.TextLocalizer
{
    public class EmbeddedJsonTextLocalizerFactory : ITextLocalizerFactory
    {
        //Properties
        private readonly string resourcesPath;
        private readonly ConcurrentDictionary<string, ITextLocalizer> localizers = new ConcurrentDictionary<string, ITextLocalizer>();

        //Constructors
        public EmbeddedJsonTextLocalizerFactory(string resourcesPath)
        {
            this.resourcesPath = resourcesPath ?? string.Empty;
        }

        //Methods 
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

            //Build localizer key for caching  
            string localizerKey = GetLocalizerKey(culture, typeName);

            //Cache and return 
            var resourceProvider = new EmbeddedFileProvider(resourceType.Assembly);
            var localizer = new EmbeddedJsonTextLocalizer(culture, resourceProvider, resourcesPath, typeName);
            return localizers.GetOrAdd(localizerKey, localizer);
        }
    }
}

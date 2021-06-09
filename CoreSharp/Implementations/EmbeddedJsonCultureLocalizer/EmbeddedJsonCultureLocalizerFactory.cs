﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using CoreSharp.Interfaces.CultureLocalizer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;

namespace CoreSharp.Implementations.EmbeddedJsonCultureLocalizer
{
    public class EmbeddedJsonCultureLocalizerFactory : ICultureLocalizerFactory
    {
        //Properties
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string resourcesPath;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentDictionary<string, ICultureLocalizer> localizers = new();

        //Constructors
        public EmbeddedJsonCultureLocalizerFactory(string resourcesPath)
        {
            this.resourcesPath = resourcesPath ?? string.Empty;
        }

        //Methods 
        internal static string GetResourceName(Type resourceType)
        {
            resourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));

            //Get type name  
            var assemblyName = resourceType.Assembly.GetName().Name;
            var resourceName = resourceType.FullName;
            if (resourceName.StartsWith(assemblyName))
                resourceName = resourceName[(assemblyName.Length + 1)..];
            return resourceName;
        }

        private static string BuildResourcePath(string location, string baseName)
        {
            location ??= location;
            if (string.IsNullOrWhiteSpace(baseName))
                throw new ArgumentNullException(nameof(baseName));

            location = location.Replace('\\', '.').Replace('/', '.').Trim('.');
            baseName = baseName.Replace('\\', '.').Replace('/', '.').Trim('.');

            return $"{location}.{baseName}";
        }

        private static string GetLocalizerKey(CultureInfo culture, string name)
        {
            culture = culture ?? throw new ArgumentNullException(nameof(culture));

            return $"Culture={culture.TwoLetterISOLanguageName}, Name={name}";
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            if (string.IsNullOrWhiteSpace(nameof(baseName)))
                throw new ArgumentNullException(nameof(baseName));
            location ??= string.Empty;

            var givenResourceName = BuildResourcePath(location, baseName);
            var resourceType = Assembly
                                .GetEntryAssembly()?
                                .GetTypes()
                                .FirstOrDefault(t => GetResourceName(t) == givenResourceName);

            return Create(resourceType);
        }

        public IStringLocalizer Create(Type resourceSource) => Create(resourceSource, CultureInfo.CurrentUICulture);

        public ICultureLocalizer Create(Type resourceSource, CultureInfo culture)
        {
            resourceSource = resourceSource ?? throw new ArgumentNullException(nameof(resourceSource));
            culture = culture ?? throw new ArgumentNullException(nameof(culture));

            //Get type name  
            var resourceName = GetResourceName(resourceSource);

            //Build localizer key for caching 
            string localizerKey = GetLocalizerKey(culture, resourceName);

            //Cache 
            if (!localizers.ContainsKey(localizerKey))
            {
                var resourceProvider = new EmbeddedFileProvider(resourceSource.Assembly);
                var localizer = new EmbeddedJsonCultureLocalizer(resourceProvider, resourcesPath, resourceSource, culture);
                localizers.AddOrUpdate(localizerKey, localizer, (key, value) => localizer);
            }

            //Return 
            return localizers[localizerKey];
        }
    }
}

using System;
using CoreSharp.Interfaces.CultureLocalizer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreSharp.Implementations.EmbeddedJsonCultureLocalizer
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        /// Register ICultureLocalizerFactory/EmbeddedJsonCultureLocalizerFactory. 
        /// If culture not found, fallback file name has no culture tag. 
        /// Will look for files right next to `TResource` requester. 
        /// If not found, will look under `Resources`. 
        public static IServiceCollection AddEmbeddedJsonTextLocalizer(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddEmbeddedJsonTextLocalizer(string.Empty);
        }

        /// <summary>
        /// Register ICultureLocalizerFactory/EmbeddedJsonCultureLocalizerFactory. 
        /// If culture not found, fallback file name has no culture tag. 
        /// Will look for files right next to `TResource` requester. 
        /// If not found, will look under given `resourcesPath`. 
        /// If not found, will look under `Resources`. 
        public static IServiceCollection AddEmbeddedJsonTextLocalizer(this IServiceCollection serviceCollection, string resourcesPath)
        {
            serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            resourcesPath ??= string.Empty;

            var factory = new EmbeddedJsonCultureLocalizerFactory(resourcesPath);
            serviceCollection.TryAddSingleton<ICultureLocalizerFactory>(factory);
            serviceCollection.TryAddTransient(typeof(ICultureLocalizer<>), typeof(EmbeddedJsonCultureLocalizer<>));
            return serviceCollection;
        }
    }
}

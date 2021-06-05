using System;
using CoreSharp.Interfaces.Localize;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreSharp.Implementations.TextLocalizer
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        /// Register ITextLocalizerFactory/EmbeddedJsonTextLocalizerFactory. 
        /// If culture not found, fallback file name has no culture tag. 
        /// Will look for files right next to `TResource` requester. 
        /// If not found, will look under `Resources`. 
        public static IServiceCollection AddEmbeddedJsonTextLocalizer(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddEmbeddedJsonTextLocalizer(string.Empty);
        }

        /// <summary>
        /// Register ITextLocalizerFactory/EmbeddedJsonTextLocalizerFactory. 
        /// If culture not found, fallback file name has no culture tag. 
        /// Will look for files right next to `TResource` requester. 
        /// If not found, will look under given `resourcesPath`. 
        /// If not found, will look under `Resources`. 
        public static IServiceCollection AddEmbeddedJsonTextLocalizer(this IServiceCollection serviceCollection, string resourcesPath)
        {
            serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            resourcesPath ??= string.Empty;

            var factory = new EmbeddedJsonTextLocalizerFactory(resourcesPath);
            serviceCollection.TryAddSingleton<ITextLocalizerFactory>(factory);
            serviceCollection.TryAddTransient(typeof(ITextLocalizer<>), typeof(EmbeddedJsonTextLocalizer<>));
            return serviceCollection;
        }
    }
}

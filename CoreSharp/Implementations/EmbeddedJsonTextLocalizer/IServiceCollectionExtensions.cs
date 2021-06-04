using System;
using CoreSharp.Interfaces.Localize;
using Microsoft.Extensions.DependencyInjection;

namespace CoreSharp.Implementations.TextLocalizer
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        /// Register ITextLocalizerFactory/EmbeddedJsonTextLocalizerFactory. 
        /// Will look for files right next to each TResource requester. 
        /// If not found, will look under `Resources`.  
        public static IServiceCollection AddEmbeddedJsonTextLocalizer(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddEmbeddedJsonTextLocalizer(string.Empty);
        }

        /// <summary>
        /// Register ITextLocalizerFactory/EmbeddedJsonTextLocalizerFactory. 
        /// Will look for files right next to each TResource requester. 
        /// If not found, will look under given `resourcesPath`.   
        /// If not found, will look under `Resources`.   
        public static IServiceCollection AddEmbeddedJsonTextLocalizer(this IServiceCollection serviceCollection, string resourcesPath)
        {
            serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));

            var factory = new EmbeddedJsonTextLocalizerFactory(resourcesPath);
            return serviceCollection.AddSingleton<ITextLocalizerFactory>(factory);
        }
    }
}

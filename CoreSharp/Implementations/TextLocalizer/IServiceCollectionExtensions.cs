using System;
using CoreSharp.Interfaces.Localize;
using Microsoft.Extensions.DependencyInjection;

namespace CoreSharp.Implementations.TextLocalizer
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonTextLocalizer(this IServiceCollection serviceCollection, string resourcesPath)
        {
            serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));

            var factory = new JsonTextLocalizerFactory(resourcesPath);
            return serviceCollection.AddSingleton<ITextLocalizerFactory>(factory);
        }
    }
}

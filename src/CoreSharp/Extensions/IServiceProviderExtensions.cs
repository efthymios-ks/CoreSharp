using System;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="IServiceProvider"/> extensions.
    /// </summary>
    public static class IServiceProviderExtensions
    {
        /// <inheritdoc cref="IServiceProvider.GetService(Type)"/>
        public static TService GetService<TService>(this IServiceProvider serviceProvider)
            where TService : class
        {
            _ = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            return serviceProvider.GetService(typeof(TService)) as TService;
        }
    }
}

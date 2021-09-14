using System;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// IServiceProvider extensions.
    /// </summary>
    public static class IServiceProviderExtensions
    {
        /// <summary>
        /// Get service using generic casting directly.
        /// </summary>
        public static TService GetService<TService>(this IServiceProvider serviceProvider) where TService : class
        {
            _ = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            return serviceProvider.GetService(typeof(TService)) as TService;
        }
    }
}

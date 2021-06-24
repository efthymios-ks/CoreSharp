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
        public static T GetService<T>(this IServiceProvider serviceProvider) where T : class
        {
            _ = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            return serviceProvider.GetService(typeof(T)) as T;
        }
    }
}

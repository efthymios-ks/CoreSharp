using CoreSharp.DependencyInjection.ByInstaller.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace CoreSharp.DependencyInjection.ByInstaller.Extensions;

/// <summary>
/// <see cref="IServiceCollection"/> extensions.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <inheritdoc cref="AddInstallers(IServiceCollection, IConfiguration, Assembly[])"/>
    public static IServiceCollection AddInstallers(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
            => serviceCollection.AddInstallers(configuration, Assembly.GetEntryAssembly());

    /// <summary>
    /// Registers all <see cref="IServiceCollectionInstaller"/>.
    /// </summary>
    public static IServiceCollection AddInstallers(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(assemblies);

        var serviceInstallers = assemblies
            .SelectMany(assembly => assembly.DefinedTypes)
            .Where(typeInfo =>
            {
                // Not a class, ignore 
                if (!typeInfo.IsClass)
                {
                    return false;
                }

                // Not a concrete class, ignore 
                else if (typeInfo.IsAbstract)
                {
                    return false;
                }

                // Doesn't implement IServiceCollectionInstaller, ignore 
                else if (!typeInfo.GetInterfaces().Contains(typeof(IServiceCollectionInstaller)))
                {
                    return false;
                }

                // Else take
                else
                {
                    return true;
                }
            }).Select(Activator.CreateInstance)
              .Cast<IServiceCollectionInstaller>();

        foreach (var serviceInstaller in serviceInstallers)
        {
            serviceInstaller.Install(serviceCollection, configuration);
        }

        return serviceCollection;
    }
}

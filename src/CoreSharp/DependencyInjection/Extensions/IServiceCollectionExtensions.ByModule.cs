using CoreSharp.DependencyInjection.ServiceModules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace CoreSharp.DependencyInjection.Extensions;

/// <summary>
/// <see cref="IServiceCollection"/> extensions.
/// </summary>
public static partial class IServiceCollectionExtensions
{
    /// <inheritdoc cref="AddServiceModules(IServiceCollection, IConfiguration, Assembly[])"/>
    public static IServiceCollection AddServiceModules(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
            => serviceCollection.AddServiceModules(configuration, Assembly.GetEntryAssembly());

    /// <summary>
    /// Registers all <see cref="IServiceModule"/>.
    /// </summary>
    public static IServiceCollection AddServiceModules(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        _ = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
        _ = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _ = assemblies ?? throw new ArgumentNullException(nameof(assemblies));

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

                // Doesn't implement IServiceModule, ignore 
                else if (!typeInfo.GetInterfaces().Contains(typeof(IServiceModule)))
                {
                    return false;
                }

                // Else take
                else
                {
                    return true;
                }
            }).Select(Activator.CreateInstance)
              .Cast<IServiceModule>();

        foreach (var serviceInstaller in serviceInstallers)
        {
            serviceInstaller.Install(serviceCollection, configuration);
        }

        return serviceCollection;
    }
}

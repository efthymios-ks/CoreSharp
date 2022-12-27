using CoreSharp.DependencyInjection.Attributes;
using CoreSharp.DependencyInjection.Interfaces;
using CoreSharp.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreSharp.DependencyInjection.Extensions;

/// <summary>
/// <see cref="IServiceCollection"/> extensions.
/// </summary>
public static partial class IServiceCollectionExtensions
{
    /// <inheritdoc cref="AddMarkedServices(IServiceCollection, Assembly[])"/>>
    public static IServiceCollection AddMarkedServices(this IServiceCollection serviceCollecton)
       => serviceCollecton.AddMarkedServices(Assembly.GetEntryAssembly());

    /// <summary>
    /// <para>Register all services marked with one of the following:</para>
    /// <para>-<see cref="ITransient"/> / <see cref="ITransient{TContract}"/>.</para>
    /// <para>-<see cref="IScoped"/> / <see cref="IScoped{TContract}"/>.</para>
    /// <para>-<see cref="ISingleton"/> / <see cref="ISingleton{TContract}"/>.</para>
    /// <para>-Use <see cref="IgnoreServiceAttribute"/> on either contract or implementation to ignore.</para>
    /// </summary>
    public static IServiceCollection AddMarkedServices(this IServiceCollection serviceCollecton, params Assembly[] assemblies)
    {
        _ = serviceCollecton ?? throw new ArgumentNullException(nameof(serviceCollecton));
        _ = assemblies ?? throw new ArgumentNullException(nameof(assemblies));

        var implementations = assemblies.SelectMany(assembly => assembly.DefinedTypes).Where(typeInfo =>
        {
            // Not a class, ignore 
            if (!typeInfo.IsClass)
                return false;

            // Not a concrete class, ignore 
            else if (typeInfo.IsAbstract)
                return false;

            // Manual ignore, ignore 
            else if (typeInfo.GetCustomAttribute<IgnoreServiceAttribute>() is not null)
                return false;

            // Doesn't implement IService, ignore 
            else if (!typeInfo.GetInterfaces().Contains(typeof(IService)))
                return false;

            // Else take
            else
                return true;
        });

        foreach (var implementation in implementations)
        {
            var contract = GetMarkedServiceContract(implementation);
            var lifetime = GetMarkedServiceLifetime(implementation);

            // [IgnoreService]
            // interface IRepository { } 
            if (contract?.GetCustomAttribute<IgnoreServiceAttribute>() is not null)
                continue;

            // class Repository : IRepository, IScope<IRepository> { } 
            else if (contract is not null)
                contract = implementation.GetMarkedServiceActualContract(contract);

            // class Repository : IScoped { } 
            else
                contract = implementation;

            // Register
            var descriptor = new ServiceDescriptor(contract, implementation, lifetime);
            serviceCollecton.TryAdd(descriptor);
        }

        return serviceCollecton;
    }

    /// <summary>
    /// Extract contract from provided
    /// service implementing <see cref="IService"/>.
    /// </summary>
    private static Type GetMarkedServiceContract(Type service)
    {
        var interfaces = service.GetInterfaces();
        var genericService = Array.Find(interfaces, i => i.IsGenericType && i.IsAssignableTo(typeof(IService)));
        return genericService?.GetGenericArguments()[0];
    }

    /// <summary>
    /// Extract <see cref="ServiceLifetime"/> from provided
    /// service using <see cref="IService"/>.
    /// </summary>
    private static ServiceLifetime GetMarkedServiceLifetime(Type service)
    {
        var lifetimesDictionary = new (Type Type, ServiceLifetime Lifetime)[]
        {
            (typeof(ITransient),  ServiceLifetime.Transient),
            (typeof(IScoped), ServiceLifetime.Scoped),
            (typeof(ISingleton), ServiceLifetime.Singleton)
        };
        return Array.Find(lifetimesDictionary, t => service.IsAssignableTo(t.Type)).Lifetime;
    }

    /// <summary>
    /// Get the actual service contract/interface
    /// from a service implementation.
    /// Applies several generic-related checks and
    /// transformations.
    /// </summary>
    private static Type GetMarkedServiceActualContract(this Type serviceType, Type contractType)
    {
        static Type GetGenericTypeBase(Type type)
            => type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        var serviceInterfaces = serviceType.GetInterfaces();
        var actualContract = Array.Find(serviceInterfaces, i => GetGenericTypeBase(i) == GetGenericTypeBase(contractType));

        // When doesn't implement the configured service.
        if (actualContract is null)
            throw new InvalidOperationException($"Service ({serviceType}) does not inherit its configured contract interface ({contractType}).");

        // When it's open generic, get type definition.
        // E.g. class Repository<TValue> : IRepository<TValue>, IScoped<IRepository<TValue> {}
        if (actualContract.FullName is null && actualContract.IsGenericType)
            actualContract = actualContract.GetGenericTypeDefinition();

        // When different closed generic arguments.
        // E.g. class Repository : IRepository<int>, IScoped<IRepository<double>> {}
        if (actualContract.FullName is not null && contractType.FullName is not null && actualContract != contractType)
            throw new InvalidOperationException($"Service ({serviceType}) is configured with wrong generic argument(s).");

        return actualContract;
    }
}

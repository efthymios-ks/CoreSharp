using CoreSharp.DependencyInjection.Attributes;
using CoreSharp.DependencyInjection.ByInterface.Interfaces;
using CoreSharp.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreSharp.DependencyInjection.ByInterface.Extensions;

/// <summary>
/// <see cref="IServiceCollection"/> extensions.
/// </summary>
public static class IServiceCollectionExtensions
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
        ArgumentNullException.ThrowIfNull(serviceCollecton);
        ArgumentNullException.ThrowIfNull(assemblies);

        var implementations = assemblies
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

                // [IgnoreService]
                // class Repository : IScoped { } 
                else if (typeInfo.GetCustomAttribute<IgnoreServiceAttribute>() is not null)
                {
                    return false;
                }

                // Doesn't implement IService, ignore 
                else if (!typeInfo.GetInterfaces().Contains(typeof(IService)))
                {
                    return false;
                }

                // Else take
                else
                {
                    return true;
                }
            });

        foreach (var implementation in implementations)
        {
            var contract = GetServiceContract(implementation);
            var lifetime = GetServiceLifetime(implementation);

            // [IgnoreService]
            // interface IRepository { } 
            if (contract?.GetCustomAttribute<IgnoreServiceAttribute>() is not null)
            {
                continue;
            }

            // class Repository : IRepository, IScoped<IRepository> { } 
            else if (contract is not null)
            {
                contract = GetServiceInnerContract(contract, implementation);
            }

            // class Repository : IScoped { } 
            else
            {
                contract = implementation;
            }

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
    private static Type GetServiceContract(Type implementation)
    {
        var interfaces = implementation.GetInterfaces();
        var genericService = Array.Find(
            interfaces,
            i => i.IsGenericType && i.IsAssignableTo(typeof(IService)));
        return genericService?.GetGenericArguments()[0];
    }

    /// <summary>
    /// Extract <see cref="ServiceLifetime"/> from provided
    /// service using <see cref="IService"/>.
    /// </summary>
    private static ServiceLifetime GetServiceLifetime(Type implementation)
    {
        var lifetimesDictionary = new (Type Type, ServiceLifetime Lifetime)[]
        {
            (typeof(ITransient),  ServiceLifetime.Transient),
            (typeof(IScoped), ServiceLifetime.Scoped),
            (typeof(ISingleton), ServiceLifetime.Singleton)
        };
        return Array.Find(lifetimesDictionary, t => implementation.IsAssignableTo(t.Type)).Lifetime;
    }

    /// <summary>
    /// Get the actual service contract/interface
    /// from a service implementation.
    /// Applies several generic-related checks and
    /// transformations.
    /// </summary>
    private static Type GetServiceInnerContract(Type contract, Type implementation)
    {
        var innerContract = GetInnerContract(contract, implementation);

        // When it's open generic, get type definition.
        // E.g. class Repository<TValue> : IRepository<TValue>, IScoped<IRepository<TValue>> {}
        if (innerContract.FullName is null && innerContract.IsGenericType)
        {
            innerContract = innerContract.GetGenericTypeDefinition();
        }

        // When different closed generic arguments.
        // E.g. class Repository : IRepository<int>, IScoped<IRepository<double>> {}
        if (innerContract.FullName is not null && contract.FullName is not null && innerContract != contract)
        {
            throw new InvalidOperationException($"Service ({implementation}) is configured with wrong generic argument(s) ('{innerContract}' != '{contract}').");
        }

        return innerContract;

        static Type GetInnerContract(Type contract, Type implementation)
        {
            contract = GetGenericTypeBase(contract);
            var implementationInterfaces = implementation.GetInterfaces();
            var innerContract = Array.Find(
                implementationInterfaces,
                implementationInterface => GetGenericTypeBase(implementationInterface) == contract);

            return innerContract
                ?? throw new InvalidOperationException($"Service ({implementation}) does not inherit its configured contract interface ({contract}).");

            static Type GetGenericTypeBase(Type type)
                => type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        }
    }
}

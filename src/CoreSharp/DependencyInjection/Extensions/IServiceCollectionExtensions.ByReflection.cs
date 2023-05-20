using CoreSharp.DependencyInjection.Attributes;
using CoreSharp.DependencyInjection.Interfaces;
using CoreSharp.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CoreSharp.DependencyInjection.Extensions;

/// <summary>
/// <see cref="IServiceCollection"/> extensions.
/// </summary>
public static partial class IServiceCollectionExtensions
{
    // Fields
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const string InterfacePrefix = "I";
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const string InterfaceGroupRegexExp = "(?<Name>.+)";

    // Properties
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static string InterfaceContractRegexExp
        => $"^{InterfacePrefix}{InterfaceGroupRegexExp}$";

    // Methods 
    /// <inheritdoc cref="AddServices(IServiceCollection, Assembly[])"/>
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        => serviceCollection.AddServices(Assembly.GetEntryAssembly());

    /// <inheritdoc cref="AddServices(IServiceCollection, Type, Assembly[])"/>
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        _ = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
        _ = assemblies ?? throw new ArgumentNullException(nameof(assemblies));

        var interfaceNameRegex = new Regex(InterfaceGroupRegexExp);
        var contractsWithImplementations = GetUnmarkedContractImplementationsPairs(assemblies,
            contract => interfaceNameRegex.IsMatch(contract.Name));

        foreach (var (key, value) in contractsWithImplementations)
        {
            serviceCollection.TryAddScoped(key, value);
        }

        return serviceCollection;
    }

    /// <inheritdoc cref="AddServices(IServiceCollection, Type, Assembly[])"/>
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection, Type interfaceBaseType)
        => serviceCollection.AddServices(interfaceBaseType, Assembly.GetEntryAssembly());

    /// <summary>
    /// <para>Register all `interface contract` + `concrete implementation` combos found in given assemblies.</para>
    /// <para>-If single implementation is found, then it is registered regardless.</para>
    /// <para>-If multiple implementations are found, only the one with the `I{InterfaceName}` and `{InterfaceName}` convention is registered.</para>
    /// <para>-If multiple implementations are found and none has a proper name, none is registered.</para>
    /// <para>-Use <see cref="IgnoreServiceAttribute"/> on either contract or implementation to ignore.</para>
    /// </summary>
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection, Type interfaceBaseType, params Assembly[] assemblies)
    {
        _ = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
        _ = interfaceBaseType ?? throw new ArgumentNullException(nameof(interfaceBaseType));
        _ = assemblies ?? throw new ArgumentNullException(nameof(assemblies));

        interfaceBaseType = interfaceBaseType.GetGenericTypeBase();
        if (!interfaceBaseType.IsInterface)
        {
            throw new ArgumentException($"{nameof(interfaceBaseType)} ({interfaceBaseType.FullName}) must be an interface.", nameof(interfaceBaseType));
        }

        bool ImplementsBaseInterfaceDirectly(Type type)
            => type.GetDirectInterfaces()
                   .Select(t => t.GetGenericTypeBase())
                   .Any(t => t == interfaceBaseType);
        var contractsWithImplementations = GetUnmarkedContractImplementationsPairs(assemblies, ImplementsBaseInterfaceDirectly);

        foreach (var (key, value) in contractsWithImplementations)
        {
            serviceCollection.TryAddScoped(key, value);
        }

        return serviceCollection;
    }

    /// <summary>
    /// Returns a <see cref="IDictionary{TKey, TValue}"/>
    /// with all contracts and their implementation.
    /// </summary>
    private static IDictionary<Type, Type> GetUnmarkedContractImplementationsPairs(Assembly[] assemblies, Func<Type, bool> additionalInterfacePredicate = null)
    {
        additionalInterfacePredicate ??= _ => true;

        bool ContractFilter(TypeInfo typeInfo)
        {
            // Not an interface, ignore 
            if (!typeInfo.IsInterface)
            {
                return false;
            }

            // Manual ignore, ignore 
            else if (typeInfo.GetCustomAttribute<IgnoreServiceAttribute>() is not null)
            {
                return false;
            }

            // Additional checks do not apply, ignore 
            else if (!additionalInterfacePredicate(typeInfo))
            {
                return false;
            }

            // Take 
            return true;
        }

        var pairs = new Dictionary<Type, Type>();
        var contracts = assemblies.SelectMany(assembly => assembly.DefinedTypes)
                                  .Where(ContractFilter);
        foreach (var contract in contracts)
        {
            var implementation = GetUnmarkedContractImplementation(contract, assemblies);
            if (implementation is not null)
            {
                pairs.Add(contract, implementation);
            }
        }

        return pairs;
    }

    /// <summary>
    /// Find implementation <see cref="Type"/>
    /// for given contract <see cref="Type"/>.
    /// </summary>
    private static Type GetUnmarkedContractImplementation(Type contractType, Assembly[] assemblies)
    {
        _ = contractType ?? throw new ArgumentNullException(nameof(contractType));
        _ = assemblies ?? throw new ArgumentNullException(nameof(assemblies));

        // Get all implementations for given contract 
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

                // Manual ignore, ignore 
                else if (typeInfo.GetCustomAttribute<IgnoreServiceAttribute>() is not null)
                {
                    return false;
                }

                // Type.GetInterface(string) doesn't work with nested classes 
                var interfaces = typeInfo.GetInterfaces();

                // It's marked, ignore 
                if (interfaces.Contains(typeof(IService)))
                {
                    return false;
                }

                // Doesn't implement given interface, ignore 
                else if (!interfaces.Contains(contractType))
                {
                    return false;
                }

                // Take 
                return true;
            }).ToArray();

        // If single implementation, register it 
        if (implementations.Length == 1)
        {
            return implementations[0];
        }

        // If multiple implementations
        else if (implementations.Length > 1)
        {
            static string TrimGenericTypeBaseName(string name)
            {
                var backtickIndex = name.IndexOf('`');
                if (backtickIndex > 0)
                {
                    name = name[..backtickIndex];
                }

                return name;
            }

            static string GetGenericTypeBaseName(Type type)
                => TrimGenericTypeBaseName(type.Name);

            // Get contract name 
            var trimmedContractName = Regex.Match(contractType.Name, InterfaceContractRegexExp)
                                           .Groups["Name"]
                                           .Value;
            trimmedContractName = TrimGenericTypeBaseName(trimmedContractName);

            // First one with the correct name convention
            return Array.Find(implementations, i => GetGenericTypeBaseName(i) == trimmedContractName);
        }

        // None found
        return null;
    }
}

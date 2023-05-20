using CoreSharp.DependencyInjection.Attributes;
using CoreSharp.DependencyInjection.ByInterface.Interfaces;
using CoreSharp.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CoreSharp.DependencyInjection.ByReflection.Extensions;

/// <summary>
/// <see cref="IServiceCollection"/> extensions.
/// </summary>
public static class IServiceCollectionExtensions
{
    // Fields
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const string ContractPrefix = "I";
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const string ContractRegexExpressionNameGroup = "(?<Name>.+)";

    // Properties
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static string ContractRegexExpression
        => $"^{ContractPrefix}{ContractRegexExpressionNameGroup}$";

    // Methods 
    /// <inheritdoc cref="AddServices(IServiceCollection, Assembly[])"/>
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        => serviceCollection.AddServices(Assembly.GetEntryAssembly());

    /// <inheritdoc cref="AddServicesInner(IServiceCollection, Func{Type, bool}, Assembly[])"/>
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        var contractNameRegex = new Regex(ContractRegexExpressionNameGroup, RegexOptions.Compiled);

        return serviceCollection.AddServicesInner(MatchesContractName, assemblies);

        bool MatchesContractName(Type contract)
            => contractNameRegex.IsMatch(contract.Name);
    }

    /// <inheritdoc cref="AddServices(IServiceCollection, Type, Assembly[])"/>
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection, Type contractType)
        => serviceCollection.AddServices(contractType, Assembly.GetEntryAssembly());

    /// <inheritdoc cref="AddServicesInner(IServiceCollection, Func{Type, bool}, Assembly[])"/>
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection, Type contractType, params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(contractType);

        contractType = contractType.GetGenericTypeBase();
        // Y class Repository : IRepository 
        // X class Repository : RepositoryBase 
        if (!contractType.IsInterface)
        {
            throw new ArgumentException($"{nameof(contractType)} ({contractType.FullName}) must be an interface.", nameof(contractType));
        }

        return serviceCollection.AddServicesInner(InheritsTargetContractDirectly, assemblies);

        bool InheritsTargetContractDirectly(Type contract)
            => contract.GetDirectInterfaces()
                       .Any(contractInterface => contractInterface.GetGenericTypeBase() == contractType);
    }

    /// <summary>
    /// <para>Register all `interface contract` + `concrete implementation` combos found in given assemblies.</para>
    /// <para>-If single implementation is found, then it is registered regardless.</para>
    /// <para>-If multiple implementations are found, only the one with the `I{ServiceName}` and `{ServiceName}` convention is registered.</para>
    /// <para>-If multiple implementations are found and none has a proper name, none is registered.</para>
    /// <para>-Use <see cref="IgnoreServiceAttribute"/> on either contract or implementation to ignore.</para>
    /// </summary>
    private static IServiceCollection AddServicesInner(this IServiceCollection serviceCollection, Func<Type, bool> contractPredicate, params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        ArgumentNullException.ThrowIfNull(contractPredicate);
        ArgumentNullException.ThrowIfNull(assemblies);

        var contractsWithImplementations = GetContractImplementationsPairs(assemblies, contractPredicate);

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
    private static IDictionary<Type, Type> GetContractImplementationsPairs(Assembly[] assemblies, Func<Type, bool> contractPredicate)
    {
        var pairs = new Dictionary<Type, Type>();
        var contracts = assemblies.SelectMany(assembly => assembly.DefinedTypes)
                                  .Where(ContractFilter);
        foreach (var contract in contracts)
        {
            var implementation = GetServiceImplementation(contract, assemblies);
            if (implementation is not null)
            {
                pairs.Add(contract, implementation);
            }
        }

        return pairs;

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
            else if (!contractPredicate(typeInfo))
            {
                return false;
            }

            // Take 
            return true;
        }
    }

    /// <summary>
    /// Find implementation <see cref="Type"/>
    /// for given contract <see cref="Type"/>.
    /// </summary>
    private static Type GetServiceImplementation(Type contract, Assembly[] assemblies)
    {
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
                else if (!interfaces.Contains(contract))
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
            // Get contract name 
            var trimmedContractName = Regex.Match(contract.Name, ContractRegexExpression)
                                           .Groups["Name"]
                                           .Value;
            trimmedContractName = TrimGenericTypeBaseName(trimmedContractName);

            // First one with the correct name convention
            return Array.Find(implementations, i => GetGenericTypeBaseName(i) == trimmedContractName);

            static string GetGenericTypeBaseName(Type type)
                => TrimGenericTypeBaseName(type.Name);

            static string TrimGenericTypeBaseName(string name)
            {
                var backtickIndex = name.IndexOf('`');
                if (backtickIndex > 0)
                {
                    name = name[..backtickIndex];
                }

                return name;
            }
        }

        // None found
        return null;
    }
}

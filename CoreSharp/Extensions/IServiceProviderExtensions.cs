using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="IServiceProvider"/> extensions.
    /// </summary>
    public static class IServiceProviderExtensions
    {
        //Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const string InterfacePrefix = "I";
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const string InterfaceGroupRegexExp = "(?<Name>.+)";

        //Properties
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static string InterfaceContractRegexExp
            => $"^{InterfacePrefix}{InterfaceGroupRegexExp}$";

        /// <inheritdoc cref="IServiceProvider.GetService(Type)"/>
        public static TService GetService<TService>(this IServiceProvider serviceProvider)
            where TService : class
        {
            _ = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            return serviceProvider.GetService(typeof(TService)) as TService;
        }

        /// <inheritdoc cref="AddInterfaces(IServiceCollection, Assembly[])"/>
        public static IServiceCollection AddInterfaces(this IServiceCollection serviceCollection)
            => serviceCollection.AddInterfaces(Assembly.GetEntryAssembly());

        /// <inheritdoc cref="AddInterfaces(IServiceCollection, Type, Assembly[])"/>
        public static IServiceCollection AddInterfaces(this IServiceCollection serviceCollection, params Assembly[] assemblies)
        {
            _ = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            _ = assemblies ?? throw new ArgumentNullException(nameof(assemblies));

            //Get all contracts 
            var interfaceNameRegex = new Regex(InterfaceGroupRegexExp);
            var contracts = GetNonRegisteredContracts(serviceCollection, assemblies)
                                .Where(type => interfaceNameRegex.IsMatch(type.Name));

            //Find the proper implementation for each contract 
            foreach (var contract in contracts)
            {
                if (GetContractImplementation(contract, assemblies) is Type implementation)
                    serviceCollection.AddScoped(contract, implementation);
            }

            return serviceCollection;
        }

        /// <inheritdoc cref="AddInterfaces(IServiceCollection, Type, Assembly[])"/>
        public static IServiceCollection AddInterfaces(this IServiceCollection serviceCollection, Type interfaceBaseType)
            => serviceCollection.AddInterfaces(interfaceBaseType, Assembly.GetEntryAssembly());

        /// <summary>
        /// <para>Register all `interface contract` + `concrete implementation` combos found in given assemblies.</para>
        /// <para>-If single implementation is found, then it is registered regardless.</para>
        /// <para>-If multiple implementations are found, only the one with the `I{InterfaceName}` and `{InterfaceName}` convention is registered.</para>
        /// <para>-If multiple implementations are found and none has a proper name, then an error is thrown.</para>
        /// </summary>
        /// <exception cref="AmbiguousMatchException"/>
        public static IServiceCollection AddInterfaces(this IServiceCollection serviceCollection, Type interfaceBaseType, params Assembly[] assemblies)
        {
            _ = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            _ = interfaceBaseType ?? throw new ArgumentNullException(nameof(interfaceBaseType));
            _ = assemblies ?? throw new ArgumentNullException(nameof(assemblies));

            if (!interfaceBaseType.IsInterface)
                throw new ArgumentException($"{nameof(interfaceBaseType)} ({interfaceBaseType.FullName}) must be an interface.", nameof(interfaceBaseType));

            //Get all contracts 
            var contracts = GetNonRegisteredContracts(serviceCollection, assemblies).Where(type =>
            {
                //Doesn't implement base interface, ignore
                if (type.GetInterface(interfaceBaseType.FullName) is null)
                    return false;

                //Found interface is not inherited directly
                else if (!type.GetDirectInterfaces().Any(directInterface => directInterface.GetGenericTypeBase() == interfaceBaseType.GetGenericTypeBase()))
                    return false;

                //Else take 
                else
                    return true;
            });

            //Find the proper implementation for each contract 
            foreach (var contract in contracts)
            {
                if (GetContractImplementation(contract, assemblies) is Type implementation)
                    serviceCollection.TryAddScoped(contract, implementation);
            }

            return serviceCollection;
        }

        private static IEnumerable<Type> GetNonRegisteredContracts(IServiceCollection serviceCollection, Assembly[] assemblies)
        {
            _ = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            _ = assemblies ?? throw new ArgumentNullException(nameof(assemblies));

            //Get all available contracts 
            return assemblies.SelectMany(assembly => assembly.GetTypes()).Where(type =>
            {
                //Already registered, ignore 
                if (serviceCollection.Any(service => service.ServiceType == type))
                    return false;

                //Not an interface, ignore 
                else if (!type.IsInterface)
                    return false;

                //Else take  
                return true;
            });
        }

        private static Type GetContractImplementation(Type contractType, Assembly[] assemblies)
        {
            _ = contractType ?? throw new ArgumentNullException(nameof(contractType));
            _ = assemblies ?? throw new ArgumentNullException(nameof(assemblies));

            //Get all implementations for given contract 
            var implementations = assemblies.SelectMany(a => a.GetTypes()).Where(t =>
            {
                //Doesn't implement given interface, ignore 
                if (t.GetInterface(contractType.FullName) is null)
                    return false;

                //Not a class, ignore 
                else if (!t.IsClass)
                    return false;

                //Not a concrete class, ignore 
                else if (t.IsAbstract)
                    return false;

                //Else take
                else
                    return true;
            }).ToArray();

            //If single implementation, register it 
            if (implementations.Length == 1)
            {
                return implementations[0];
            }

            //If multiple implementations
            else if (implementations.Length > 1)
            {
                static string TrimGenericTypeBaseName(string name)
                {
                    _ = name ?? throw new ArgumentNullException(nameof(name));

                    if (name.Contains('`'))
                        name = name[..name.LastIndexOf('`')];

                    return name;
                }

                static string GetGenericTypeBaseName(Type type)
                    => TrimGenericTypeBaseName(type?.Name);

                //Get contract name 
                var trimmedContractName = Regex.Match(contractType.Name, InterfaceContractRegexExp).Groups["Name"].Value;
                trimmedContractName = TrimGenericTypeBaseName(trimmedContractName);

                //Register only if there is a single one with the correct name convention
                var sameNameImplementation = Array.Find(implementations, i => GetGenericTypeBaseName(i) == trimmedContractName);
                if (sameNameImplementation is null)
                    throw new AmbiguousMatchException($"Multiple instances of `{contractType.Name}` have been found, but couldn't match any by name `{trimmedContractName}`.");

                return sameNameImplementation;
            }

            return null;
        }
    }
}

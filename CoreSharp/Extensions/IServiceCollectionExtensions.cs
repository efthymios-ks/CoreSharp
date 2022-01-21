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
    /// <see cref="IServiceCollection"/> extensions.
    /// </summary>
    public static class IServiceCollectionExtensions
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
            var unregisteredContracts = GetUnregisteredContracts(assemblies, serviceCollection);
            var contracts = unregisteredContracts.Where(type => interfaceNameRegex.IsMatch(type.Name));

            //Find the proper implementation for each contract 
            foreach (var contract in contracts)
            {
                if (GetContractImplementation(contract, assemblies) is Type implementation)
                    serviceCollection.TryAddScoped(contract, implementation);
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
        /// <para>-If multiple implementations are found and none has a proper name, none is registered.</para>
        /// </summary>
        public static IServiceCollection AddInterfaces(this IServiceCollection serviceCollection, Type interfaceBaseType, params Assembly[] assemblies)
        {
            _ = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            _ = interfaceBaseType ?? throw new ArgumentNullException(nameof(interfaceBaseType));
            _ = assemblies ?? throw new ArgumentNullException(nameof(assemblies));

            interfaceBaseType = interfaceBaseType.GetGenericTypeBase();
            if (!interfaceBaseType.IsInterface)
                throw new ArgumentException($"{nameof(interfaceBaseType)} ({interfaceBaseType.FullName}) must be an interface.", nameof(interfaceBaseType));

            //Get all contracts 
            bool ImplementsBaseInterfaceDirectly(Type type)
                => type.GetDirectInterfaces()
                       .Select(t => t.GetGenericTypeBase())
                       .Any(t => t == interfaceBaseType);
            var unregisteredContracts = GetUnregisteredContracts(assemblies, serviceCollection);
            var contracts = unregisteredContracts.Where(ImplementsBaseInterfaceDirectly);

            //Find the proper implementation for each contract 
            foreach (var contract in contracts)
            {
                if (GetContractImplementation(contract, assemblies) is Type implementation)
                    serviceCollection.TryAddScoped(contract, implementation);
            }

            return serviceCollection;
        }

        private static IEnumerable<Type> GetUnregisteredContracts(Assembly[] assemblies, IServiceCollection serviceCollection)
        {
            _ = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
            _ = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));

            //Get all available contracts 
            return assemblies.SelectMany(assembly => assembly.GetTypes()).Where(type =>
            {
                //Not an interface, ignore 
                if (!type.IsInterface)
                    return false;

                //Already registered, ignore 
                else if (serviceCollection.Any(service => service.ServiceType == type))
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
                    if (name.Contains('`'))
                        name = name[..name.LastIndexOf('`')];
                    return name;
                }

                static string GetGenericTypeBaseName(Type type)
                    => TrimGenericTypeBaseName(type?.Name);

                //Get contract name 
                var trimmedContractName = Regex.Match(contractType.Name, InterfaceContractRegexExp).Groups["Name"].Value;
                trimmedContractName = TrimGenericTypeBaseName(trimmedContractName);

                //First one with the correct name convention
                return Array.Find(implementations, i => GetGenericTypeBaseName(i) == trimmedContractName);
            }

            return null;
        }
    }
}

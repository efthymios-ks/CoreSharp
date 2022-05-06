using CoreSharp.DependencyInjection.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
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

        //Methods 
        /// <summary>
        /// Registers and attempts to bind a particular type of <see cref="IOptions{TOptions}"/>.
        /// </summary>
        public static IServiceCollection ConfigureBind<TOptions>(this IServiceCollection services, IConfigurationSection section)
            where TOptions : class
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = section ?? throw new ArgumentNullException(nameof(section));

            return services.Configure<TOptions>(options => section.Bind(options));
        }

        #region Unmarked services
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
                serviceCollection.TryAddScoped(key, value);

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
        /// </summary>
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection, Type interfaceBaseType, params Assembly[] assemblies)
        {
            _ = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            _ = interfaceBaseType ?? throw new ArgumentNullException(nameof(interfaceBaseType));
            _ = assemblies ?? throw new ArgumentNullException(nameof(assemblies));

            interfaceBaseType = interfaceBaseType.GetGenericTypeBase();
            if (!interfaceBaseType.IsInterface)
                throw new ArgumentException($"{nameof(interfaceBaseType)} ({interfaceBaseType.FullName}) must be an interface.", nameof(interfaceBaseType));

            bool ImplementsBaseInterfaceDirectly(Type type)
                => type.GetDirectInterfaces()
                       .Select(t => t.GetGenericTypeBase())
                       .Any(t => t == interfaceBaseType);
            var contractsWithImplementations = GetUnmarkedContractImplementationsPairs(assemblies, ImplementsBaseInterfaceDirectly);

            foreach (var (key, value) in contractsWithImplementations)
                serviceCollection.TryAddScoped(key, value);

            return serviceCollection;
        }

        /// <summary>
        /// Returns a <see cref="IDictionary{TKey, TValue}"/>
        /// with all contracts and their implementation.
        /// </summary>
        private static IDictionary<Type, Type> GetUnmarkedContractImplementationsPairs(Assembly[] assemblies, Func<Type, bool> additionalInterfacePredicate = null)
        {
            additionalInterfacePredicate ??= _ => true;

            bool ContractFilter(Type type)
            {
                //Not an interface, ignore
                if (!type.IsInterface)
                    return false;

                //Additional checks do not apply, ignore 
                else if (!additionalInterfacePredicate(type))
                    return false;

                //Take 
                return true;
            }

            var pairs = new Dictionary<Type, Type>();
            var contracts = assemblies.SelectMany(assembly => assembly.GetTypes())
                                      .Where(ContractFilter);
            foreach (var contract in contracts)
            {
                var implementation = GetUnmarkedContractImplementation(contract, assemblies);
                if (implementation is not null)
                    pairs.Add(contract, implementation);
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

            //Get all implementations for given contract 
            var implementations = assemblies.SelectMany(a => a.GetTypes()).Where(t =>
            {
                //Not a class, ignore 
                if (!t.IsClass)
                    return false;

                //Not a concrete class, ignore 
                else if (t.IsAbstract)
                    return false;

                //Type.GetInterface(string) doesn't work with nested classes 
                var interfaces = t.GetInterfaces();

                //It's marked, ignore 
                if (interfaces.Contains(typeof(IService)))
                    return false;

                //Doesn't implement given interface, ignore 
                else if (!interfaces.Contains(contractType))
                    return false;

                //Take 
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
                    var backtickIndex = name.IndexOf('`');
                    if (backtickIndex > 0)
                        name = name[..backtickIndex];
                    return name;
                }

                static string GetGenericTypeBaseName(Type type)
                    => TrimGenericTypeBaseName(type.Name);

                //Get contract name 
                var trimmedContractName = Regex.Match(contractType.Name, InterfaceContractRegexExp)
                                               .Groups["Name"]
                                               .Value;
                trimmedContractName = TrimGenericTypeBaseName(trimmedContractName);

                //First one with the correct name convention
                return Array.Find(implementations, i => GetGenericTypeBaseName(i) == trimmedContractName);
            }

            //None found
            return null;
        }
        #endregion

        #region Marked services
        /// <inheritdoc cref="AddMarkedServices(IServiceCollection, Assembly[])"/>>
        public static IServiceCollection AddMarkedServices(this IServiceCollection serviceCollecton)
           => serviceCollecton.AddMarkedServices(Assembly.GetEntryAssembly());

        /// <summary>
        /// <para>Register all services marked with one of the following:</para>
        /// <para>-<see cref="ITransient"/> / <see cref="ITransient{TContract}"/>.</para>
        /// <para>-<see cref="IScoped"/> / <see cref="IScoped{TContract}"/>.</para>
        /// <para>-<see cref="ISingleton"/> / <see cref="ISingleton{TContract}"/>.</para>
        /// </summary>
        public static IServiceCollection AddMarkedServices(this IServiceCollection serviceCollecton, params Assembly[] assemblies)
        {
            _ = serviceCollecton ?? throw new ArgumentNullException(nameof(serviceCollecton));
            _ = assemblies ?? throw new ArgumentNullException(nameof(assemblies));

            var implementations = assemblies.SelectMany(assembly => assembly.GetTypes()).Where(t =>
            {
                //Not a class, ignore 
                if (!t.IsClass)
                    return false;

                //Not a concrete class, ignore 
                else if (t.IsAbstract)
                    return false;

                //Doesn't implement IService 
                else if (t.GetInterface(typeof(IService).FullName) is null)
                    return false;

                //Else take
                else
                    return true;
            });

            foreach (var implementation in implementations)
            {
                var contract = GetMarkedServiceContract(implementation);
                var lifetime = GetMarkedServiceLifetime(implementation);

                // class Repository : IRepository, IScope<IRepository> 
                if (contract is not null)
                    contract = GetMarkedServiceActualContract(implementation, contract);

                // class Repository : IScoped 
                else
                    contract = implementation;

                //Register
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
        #endregion
    }
}

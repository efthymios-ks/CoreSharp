using Microsoft.Extensions.DependencyInjection;
using System;
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
        private const string ServiceContractPrefix = "I";
        private const string ServiceSuffix = "Service";
        private const string RepositorySuffix = "Repository";
        private const string ServiceNameGroupRegex = "(?<Name>.+)";

        //Methods
        private static string BuildServiceContractRegex(string prefix, string suffix) => $"^{prefix}{ServiceNameGroupRegex}{suffix}$";

        private static IServiceCollection RegisterAllInternal(this IServiceCollection serviceCollection, Assembly assembly, string suffix, Type baseType)
        {
            //Validate arguments 
            _ = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            _ = assembly ?? throw new ArgumentNullException(nameof(assembly));
            if (baseType?.IsInterface == false)
                throw new ArgumentException($"{nameof(baseType)} ({baseType.FullName}) must be an interface.", nameof(baseType));

            //Get all contracts
            var contractRegex = BuildServiceContractRegex(ServiceContractPrefix, suffix);
            var contracts = assembly.GetTypes().Where(t =>
            {
                //Already registered, ignore 
                if (serviceCollection.Any(s => s.ServiceType == t))
                    return false;
                //Name convention doesn't match, ignore 
                else if (!Regex.IsMatch(t.Name, contractRegex))
                    return false;
                //Not public, ignore 
                else if (!t.IsPublic)
                    return false;
                //Not an interface, ignore 
                else if (!t.IsInterface)
                    return false;
                //Doesn't implement base interface, ignore
                else if (baseType is not null && t.GetInterface(baseType.FullName!) is null)
                    return false;
                //Else take 
                else
                    return true;
            });

            //Find the proper implementation for each contract 
            foreach (var contract in contracts)
            {
                //Get all implementations for given contract 
                var implementations = assembly.GetTypes().Where(t =>
                {
                    //Doesn't implement given interface, ignore 
                    if (t.GetInterface(contract.FullName!) is null)
                        return false;
                    //Not public, ignore  
                    else if (!t.IsPublic)
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
                var implementationsCount = implementations.Length;

                //If single implementation, register it 
                if (implementationsCount == 1)
                {
                    serviceCollection.AddScoped(contract, implementations[0]);
                }

                //If multiple implementations
                else if (implementationsCount > 1)
                {
                    static string GetGenericTypeBaseName(string genericName) =>
                        genericName[..genericName.LastIndexOf('`')];
                    static string TrimGenericTypeName(Type genericType, string name = null)
                    {
                        _ = genericType ?? throw new ArgumentNullException(nameof(genericType));
                        name ??= genericType.Name;

                        if (genericType.IsGenericType)
                            name = GetGenericTypeBaseName(name);

                        return name;
                    }

                    //Get contract name 
                    var contractName = Regex.Match(contract.Name, contractRegex).Groups["Name"].Value;
                    contractName = TrimGenericTypeName(contract, contractName);

                    //Build target name 
                    var targetImplementationName = $"{contractName}{suffix}";

                    //Register only if there is a single one with the correct name convention
                    var sameNameImplementation = Array.Find(implementations, i => TrimGenericTypeName(i) == targetImplementationName);
                    if (sameNameImplementation is not null)
                        serviceCollection.AddScoped(contract, sameNameImplementation);
                }
            }

            return serviceCollection;
        }

        /// <inheritdoc cref="RegisterAll(IServiceCollection, Assembly, string)"/>
        public static IServiceCollection RegisterAll(this IServiceCollection serviceCollection)
            => serviceCollection.RegisterAll(Assembly.GetExecutingAssembly(), string.Empty);

        /// <inheritdoc cref="RegisterAll(IServiceCollection, Assembly, string)"/>
        public static IServiceCollection RegisterAll(this IServiceCollection serviceCollection, Assembly assembly)
           => serviceCollection.RegisterAll(assembly, string.Empty);

        /// <inheritdoc cref="RegisterAll(IServiceCollection, Assembly, string)"/>
        public static IServiceCollection RegisterAll(this IServiceCollection serviceCollection, string suffix)
           => serviceCollection.RegisterAll(Assembly.GetExecutingAssembly(), suffix);

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in given assembly.
        /// If single implementation is found, then it is registered regardless.
        /// If multiple implementations are found, only the one with the `I{Name}{Suffix}` and `{Name}{Suffix}` convention is registered.
        /// If multiple implementations are found and none has a proper name, then none is registered.
        /// </summary>
        public static IServiceCollection RegisterAll(this IServiceCollection serviceCollection, Assembly assembly, string suffix)
           => serviceCollection.RegisterAllInternal(assembly, suffix, null);

        /// <inheritdoc cref="RegisterAllServices(IServiceCollection, Assembly)"/>
        public static IServiceCollection RegisterAllServices(this IServiceCollection serviceCollection)
            => serviceCollection.RegisterAll(Assembly.GetExecutingAssembly(), ServiceSuffix);

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in given assembly.
        /// If single implementation is found, then it is registered regardless.
        /// If multiple implementations are found, only the one with the `I{Name}Service` and `{Name}Service` convention is registered.
        /// If multiple implementations are found and none has a proper name, then none is registered.
        /// </summary>
        public static IServiceCollection RegisterAllServices(this IServiceCollection serviceCollection, Assembly assembly)
            => serviceCollection.RegisterAll(assembly, ServiceSuffix);

        /// <inheritdoc cref="RegisterAllRepositories(IServiceCollection, Assembly)"/>
        public static IServiceCollection RegisterAllRepositories(this IServiceCollection serviceCollection)
            => serviceCollection.RegisterAll(Assembly.GetExecutingAssembly(), RepositorySuffix);

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in given assembly.
        /// If single implementation is found, then it is registered regardless.
        /// If multiple implementations are found, only the one with the `I{Name}Repository` and `{Name}Repository` convention is registered.
        /// If multiple implementations are found and none has a proper name, then none is registered.
        /// </summary>
        public static IServiceCollection RegisterAllRepositories(this IServiceCollection serviceCollection, Assembly assembly)
            => serviceCollection.RegisterAll(assembly, RepositorySuffix);

        /// <inheritdoc cref="RegisterAll{TBase}(IServiceCollection, Assembly, string)"/>
        public static IServiceCollection RegisterAll<TBase>(this IServiceCollection serviceCollection) where TBase : class
            => serviceCollection.RegisterAll<TBase>(Assembly.GetExecutingAssembly());

        /// <inheritdoc cref="RegisterAll{TBase}(IServiceCollection, Assembly, string)"/>
        public static IServiceCollection RegisterAll<TBase>(this IServiceCollection serviceCollection, Assembly assembly) where TBase : class
            => serviceCollection.RegisterAll<TBase>(assembly, string.Empty);

        /// <inheritdoc cref="RegisterAll{TBase}(IServiceCollection, Assembly, string)"/>
        public static IServiceCollection RegisterAll<TBase>(this IServiceCollection serviceCollection, string suffix) where TBase : class
            => serviceCollection.RegisterAll<TBase>(Assembly.GetExecutingAssembly(), suffix);

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in given assembly based on given class.
        /// If single implementation is found, then it is registered regardless.
        /// If multiple implementations are found, only the one with the `I{Name}{Suffix}` and `{Name}{Suffix}` convention is registered.
        /// If multiple implementations are found and none has a proper name, then none is registered.
        /// </summary>
        public static IServiceCollection RegisterAll<TBase>(this IServiceCollection serviceCollection, Assembly assembly, string suffix) where TBase : class
             => serviceCollection.RegisterAllInternal(assembly, suffix, typeof(TBase));
    }
}

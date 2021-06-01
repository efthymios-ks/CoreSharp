using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// IServiceCollection extensions. 
    /// </summary>
    public static partial class IServiceCollectionExtensions
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
            serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            if (baseType != null)
                if (!baseType.IsInterface)
                    throw new ArgumentException($"{nameof(baseType)} ({baseType.FullName}) must be an interface.", nameof(baseType));

            //Get all contracts
            string contractRegex = BuildServiceContractRegex(ServiceContractPrefix, suffix);
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
                else if (baseType != null && t.GetInterface(baseType.FullName) == null)
                    return false;
                //Else take 
                else
                    return true;
            });

            //Find the proper implementation for each contract 
            foreach (var contract in contracts)
            {
                //Get all implentations for given contract 
                var implementations = assembly.GetTypes().Where(t =>
                {
                    //Doesn't implement given interface, ignore 
                    if (t.GetInterface(contract.FullName) == null)
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
                });
                var implementationsCount = implementations.Count();

                //If single implementation, register it 
                if (implementationsCount == 1)
                    serviceCollection.AddScoped(contract, implementations.First());

                //If multiple implementations
                else if (implementationsCount > 1)
                {
                    static string GetGenericTypeBaseName(string genericName) => genericName.Substring(0, genericName.LastIndexOf('`'));
                    static string TrimGenericTypeName(Type genericType, string name = null)
                    {
                        genericType = genericType ?? throw new ArgumentNullException(nameof(genericType));
                        name ??= genericType.Name;

                        if (genericType.IsGenericType)
                            name = GetGenericTypeBaseName(name);

                        return name;
                    };

                    //Get contract name 
                    var contractName = Regex.Match(contract.Name, contractRegex).Groups["Name"].Value;
                    contractName = TrimGenericTypeName(contract, contractName);

                    //Build target name 
                    var targetImplementationName = $"{contractName}{suffix}";

                    //Register only if there is a single one with the correct name convention
                    var sameNameImplementation = implementations.FirstOrDefault(i => TrimGenericTypeName(i) == targetImplementationName);
                    if (sameNameImplementation != null)
                        serviceCollection.AddScoped(contract, sameNameImplementation);
                }
            }

            return serviceCollection;
        }

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in currently executing assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}` and `{Name}` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAll(this IServiceCollection serviceCollection)
        {
            return serviceCollection.RegisterAll(Assembly.GetExecutingAssembly(), string.Empty);
        }

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in given assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}` and `{Name}` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAll(this IServiceCollection serviceCollection, Assembly assembly)
        {
            return serviceCollection.RegisterAll(assembly, string.Empty);
        }

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in currently executing assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}{Suffix}` and `{Name}{Suffix}` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAll(this IServiceCollection serviceCollection, string suffix)
        {
            return serviceCollection.RegisterAll(Assembly.GetExecutingAssembly(), suffix);
        }

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in given assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}{Suffix}` and `{Name}{Suffix}` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAll(this IServiceCollection serviceCollection, Assembly assembly, string suffix)
        {
            return serviceCollection.RegisterAllInternal(assembly, suffix, null);
        }

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in currently executing assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}Service` and `{Name}Service` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAllServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.RegisterAll(Assembly.GetExecutingAssembly(), ServiceSuffix);
        }

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in given assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}Service` and `{Name}Service` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAllServices(this IServiceCollection serviceCollection, Assembly assembly)
        {
            return serviceCollection.RegisterAll(assembly, ServiceSuffix);
        }

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in currently executing assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}Repository` and `{Name}Repository` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAllRepositories(this IServiceCollection serviceCollection)
        {
            return serviceCollection.RegisterAll(Assembly.GetExecutingAssembly(), RepositorySuffix);
        }

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in given assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}Repository` and `{Name}Repository` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAllRepositories(this IServiceCollection serviceCollection, Assembly assembly)
        {
            return serviceCollection.RegisterAll(assembly, RepositorySuffix);
        }

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in currently executing assembly based on given class.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}` and `{Name}` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAll<TBase>(this IServiceCollection serviceCollection) where TBase : class
        {
            return serviceCollection.RegisterAll<TBase>(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in given assembly based on given class.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}` and `{Name}` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAll<TBase>(this IServiceCollection serviceCollection, Assembly assembly) where TBase : class
        {
            return serviceCollection.RegisterAll<TBase>(assembly, string.Empty);
        }

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in currently executing assembly based on given class.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}{Suffix}` and `{Name}{Suffix}` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAll<TBase>(this IServiceCollection serviceCollection, string suffix) where TBase : class
        {
            return serviceCollection.RegisterAll<TBase>(Assembly.GetExecutingAssembly(), suffix);
        }

        /// <summary>
        /// Register all `Interface Contract` + `Concrete Implementation` combos found in given assembly based on given class.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}{Suffix}` and `{Name}{Suffix}` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAll<TBase>(this IServiceCollection serviceCollection, Assembly assembly, string suffix) where TBase : class
        {
            return serviceCollection.RegisterAllInternal(assembly, suffix, typeof(TBase));
        }
    }
}
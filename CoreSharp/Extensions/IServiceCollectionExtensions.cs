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
        private const string ContractPrefix = "I";
        private const string ServiceSuffix = "Service";
        private const string RepositorySuffix = "Repository";
        private const string NameGroupRegex = "(?<Name>.+)";

        //Methods
        private static string BuildContractRegex(string prefix, string suffix) => $"^{prefix}{NameGroupRegex}{suffix}$";

        /// <summary>
        /// Register all interface+implementation found in currently executing assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}{Suffix}` and `{Name}{Suffix}` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAll(this IServiceCollection serviceCollection, string suffix)
        {
            return serviceCollection.RegisterAll(Assembly.GetExecutingAssembly(), suffix);
        }

        /// <summary>
        /// Register all interface+implementation found in given assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}{Suffix}` and `{Name}{Suffix}` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAll(this IServiceCollection serviceCollection, Assembly assembly, string suffix)
        {
            //serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));

            string contractRegex = BuildContractRegex(ContractPrefix, suffix);
            var contracts = assembly.GetTypes().Where(t =>
                                    //Not registered already 
                                    !serviceCollection.Any(s => s.ServiceType == t)
                                    //Name convention matches
                                    && Regex.IsMatch(t.Name, contractRegex)
                                    //Public 
                                    && t.IsPublic
                                    //Interface
                                    && t.IsInterface);

            foreach (var contract in contracts)
            {
                //Get all implentations for given contract 
                var implementations = assembly.GetTypes().Where(t =>
                                        //Implements given interface 
                                        t.GetInterface(contract.FullName) != null
                                        //Public 
                                        && t.IsPublic
                                        //Class 
                                        && t.IsClass
                                        //Not abstract
                                        && !t.IsAbstract);

                var implementationsCount = implementations.Count();

                //If single implementations, register it 
                if (implementationsCount == 1)
                    serviceCollection.AddScoped(contract, implementations.First());

                //If multiple implementations
                else if (implementationsCount > 1)
                {
                    //Register only if there is one with the same name convention
                    var contractName = Regex.Match(contract.Name, contractRegex).Groups["Name"].Value;
                    var implementationName = $"{contractName}{suffix}";
                    var sameNameImplementation = implementations.FirstOrDefault(i => i.Name == implementationName);
                    if (sameNameImplementation != null)
                        serviceCollection.AddScoped(contract, sameNameImplementation);
                }
            }

            return serviceCollection;
        }

        /// <summary>
        /// Register all interface+implementation found in currently executing assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}` and `{Name}` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAll(this IServiceCollection serviceCollection)
        {
            return serviceCollection.RegisterAll(Assembly.GetExecutingAssembly(), string.Empty);
        }

        /// <summary>
        /// Register all interface+implementation found in given assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}Service` and `{Name}Service` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAll(this IServiceCollection serviceCollection, Assembly assembly)
        {
            return serviceCollection.RegisterAll(assembly, string.Empty);
        }

        /// <summary>
        /// Register all service+implementation found in currently executing assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}Service` and `{Name}Service` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAllServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.RegisterAll(Assembly.GetExecutingAssembly(), ServiceSuffix);
        }

        /// <summary>
        /// Register all service+implementation found in given assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}Service` and `{Name}Service` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAllServices(this IServiceCollection serviceCollection, Assembly assembly)
        {
            return serviceCollection.RegisterAll(assembly, ServiceSuffix);
        }

        /// <summary>
        /// Register all repository+implementation found in currently executing assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}Repository` and `{Name}Repository` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAllRepositories(this IServiceCollection serviceCollection)
        {
            return serviceCollection.RegisterAll(Assembly.GetExecutingAssembly(), RepositorySuffix);
        }

        /// <summary>
        /// Register all repository+implementation found in given assembly.  
        /// If single implementation is found, then it is registered regardless. 
        /// If multiple implementations are found, only the one with the `I{Name}Repository` and `{Name}Repository` convention is registered. 
        /// If multiple implementations are found and none has a proper name, then none is registered. 
        /// </summary> 
        public static IServiceCollection RegisterAllRepositories(this IServiceCollection serviceCollection, Assembly assembly)
        {
            return serviceCollection.RegisterAll(assembly, RepositorySuffix);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="Assembly"/> extensions.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <inheritdoc cref="LoadReferencedAssemblies(Assembly, Func{AssemblyName, bool})"/>
        public static IEnumerable<Assembly> LoadReferencedAssemblies(this Assembly assembly)
            => assembly.LoadReferencedAssemblies(_ => true);

        /// <summary>
        /// Loads all referenced <see cref="Assembly"/> list
        /// using <see cref="Assembly.GetReferencedAssemblies"/>
        /// and <see cref="Assembly.Load(AssemblyName)"/>.
        /// </summary>
        public static IEnumerable<Assembly> LoadReferencedAssemblies(this Assembly assembly, Func<AssemblyName, bool> filter)
        {
            _ = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _ = filter ?? throw new ArgumentNullException(nameof(filter));

            return assembly.GetReferencedAssemblies()
                           .Where(filter)
                           .Select(a => Assembly.Load(a));
        }
    }
}

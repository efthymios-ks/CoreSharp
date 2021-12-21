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
        /// <summary>
        /// Loads all referenced <see cref="Assembly"/> list
        /// using <see cref="Assembly.GetReferencedAssemblies"/>
        /// and <see cref="Assembly.Load(AssemblyName)"/>.
        /// </summary>
        public static IEnumerable<Assembly> LoadReferencedAssemblies(this Assembly assembly)
        {
            _ = assembly ?? throw new ArgumentNullException(nameof(assembly));

            return assembly.GetReferencedAssemblies()
                           .Select(a => Assembly.Load(a));
        }
    }
}

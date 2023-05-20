using System;
using System.Reflection;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="AssemblyName"/> extensions.
/// </summary>
public static class AssemblyNameExtensions
{
    /// <inheritdoc cref="Assembly.Load(AssemblyName)"/>
    public static Assembly Load(this AssemblyName assemblyName)
    {
        ArgumentNullException.ThrowIfNull(assemblyName);

        return Assembly.Load(assemblyName);
    }
}

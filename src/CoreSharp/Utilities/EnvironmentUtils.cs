using Microsoft.Extensions.Hosting;
using System;

namespace CoreSharp.Utilities;

/// <summary>
/// <see cref="Environment"/> utilities.
/// </summary>
public static class EnvironmentUtils
{
    // Methods
    /// <summary>
    /// Get ASPNETCORE_ENVIRONMENT variable.
    /// </summary>
    private static string GetAspNetCoreEnvironment()
        => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    /// <summary>
    /// Checks if the current host environment name is <see cref="EnvironmentName.Development"/>.
    /// </summary>
    public static bool IsDevelopment()
        => string.Equals(GetAspNetCoreEnvironment(), Environments.Development, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Checks if the current host environment name is <see cref="EnvironmentName.Staging"/>.
    /// </summary>
    public static bool IsStaging()
        => string.Equals(GetAspNetCoreEnvironment(), Environments.Staging, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Checks if the current host environment name is <see cref="EnvironmentName.Production"/>.
    /// </summary>
    public static bool IsProduction()
        => string.Equals(GetAspNetCoreEnvironment(), Environments.Production, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Check if DEBUG is defined.
    /// </summary>
    public const bool IsDebugging =
#if DEBUG
        true;
#else
        false;
#endif 
}

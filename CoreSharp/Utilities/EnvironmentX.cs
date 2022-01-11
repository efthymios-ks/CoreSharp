using System;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// <see cref="Environment"/> utilities.
    /// </summary>
    public static class EnvironmentX
    {
        //Methods
        /// <summary>
        /// Get ASPNETCORE_ENVIRONMENT variable.
        /// </summary>
        private static string GetAspNetCoreEnvironment()
            => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        /// <summary>
        /// Check if ASPNETCORE_ENVIRONMENT == DEVELOPMENT.
        /// </summary>
        public static bool IsDevelopment()
            => string.Equals(GetAspNetCoreEnvironment(), "Development", StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Check if DEBUG is defined.
        /// </summary>
        public static bool IsDebugging()
            =>
#if DEBUG
            return true;
#else
            false;
#endif

    }
}

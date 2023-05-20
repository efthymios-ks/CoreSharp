using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="IServiceCollection"/> extensions.
/// </summary>
public static class IServiceCollectionExtensions
{
    // Methods 
    /// <summary>
    /// Registers and attempts to bind a particular type of <see cref="IOptions{TOptions}"/>.
    /// </summary>
    public static IServiceCollection ConfigureBind<TOptions>(this IServiceCollection services, IConfigurationSection section)
        where TOptions : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(section);

        return services.Configure<TOptions>(section.Bind);
    }
}

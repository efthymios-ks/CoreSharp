using CoreSharp.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CoreSharp.DependencyInjection.Attributes
{
    /// <summary>
    /// Mark either interfaces (contracts) or classes (implementations) that need to be ignored by
    /// <see cref="IServiceCollectionExtensions.AddServices(IServiceCollection)"/> or
    /// <see cref="IServiceCollectionExtensions.AddMarkedServices(IServiceCollection)"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class IgnoreServiceAttribute : Attribute
    {
    }
}

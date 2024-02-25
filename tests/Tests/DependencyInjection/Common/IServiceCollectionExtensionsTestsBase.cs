using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Tests.DependencyInjection.Common;

[SuppressMessage(
    "Minor Code Smell", "S101:Types should be named in PascalCase",
    Justification = "<Pending>")]
public abstract class IServiceCollectionExtensionsTestsBase
{
    // Properties
    protected IServiceCollection ServiceCollection { get; set; }

    // Methods 
    protected ServiceDescriptor GetServiceDescriptor(Type serviceType)
        => ServiceCollection.FirstOrDefault(s => s.ServiceType == serviceType);

    protected ServiceDescriptor GetServiceDescriptor<TService>()
        => GetServiceDescriptor(typeof(TService));
}
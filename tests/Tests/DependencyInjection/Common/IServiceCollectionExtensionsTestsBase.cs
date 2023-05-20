using Microsoft.Extensions.DependencyInjection;

namespace Tests.DependencyInjection.Common;

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
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tests.Internal.Dummies.Services;
using Tests.Internal.Dummies.Services.Marked;
using Tests.Internal.Dummies.Services.Unmarked;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class IServiceCollectionExtensionsTests
{
    // Fields
    private IServiceCollection _serviceCollection;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var assembly = Assembly.GetExecutingAssembly();
        _serviceCollection = new ServiceCollection();
        _serviceCollection.AddServices(assembly);
        _serviceCollection.AddMarkedServices(assembly);
    }

    // Methods 
    #region Unmarked services
    [Test]
    public void AddServices_ServicesIsNull_ThrowArgumentNullException()
    {
        // Arrange 
        IServiceCollection services = null;

        // Act
        Action action = () => services.AddServices();

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void AddServices_InterfaceBaseTypeIsNull_ThrowArgumentNullException()
    {
        // Arrange 
        var services = new ServiceCollection();
        Type interfaceBaseType = null;

        // Act
        Action action = () => services.AddServices(interfaceBaseType);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void AddServices_AssembliesIsNull_ThrowArgumentNullException()
    {
        // Arrange 
        var services = new ServiceCollection();
        var interfaceBaseType = typeof(IPlainService);

        // Act
        Action action = () => services.AddServices(interfaceBaseType, assemblies: null);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void AddServices_NoImplementationFound_Skip()
    {
        // Act
        var descriptor = GetServiceDescriptor<IServiceWithNoImplementation>();

        // Assert
        descriptor.Should().BeNull();
    }

    [Test]
    public void AddServices_SingleImplementationFoundAndNameMissmatch_Register()
    {
        // Act
        var descriptor = GetServiceDescriptor<IServiceWithSingleImplementationAndNameMissmatch>();

        // Assert
        descriptor.Should().NotBeNull();
        descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
        descriptor.ServiceType.Should().Be<IServiceWithSingleImplementationAndNameMissmatch>();
        descriptor.ImplementationType.Should().Be<ServiceWithSingleImplementationAndNameMissmatch1>();
    }

    [Test]
    public void AddServices_SingleImplementationFoundAndNameMatch_Register()
    {
        // Act
        var descriptor = GetServiceDescriptor<IServiceWithSingleImplementationAndNameMatch>();

        // Assert
        descriptor.Should().NotBeNull();
        descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
        descriptor.ServiceType.Should().Be<IServiceWithSingleImplementationAndNameMatch>();
        descriptor.ImplementationType.Should().Be<ServiceWithSingleImplementationAndNameMatch>();
    }

    [Test]
    public void AddServices_ManyImplementationFoundAndNameMissmatch_Skip()
    {
        // Act
        var descriptor = GetServiceDescriptor<IServiceWithManyImplementationsAndNameMissmatch>();

        // Assert
        descriptor.Should().BeNull();
    }

    [Test]
    public void AddServices_ManyImplementationFoundAndNameMatch_Register()
    {
        // Act
        var descriptor = GetServiceDescriptor<IServiceWithManyImplementationsAndNameMatch>();

        // Assert
        descriptor.Should().NotBeNull();
        descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
        descriptor.ServiceType.Should().Be<IServiceWithManyImplementationsAndNameMatch>();
        descriptor.ImplementationType.Should().Be<ServiceWithManyImplementationsAndNameMatch>();
    }

    [Test]
    public void AddServices_MatchesButHasIgnoreServiceAttributeOnContract_Skip()
    {
        // Act
        var descriptor = GetServiceDescriptor<IServiceWithIgnoreServiceAttributeOnContract>();

        // Assert
        descriptor.Should().BeNull();
    }

    [Test]
    public void AddServices_MatchesButHasIgnoreServiceAttributeOnImplementation_Skip()
    {
        // Act
        var descriptor = GetServiceDescriptor<IServiceWithIgnoreServiceAttributeOnImplementation>();

        // Assert
        descriptor.Should().BeNull();
    }
    #endregion

    #region Marked services
    [Test]
    public void AddMarkedServices_ServicesIsNull_ThrowArgumentNullException()
    {
        // Arrange 
        IServiceCollection services = null;

        // Act
        Action action = () => services.AddMarkedServices();

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void AddMarkedServices_AssembliesIsNull_ThrowArgumentNullException()
    {
        // Arrange 
        var services = new ServiceCollection();

        // Act
        Action action = () => services.AddMarkedServices(assemblies: null);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void AddMarkedServices_TransientWithContract_Register()
    {
        // Act 
        var descriptor = GetServiceDescriptor<ITransientServiceWithContract>();

        // Assert 
        descriptor.Should().NotBeNull();
        descriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
        descriptor.ServiceType.Should().Be(typeof(ITransientServiceWithContract));
        descriptor.ImplementationType.Should().Be(typeof(TransientServiceWithContract));
    }

    [Test]
    public void AddMarkedServices_TransientWithoutContract_Register()
    {
        // Act 
        var descriptor = GetServiceDescriptor<ScopedServiceWithoutContract>();

        // Assert 
        descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
        descriptor.ServiceType.Should().Be(typeof(ScopedServiceWithoutContract));
        descriptor.ImplementationType.Should().Be(typeof(ScopedServiceWithoutContract));
    }

    [Test]
    public void AddMarkedServices_ScopedWithContract_Register()
    {
        // Act 
        var descriptor = GetServiceDescriptor<IScopedServiceWithContract>();

        // Assert 
        descriptor.Should().NotBeNull();
        descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
        descriptor.ServiceType.Should().Be(typeof(IScopedServiceWithContract));
        descriptor.ImplementationType.Should().Be(typeof(ScopedServiceWithContract));
    }

    [Test]
    public void AddMarkedServices_ScopedWithoutContract_Register()
    {
        // Act 
        var descriptor = GetServiceDescriptor<ScopedServiceWithoutContract>();

        // Assert 
        descriptor.Should().NotBeNull();
        descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
        descriptor.ServiceType.Should().Be(typeof(ScopedServiceWithoutContract));
        descriptor.ImplementationType.Should().Be(typeof(ScopedServiceWithoutContract));
    }

    [Test]
    public void AddMarkedServices_SingletonWithContract_Register()
    {
        // Act 
        var descriptor = GetServiceDescriptor<ISingletonServiceWithContract>();

        // Assert 
        descriptor.Should().NotBeNull();
        descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
        descriptor.ServiceType.Should().Be(typeof(ISingletonServiceWithContract));
        descriptor.ImplementationType.Should().Be(typeof(SingletonServiceWithContract));
    }

    [Test]
    public void AddMarkedServices_SingletonWithoutContract_Register()
    {
        // Act 
        var descriptor = GetServiceDescriptor<SingletonServiceWithoutContract>();

        // Assert 
        descriptor.Should().NotBeNull();
        descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
        descriptor.ServiceType.Should().Be(typeof(SingletonServiceWithoutContract));
        descriptor.ImplementationType.Should().Be(typeof(SingletonServiceWithoutContract));
    }

    [Test]
    public void AddMarkedServices_MatchesButHasIgnoreServiceAttributeOnContract_Skip()
    {
        // Act
        var descriptor = GetServiceDescriptor<IMarkedServiceWithIgnoreServiceAttributeOnContract>();

        // Assert
        descriptor.Should().BeNull();
    }

    [Test]
    public void AddMarkedServices_MatchesButHasIgnoreServiceAttributeOnImplementation_Skip()
    {
        // Act
        var descriptor = GetServiceDescriptor<IMarkedServiceWithIgnoreServiceAttributeOnImplementation>();

        // Assert
        descriptor.Should().BeNull();
    }

    [Test]
    public void AddMarkedServices_MatchesButHasIgnoreServiceAttributeOnItself_Skip()
    {
        // Act
        var descriptor = GetServiceDescriptor<MarkedServiceWithIgnoreServiceAttributeOnItself>();

        // Assert
        descriptor.Should().BeNull();
    }
    #endregion

    // Local 
    private ServiceDescriptor GetServiceDescriptor<TService>()
        => GetServiceDescriptor(typeof(TService));

    private ServiceDescriptor GetServiceDescriptor(Type serviceType)
        => _serviceCollection.FirstOrDefault(s => s.ServiceType == serviceType);
}

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tests.DependencyInjection.Extensions.Common;
using Tests.Internal.Dummies.Services.Marked;

namespace CoreSharp.DependencyInjection.Extensions.Tests;

[TestFixture]
public class IServiceCollectionExtensionsByInterfaceTests : IServiceCollectionExtensionsTestsBase
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var assembly = Assembly.GetExecutingAssembly();
        ServiceCollection = new ServiceCollection();
        ServiceCollection.AddMarkedServices(assembly);
    }

    // Methods  
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
}

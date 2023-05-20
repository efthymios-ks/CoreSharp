using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tests.DependencyInjection.Common;
using Tests.Internal.Dummies.Services;
using Tests.Internal.Dummies.Services.Unmarked;

namespace CoreSharp.DependencyInjection.ByReflection.Extensions.Tests;

[TestFixture]
public class IServiceCollectionExtensionsTests : IServiceCollectionExtensionsTestsBase
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var assembly = Assembly.GetExecutingAssembly();
        ServiceCollection = new ServiceCollection();
        ServiceCollection.AddServices(assembly);
    }

    // Methods 
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
}

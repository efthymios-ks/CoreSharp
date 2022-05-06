using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using Tests.Dummies.Services;
using Tests.Dummies.Services.Marked;
using Tests.Dummies.Services.Unmarked;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class IServiceCollectionExtensionsTests
    {
        //Fields
        private IServiceCollection _serviceCollection;
        private IServiceProvider _services;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _serviceCollection = new ServiceCollection();
            var assembly = Assembly.GetExecutingAssembly();
            _serviceCollection.AddServices(assembly);
            _serviceCollection.AddMarkedServices(assembly);
            _services = _serviceCollection.BuildServiceProvider();
        }

        //Methods 
        #region Unmarked services
        [Test]
        public void AddServices_ServicesIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            IServiceCollection services = null;

            //Act
            Action action = () => services.AddServices();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void AddServices_InterfaceBaseTypeIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            var services = new ServiceCollection();
            Type interfaceBaseType = null;

            //Act
            Action action = () => services.AddServices(interfaceBaseType);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void AddServices_AssembliesIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            var services = new ServiceCollection();
            var interfaceBaseType = typeof(IPlainService);

            //Act
            Action action = () => services.AddServices(interfaceBaseType, assemblies: null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void AddServices_NoImplementationFound_Skip()
        {
            //Act
            var implementation = _services.GetService<IServiceWithNoImplementation>();

            //Assert
            implementation.Should().BeNull();
        }

        [Test]
        public void AddServices_SingleImplementationFoundAndNameMissmatch_Register()
        {
            //Act
            var implementation = _services.GetService<IServiceWithSingleImplementationAndNameMissmatch>();

            //Assert
            implementation.Should().BeOfType<ServiceWithSingleImplementationAndNameMissmatch1>();
            implementation.Should().BeAssignableTo<IServiceWithSingleImplementationAndNameMissmatch>();
        }

        [Test]
        public void AddServices_SingleImplementationFoundAndNameMatch_Register()
        {
            //Act
            var implementation = _services.GetService<IServiceWithSingleImplementationAndNameMatch>();

            //Assert
            implementation.Should().BeOfType<ServiceWithSingleImplementationAndNameMatch>();
            implementation.Should().BeAssignableTo<IServiceWithSingleImplementationAndNameMatch>();
        }

        [Test]
        public void AddServices_ManyImplementationFoundAndNameMissmatch_Skip()
        {
            //Act
            var implementation = _services.GetService<IServiceWithManyImplementationsAndNameMissmatch>();

            //Assert
            implementation.Should().BeNull();
        }

        [Test]
        public void AddServices_ManyImplementationFoundAndNameMatch_Register()
        {
            //Act
            var implementation = _services.GetService<IServiceWithManyImplementationsAndNameMatch>();

            //Assert
            implementation.Should().BeOfType<ServiceWithManyImplementationsAndNameMatch>();
            implementation.Should().BeAssignableTo<IServiceWithManyImplementationsAndNameMatch>();
        }
        #endregion

        #region Marked services
        [Test]
        public void AddMarkedServices_ServicesIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            IServiceCollection services = null;

            //Act
            Action action = () => services.AddMarkedServices();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void AddMarkedServices_AssembliesIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            var services = new ServiceCollection();

            //Act
            Action action = () => services.AddMarkedServices(assemblies: null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void AddMarkedServices_TransientWithContract_Register()
        {
            //Act
            var implementation = _services.GetService<ITransientServiceWithContract>();
            var descriptor = GetServiceDescriptor<ITransientServiceWithContract>(_serviceCollection);

            //Assert
            implementation.Should().BeOfType<TransientServiceWithContract>();
            implementation.Should().BeAssignableTo<ITransientServiceWithContract>();

            descriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
            descriptor.ServiceType.Should().Be(typeof(ITransientServiceWithContract));
            descriptor.ImplementationType.Should().Be(typeof(TransientServiceWithContract));
        }

        [Test]
        public void AddMarkedServices_TransientWithoutContract_Register()
        {
            //Act
            var implementation = _services.GetService<ScopedServiceWithoutContract>();
            var descriptor = GetServiceDescriptor<ScopedServiceWithoutContract>(_serviceCollection);

            //Assert
            implementation.Should().BeOfType<ScopedServiceWithoutContract>();

            descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
            descriptor.ServiceType.Should().Be(typeof(ScopedServiceWithoutContract));
            descriptor.ImplementationType.Should().Be(typeof(ScopedServiceWithoutContract));
        }

        [Test]
        public void AddMarkedServices_ScopedWithContract_Register()
        {
            //Act
            var implementation = _services.GetService<IScopedServiceWithContract>();
            var descriptor = GetServiceDescriptor<IScopedServiceWithContract>(_serviceCollection);

            //Assert
            implementation.Should().BeOfType<ScopedServiceWithContract>();
            implementation.Should().BeAssignableTo<IScopedServiceWithContract>();

            descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
            descriptor.ServiceType.Should().Be(typeof(IScopedServiceWithContract));
            descriptor.ImplementationType.Should().Be(typeof(ScopedServiceWithContract));
        }

        [Test]
        public void AddMarkedServices_ScopedWithoutContract_Register()
        {
            //Act
            var implementation = _services.GetService<ScopedServiceWithoutContract>();
            var descriptor = GetServiceDescriptor<ScopedServiceWithoutContract>(_serviceCollection);

            //Assert
            implementation.Should().BeOfType<ScopedServiceWithoutContract>();

            descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
            descriptor.ServiceType.Should().Be(typeof(ScopedServiceWithoutContract));
            descriptor.ImplementationType.Should().Be(typeof(ScopedServiceWithoutContract));
        }

        [Test]
        public void AddMarkedServices_SingletonWithContract_Register()
        {
            //Act
            var implementation = _services.GetService<ISingletonServiceWithContract>();
            var descriptor = GetServiceDescriptor<ISingletonServiceWithContract>(_serviceCollection);

            //Assert
            implementation.Should().BeOfType<SingletonServiceWithContract>();
            implementation.Should().BeAssignableTo<ISingletonServiceWithContract>();

            descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            descriptor.ServiceType.Should().Be(typeof(ISingletonServiceWithContract));
            descriptor.ImplementationType.Should().Be(typeof(SingletonServiceWithContract));
        }

        [Test]
        public void AddMarkedServices_SingletonWithoutContract_Register()
        {
            //Act
            var implementation = _services.GetService<SingletonServiceWithoutContract>();
            var descriptor = GetServiceDescriptor<SingletonServiceWithoutContract>(_serviceCollection);

            //Assert
            implementation.Should().BeOfType<SingletonServiceWithoutContract>();

            descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            descriptor.ServiceType.Should().Be(typeof(SingletonServiceWithoutContract));
            descriptor.ImplementationType.Should().Be(typeof(SingletonServiceWithoutContract));
        }
        #endregion

        //Local 
        private static ServiceDescriptor GetServiceDescriptor<TService>(IServiceCollection services)
            => GetServiceDescriptor(services, typeof(TService));

        private static ServiceDescriptor GetServiceDescriptor(IServiceCollection services, Type serviceType)
            => services.FirstOrDefault(s => s.ServiceType == serviceType);
    }
}

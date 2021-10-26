using CoreSharp.Tests.Dummies;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class IServiceProviderExtensionsTests
    {
        //Fields
        private Mock<IServiceProvider> _serviceProviderMock;

        //Methods 
        [SetUp]
        public void SetUp()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
        }

        [Test]
        public void GetService_ServiceNotFound_ReturnNull()
        {
            //Arrange 
            _serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IDummyService)))
                .Returns(null);
            var serviceProvider = _serviceProviderMock.Object;

            //Act
            var result = serviceProvider.GetService<IDummyService>();

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public void GetService_ServiceFound_ReturnService()
        {
            //Arrange 
            var service = new DummyService();
            _serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IDummyService)))
                .Returns(service);
            var serviceProvider = _serviceProviderMock.Object;

            //Act
            var result = serviceProvider.GetService<IDummyService>();

            //Assert 
            result.Should().Be(service);
        }
    }
}

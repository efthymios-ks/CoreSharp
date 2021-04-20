using System;
using CoreSharp.Tests.Dummies;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class IServiceProviderExtensionsTests
    {
        //Fields
        private Mock<IServiceProvider> serviceProviderMock;

        //Methods 
        [SetUp]
        public void SetUp()
        {
            serviceProviderMock = new Mock<IServiceProvider>();
        }

        [Test]
        public void GetService_ServiceNotFound_ReturnNull()
        {
            //Arrange 
            serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IDummyService)))
                .Returns(null);
            var serviceProvider = serviceProviderMock.Object;

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
            serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IDummyService)))
                .Returns(service);
            var serviceProvider = serviceProviderMock.Object;

            //Act
            var result = serviceProvider.GetService<IDummyService>();

            //Assert 
            result.Should().Be(service);
        }
    }
}
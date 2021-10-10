using FluentAssertions;
using NUnit.Framework;
using System;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class TypeExtensionsTests
    {
        [Test]
        public void IsNumeric_TypeIsNull_ThrowArgumentNullException()
        {
            //Arrange
            Type typeNull = null;

            //Act
            Action action = () => typeNull.IsNumeric();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(typeof(double), true)]
        [TestCase(typeof(double?), true)]
        [TestCase(typeof(int), true)]
        [TestCase(typeof(int?), true)]
        [TestCase(typeof(string), false)]
        public void IsNumeric_TypeIsNumeric_ReturnTrue(Type type, bool expected)
        {
            //Act
            var result = type.IsNumeric();

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void IsDate_TypeIsNull_ThrowArgumentNullException()
        {
            //Arrange
            Type typeNull = null;

            //Act
            Action action = () => typeNull.IsDate();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(typeof(DateTime), true)]
        [TestCase(typeof(DateTime?), true)]
        [TestCase(typeof(DateTimeOffset), true)]
        [TestCase(typeof(DateTimeOffset?), true)]
        [TestCase(typeof(string), false)]
        public void IsDate_TypeIsDate_ReturnTrue(Type type, bool expected)
        {
            //Act
            var result = type.IsDate();

            //Assert
            result.Should().Be(expected);
        }
    }
}

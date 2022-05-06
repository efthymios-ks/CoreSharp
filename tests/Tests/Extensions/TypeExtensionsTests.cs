using FluentAssertions;
using NUnit.Framework;
using System;
using Tests.Dummies.Entities;

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

        [Test]
        [TestCase(typeof(int?), typeof(int))]
        [TestCase(typeof(int), typeof(int))]
        public void GetNullableBaseType_WhenCalled_ReturnBaseType(Type inputType, Type expectedType)
        {
            //Act
            var result = inputType.GetNullableBaseType();

            //Assert
            result.Should().Be(expectedType);
        }

        [Test]
        public void GetGenericTypeBase_ArgIsNull_ThrowArgumentNullException()
        {
            //Arrange
            Type type = null;

            //Act
            Action action = () => type.GetGenericTypeBase();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(typeof(DummyClass), typeof(DummyClass))]
        [TestCase(typeof(DummyClass<int>), typeof(DummyClass<>))]
        public void GetGenericTypeBase_WhenCalled_ReturnGenericBaseType(Type inputType, Type expectedType)
        {
            //Act
            var result = inputType.GetGenericTypeBase();

            //Assert
            result.Should().Be(expectedType);
        }

        [Test]
        [TestCase(null, typeof(int))]
        [TestCase(typeof(int), null)]
        public void Implements_ArgIsNull_ThrowArgumentNullException(Type parentType, Type baseType)
        {
            //Act
            Action action = () => parentType.Implements(baseType);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(typeof(DummyClass<>), typeof(int), false, false)]
        [TestCase(typeof(DummyClass<>), typeof(DummyClass), false, true)]
        [TestCase(typeof(DummyClass<>), typeof(DummyClass<>), false, true)]
        [TestCase(typeof(DummyClass<>), typeof(DummyClass<int>), false, false)]
        [TestCase(typeof(DummyClass<>), typeof(DummyClass<int>), true, true)]
        public void Implements_WhenCalled_ReturnGenericBaseType(Type parentType, Type baseType, bool useGenericBaseType, bool expected)
        {
            //Act
            var result = parentType.Implements(baseType, useGenericBaseType);

            //Assert
            result.Should().Be(expected);
        }
    }
}

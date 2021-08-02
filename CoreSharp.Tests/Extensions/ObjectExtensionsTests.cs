using CoreSharp.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture()]
    public class ObjectExtensionsTests
    {
        [Test]
        public void Is_InputIsNull_ReturnFalse()
        {
            //Arrange
            object input = null;

            //Action
            var result = input.Is<string>();

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void Is_InputTypeDiffers_ReturnFalse()
        {
            //Arrange
            object input = 1;

            //Action
            var result = input.Is<string>();

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void Is_InputTypeMatches_ReturnTrue()
        {
            //Arrange
            object input = 1;

            //Action
            var result = input.Is<int>();

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public void As_InputIsNull_ReturnNull()
        {
            //Arrange
            object input = null;

            //Action
            var result = input.As<string>();

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public void As_InputTypeDiffers_ReturnNull()
        {
            //Arrange
            object input = 1;

            //Action
            var result = input.As<string>();

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public void As_InputTypeMatches_ReturnCastValue()
        {
            //Arrange
            object input = 1;

            //Action
            var result = input.As<int>();

            //Assert
            result.Should().Be(1);
        }

        [Test]
        public void AsOrDefault_InputIsNull_ReturnDefault()
        {
            //Arrange
            object input = null;

            //Action
            var result = input.AsOrDefault<int>();

            //Assert
            result.Should().Be(default);
        }

        [Test]
        public void AsOrDefault_InputIsDbNull_ReturnDefault()
        {
            //Arrange
            object input = DBNull.Value;

            //Action
            var result = input.AsOrDefault<int>();

            //Assert
            result.Should().Be(default);
        }

        [Test]
        public void AsOrDefault_InputTypeDiffers_ReturnDefault()
        {
            //Arrange
            object input = "1";

            //Action
            var result = input.AsOrDefault<int>();

            //Assert
            result.Should().Be(default);
        }

        [Test]
        public void AsOrDefault_InputMissMatchAndGenericIsString_ReturnStringEmpty()
        {
            //Arrange
            object input = DBNull.Value;

            //Action
            var result = input.AsOrDefault<string>();

            //Assert
            result.Should().Be(string.Empty);
        }

        [Test]
        public void AsOrDefault_InputMissMatchAndGivenDefaultValue_ReturnGivenDefaultValue()
        {
            //Arrange
            object input = "1";

            //Action
            var result = input.AsOrDefault(2);

            //Assert
            result.Should().Be(2);
        }

        [Test]
        public void AsOrDefault_InputTypeMatches_ReturnCastValue()
        {
            //Arrange
            object input = 1;

            //Action
            var result = input.AsOrDefault<int>();

            //Assert
            result.Should().Be(1);
        }
    }
}
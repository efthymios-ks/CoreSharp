using FluentAssertions;
using NUnit.Framework;
using System;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
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

using CoreSharp.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        //Fields
        private readonly string stringNull = null;
        private readonly string stringEmpty = string.Empty;

        //Methods 
        [Test]
        public void Truncate_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => stringNull.Truncate(5);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Truncate_LengthIsNegative_ThrowArgumentOutOfRangeException()
        {
            //Act
            Action action = () => stringEmpty.Truncate(-1);

            //Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Truncate_WhenCalled_ReturnTruncatedString()
        {
            //Arrange
            string input = "12345";
            string expected = "123";

            //Act
            var result = input.Truncate(3);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void FormatAsciiControls_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => stringNull.FormatAsciiControls();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void FormatAsciiControls_WhenCalled_ReplaceAsciiControlCharsAndReturn()
        {
            //Arrange
            char SOH = Convert.ToChar(1);
            char EOT = Convert.ToChar(4);
            string input = $"{SOH}_Hello_{EOT}";
            string expected = "<SOH>_Hello_<EOT>";

            //Act
            var result = input.FormatAsciiControls('<', '>');

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void SplitChunks_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => stringNull.SplitChunks(2);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void SplitChunks_ChunckSizeIsZeroOrLess_ThrowArgumentOutOfRangeException(int chunkSize)
        {
            //Act
            Action action = () => stringEmpty.SplitChunks(chunkSize);

            //Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void SplitChunks_WhenCalled_SplitAndReturnInputInChunks()
        {
            //Arrange 
            string input = "12345";
            var expected = new[] { "12", "34", "5" };

            //Act
            var result = input.SplitChunks(2);

            //Assert
            result.Should().Equal(expected);
        }

        [Test]
        public void PadCenter_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => stringNull.PadCenter(2);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void PadCenter_TotalWidthIsNegative_ThrowArgumentOutOfRangeException()
        {
            //Act
            Action action = () => stringEmpty.PadCenter(-1);

            //Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void PadCenter_WhenCalled_PadCenterAndReturnInput()
        {
            //Arrange
            string input = "123";
            char padCharacter = ' ';
            int totalWidth = 7;
            string expected = "  123  ";


            //Act
            var result = input.PadCenter(totalWidth, padCharacter);

            //Assert
            result.Should().Be(expected);
        }
    }
}
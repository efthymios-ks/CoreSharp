using CoreSharp.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Globalization;

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
            Action action = () => stringNull.Truncate(3);

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
        public void SplitChunks_WhenCalled_SplitInputInChunks()
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

        [Test]
        public void RemoveFirst_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => stringNull.RemoveFirst("1");

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void RemoveFirst_ValueIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => stringEmpty.RemoveFirst(null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void RemoveFirst_WhenCalled_RemoveFirstValueOccurence()
        {
            //Arrange
            string input = "1 A 2 A 3 A";
            string value = "A";
            string expected = "1  2 A 3 A";

            //Act
            var result = input.RemoveFirst(value);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void RemoveAll_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => stringNull.RemoveAll("1");

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void RemoveAll_ValueIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => stringEmpty.RemoveAll(null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void RemoveAll_WhenCalled_RemoveFirstValueOccurence()
        {
            //Arrange
            string input = "1 A 2 A 3 A";
            string value = "A";
            string expected = "1  2  3 ";

            //Act
            var result = input.RemoveAll(value);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void Left_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => stringNull.Left(3);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Left_LengthIsNegative_ThrowArgumentOutOfRangeException()
        {
            //Act
            Action action = () => stringEmpty.Left(-1);

            //Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Left_WhenCalled_ReturnLeftSubstring()
        {
            //Arrange
            string input = "12345";
            string expected = "123";

            //Act
            var result = input.Left(3);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void Right_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => stringNull.Right(3);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Right_LengthIsNegative_ThrowArgumentOutOfRangeException()
        {
            //Act
            Action action = () => stringEmpty.Right(-1);

            //Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Right_WhenCalled_ReturnRightSubstring()
        {
            //Arrange
            string input = "12345";
            string expected = "345";

            //Act
            var result = input.Right(3);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void Mid_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => stringNull.Mid(2);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase("123", -1)]
        [TestCase("123", 4)]
        public void Mid_IndexIsOutOfRange_ThrowArgumentOutOfRangeException(string input, int index)
        {
            //Act
            Action action = () => input.Mid(index, 2);

            //Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Mid_LengthIsNegative_ThrowArgumentOutOfRangeException()
        {
            //Act
            Action action = () => stringEmpty.Mid(0, -1);

            //Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Mid_WhenCalled_ReturnMidSubstring()
        {
            //Arrange
            string input = "12345";
            string expected = "234";

            //Act
            var result = input.Mid(1, 3);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void FormatWith_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => stringNull.FormatWith(1);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void FormatWith_FormatProviderIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => stringEmpty.FormatWith(formatProvider: null, parameters: 1);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void FormatWith_ParametersIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => stringEmpty.FormatWith(CultureInfo.InvariantCulture, parameters: null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void FormatWith_WhenCalled_ReturnFormattedString()
        {
            //Arrange
            string format = "{0}";
            int value = 1000;
            var culture = CultureInfo.CurrentCulture;
            string expected = string.Format(culture, format, value);

            //Act
            var result = format.FormatWith(culture, value);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void FormatWithCI_WhenCalled_FormatStringWithCultureInvariant()
        {
            //Arrange
            string format = "{0}";
            int value = 1000;
            var culture = CultureInfo.InvariantCulture;
            string expected = "1000";

            //Act
            var result = format.FormatWith(culture, value);

            //Assert
            result.Should().Be(expected);
        }
    }
}
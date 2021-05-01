using System;
using System.Globalization;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class StringBuilderExtensionsTests
    {
        //Fields
        private readonly StringBuilder builderNull = null;
        private readonly StringBuilder builder = new();

        //Methods
        [SetUp]
        public void SetUp()
        {
            builder.Clear();
        }

        [Test]
        public void AppendFormatLine_BuilderIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => builderNull.AppendFormatLine("{0}", 1);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void AppendFormatLine_WhenCalled_AppendFormattedStringAndNewLine()
        {
            //Arrange
            string format = "{0}";
            int value = 1000;
            var culture = CultureInfo.CurrentCulture;
            string formatResult = string.Format(culture, format, value);
            string expected = formatResult + Environment.NewLine;

            //Act
            var result = builder.AppendFormatLine(culture, format, value);

            //Assert
            result.ToString().Should().EndWith(expected);
        }

        [Test]
        public void AppendFormatLineCI_WhenCalledAppendStringFormatWithInvariantCultureArgument()
        {
            //Arrange
            string format = "{0}";
            int value = 1000;
            string formatResult = string.Format(CultureInfo.InvariantCulture, format, value);
            string expected = formatResult + Environment.NewLine;

            //Act
            var result = builder.AppendFormatLineCI(format, value);

            //Assert
            result.ToString().Should().EndWith(expected);
        }
    }
}
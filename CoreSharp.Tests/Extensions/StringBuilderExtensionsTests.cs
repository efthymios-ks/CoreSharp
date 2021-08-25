﻿using FluentAssertions;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Text;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class StringBuilderExtensionsTests
    {
        //Fields
        private readonly StringBuilder _builderNull = null;
        private readonly StringBuilder _builder = new();

        //Methods
        [SetUp]
        public void SetUp()
        {
            _builder.Clear();
        }

        [Test]
        public void AppendFormatLine_BuilderIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => _builderNull.AppendFormatLine("{0}", 1);

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
            var result = _builder.AppendFormatLine(culture, format, value);

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
            var result = _builder.AppendFormatLineCI(format, value);

            //Assert
            result.ToString().Should().EndWith(expected);
        }
    }
}
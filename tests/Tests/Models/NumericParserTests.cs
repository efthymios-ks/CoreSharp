using CoreSharp.Models;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Linq;
using Tests.Attributes;

namespace Tests.Models
{
    [TestFixture]
    public class NumericParserTests
    {
        [Test]
        [TestCaseGeneric(TypeArguments = new[] { typeof(string) })]
        [TestCaseGeneric(TypeArguments = new[] { typeof(DateTime) })]
        [TestCaseGeneric(TypeArguments = new[] { typeof(DateTime?) })]
        [TestCaseGeneric(TypeArguments = new[] { typeof(Guid) })]
        [TestCaseGeneric(TypeArguments = new[] { typeof(Guid?) })]
        public void Constructor_GenericIsNotNumeric_ThrowArgumentException<TValue>()
        {
            //Act 
            Action action = () => _ = new NumericParser<TValue>();

            //Assert 
            action.Should().ThrowExactly<ArgumentException>();
        }

        [Test]
        [TestCaseGeneric(TypeArguments = new[] { typeof(int) })]
        [TestCaseGeneric(TypeArguments = new[] { typeof(int?) })]
        [TestCaseGeneric(TypeArguments = new[] { typeof(float) })]
        [TestCaseGeneric(TypeArguments = new[] { typeof(float?) })]
        [TestCaseGeneric(TypeArguments = new[] { typeof(double) })]
        [TestCaseGeneric(TypeArguments = new[] { typeof(double?) })]
        [TestCaseGeneric(TypeArguments = new[] { typeof(decimal) })]
        [TestCaseGeneric(TypeArguments = new[] { typeof(decimal?) })]
        public void Constructor_GenericIsNumeric_CreateInstance<TValue>()
        {
            //Act 
            var result = new NumericParser<TValue>();
            var resultType = result.GetType();
            var resultTypeGenericArguments = resultType.GetGenericArguments();
            var resultTypeGenericArgument = resultTypeGenericArguments?.FirstOrDefault();

            //Assert 
            result.Should().NotBeNull();
            resultType.IsGenericType.Should().BeTrue();
            resultTypeGenericArguments.Should().HaveCount(1);
            resultTypeGenericArgument.Should().Be(typeof(TValue));
        }

        [Test]
        public void Constructor_CultureIsNull_ThrowArgumentException()
        {
            //Act 
            Action action = () => _ = new NumericParser<int>(cultureInfo: null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase("N2", "en-US", 1000.121, "1,000.12")]
        [TestCase("N2", "el-GR", 1000.121, "1.000,12")]
        [TestCase("C2", "en-US", 1000.129, "$1,000.13")]
        [TestCase("C2", "el-GR", 1000.129, "1.000,13 €")]
        [TestCase("P2", "en-US", 0.555, "55.50%")]
        [TestCase("P2", "el-GR", 55.5, "5.550,00%")]
        public void FormatValue_WhenCalled_FormatRespectingFormatAndCulture(string format, string cultureName, double value, string expected)
        {
            //Arrange
            var culture = CultureInfo.CreateSpecificCulture(cultureName);
            var parser = new NumericParser<double>(format, culture);

            //Act 
            var formattedValue = parser.FormatValue(value);

            //Assert 
            formattedValue.Should().Be(expected);
        }

        [Test]
        [TestCase("N2", "en-US", null)]
        [TestCase("N2", "en-US", "")]
        [TestCase("N2", "en-US", "Test")]
        [TestCase("N2", "en-US", "1.000,12")]
        [TestCase("N2", "en-US", "1.000,13 €")]
        public void TryParseValue_InvalidInput_ReturnFalseAndDefaultValue(string format, string cultureName, string input)
        {
            //Arrange
            var culture = CultureInfo.CreateSpecificCulture(cultureName);
            var parser = new NumericParser<double>(format, culture);

            //Act 
            var result = parser.TryParseValue(input, out var value);

            //Assert 
            result.Should().BeFalse();
            value.Should().Be(default);
        }

        [Test]
        [TestCase("N2", "en-US", "1,000.121", 1000.12)]
        [TestCase("N2", "el-GR", "1.000,121", 1000.12)]
        [TestCase("C2", "en-US", "$1,000.129", 1000.13)]
        [TestCase("C2", "el-GR", "1.000,129 €", 1000.13)]
        [TestCase("P2", "en-US", "55.55%", 0.5555)]
        [TestCase("P2", "el-GR", "55,550%", 0.5555)]
        public void TryParseValue_WhenCalled_ReturnTrueAndParsedValue(string format, string cultureName, string input, double expected)
        {
            //Arrange
            var culture = CultureInfo.CreateSpecificCulture(cultureName);
            var parser = new NumericParser<double>(format, culture);

            //Act 
            var result = parser.TryParseValue(input, out var value);

            //Assert 
            result.Should().BeTrue();
            value.Should().Be(expected);
        }
    }
}
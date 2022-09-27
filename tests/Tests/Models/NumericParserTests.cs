using Tests.Attributes;

namespace CoreSharp.Models.Tests;

[TestFixture]
public class NumericParserTests
{
    [Test]
    [GenericTestCase(TypeArguments = new[] { typeof(string) })]
    [GenericTestCase(TypeArguments = new[] { typeof(DateTime) })]
    [GenericTestCase(TypeArguments = new[] { typeof(DateTime?) })]
    [GenericTestCase(TypeArguments = new[] { typeof(Guid) })]
    [GenericTestCase(TypeArguments = new[] { typeof(Guid?) })]
    public void Constructor_GenericIsNotNumeric_ThrowArgumentException<TValue>()
    {
        // Act 
        Action action = () => _ = new NumericParser<TValue>();

        // Assert 
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    [GenericTestCase(TypeArguments = new[] { typeof(int) })]
    [GenericTestCase(TypeArguments = new[] { typeof(int?) })]
    [GenericTestCase(TypeArguments = new[] { typeof(float) })]
    [GenericTestCase(TypeArguments = new[] { typeof(float?) })]
    [GenericTestCase(TypeArguments = new[] { typeof(double) })]
    [GenericTestCase(TypeArguments = new[] { typeof(double?) })]
    [GenericTestCase(TypeArguments = new[] { typeof(decimal) })]
    [GenericTestCase(TypeArguments = new[] { typeof(decimal?) })]
    public void Constructor_GenericIsNumeric_CreateInstance<TValue>()
    {
        // Act 
        var result = new NumericParser<TValue>();
        var resultType = result.GetType();
        var resultTypeGenericArguments = resultType.GetGenericArguments();
        var resultTypeGenericArgument = resultTypeGenericArguments?.FirstOrDefault();

        // Assert 
        result.Should().NotBeNull();
        resultType.IsGenericType.Should().BeTrue();
        resultTypeGenericArguments.Should().HaveCount(1);
        resultTypeGenericArgument.Should().Be(typeof(TValue));
    }

    [Test]
    public void Constructor_CultureIsNull_ThrowArgumentException()
    {
        // Act 
        Action action = () => _ = new NumericParser<int>(cultureInfo: null);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase("N2", "en-US", 1000.121, "1,000.12")]
    [TestCase("N2", "el-GR", 1000.121, "1.000,12")]
    [TestCase("C2", "en-US", 1000.129, "$1,000.13")]
    [TestCase("C2", "el-GR", 1000.129, "1.000,13 €")]
    [TestCase("P2", "en-US", 0.555, "55.50%")]
    [TestCase("P2", "el-GR", 55.5, "5.550,00%")]
    public void FormatValue_WhenCalled_FormatRespectingFormatAndCulture(string format, string cultureName, double value, string expectedFormattedValue)
    {
        // Arrange
        var culture = CultureInfo.CreateSpecificCulture(cultureName);
        var parser = new NumericParser<double>(format, culture);

        // Act 
        var formattedValue = parser.ToString(value);

        // Assert 
        formattedValue.Should().Be(expectedFormattedValue);
    }

    [Test]
    [TestCase("N2", "en-US", null)]
    [TestCase("N2", "en-US", "")]
    [TestCase("N2", "en-US", "Test")]
    [TestCase("N2", "en-US", "1.000,12")]
    [TestCase("N2", "en-US", "1.000,13 €")]
    public void TryParseValue_InvalidInput_ReturnFalseAndUntouchedValue(string format, string cultureName, string input)
    {
        // Arrange
        var culture = CultureInfo.CreateSpecificCulture(cultureName);
        var parser = new NumericParser<double>(format, culture);
        const double initialValue = -1.0;

        // Act 
        var value = initialValue;
        var parsed = parser.TryParse(input, ref value);

        // Assert 
        parsed.Should().BeFalse();
        value.Should().Be(initialValue);
    }

    [Test]
    [TestCase("N2", "en-US", "1,000.121", 1000.12)]
    [TestCase("N2", "el-GR", "1.000,121", 1000.12)]
    [TestCase("C2", "en-US", "$1,000.129", 1000.13)]
    [TestCase("C2", "el-GR", "1.000,129 €", 1000.13)]
    [TestCase("P2", "en-US", "55.55%", 0.5555)]
    [TestCase("P2", "el-GR", "55,550%", 0.5555)]
    public void TryParseValue_WhenCalled_ReturnTrueAndParsedValue(string format, string cultureName, string input, double expectedValue)
    {
        // Arrange
        var culture = CultureInfo.CreateSpecificCulture(cultureName);
        var parser = new NumericParser<double>(format, culture);
        var value = -1.0;

        // Act 
        var parsed = parser.TryParse(input, ref value);

        // Assert 
        parsed.Should().BeTrue();
        value.Should().Be(expectedValue);
    }
}
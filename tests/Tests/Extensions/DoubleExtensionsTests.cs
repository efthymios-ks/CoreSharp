using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class DoubleExtensionsTests
{
    [Test]
    [TestCase(1, 1, 3, 1, 10, 1)]
    [TestCase(2, 1, 3, 1, 10, 5.5)]
    [TestCase(3, 1, 3, 1, 10, 10)]
    public void Map_WhenCalled_MapAndReturnValueToNewRange(double value, double fromLow, double fromHigh, double toLow, double toHigh, double expected)
    {
        //Act
        var result = value.Map(fromLow, fromHigh, toLow, toHigh);

        //Assert
        result.Should().Be(expected);
    }

    [Test]
    [TestCase(-0.0001234567, "-123.457u")]
    [TestCase(-0.1234, "-123.4m")]
    [TestCase(0, "0")]
    [TestCase(1, "1")]
    [TestCase(1234, "1.234k")]
    [TestCase(1234567, "1.235M")]
    public void ToMetricSize_WhenCalled_ReturnSiMetricString(double value, string expected)
    {
        //Action
        var result = value.ToMetricSizeCI();

        //Assert
        result.Should().Be(expected);
    }
}

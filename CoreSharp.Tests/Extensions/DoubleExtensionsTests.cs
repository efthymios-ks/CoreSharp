using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests
{
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
    }
}
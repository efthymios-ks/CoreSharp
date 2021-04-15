using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture()]
    public class DecimalExtensionsTests
    {
        [Test]
        [TestCase(1, 1, 3, 1, 10, 1)]
        [TestCase(2, 1, 3, 1, 10, 5.5)]
        [TestCase(3, 1, 3, 1, 10, 10)]
        public void Map_WhenCalled_MapAndReturnValueToNewRange(decimal value, decimal fromLow, decimal fromHigh, decimal toLow, decimal toHigh, decimal expected)
        {
            //Act
            var result = value.Map(fromLow, fromHigh, toLow, toHigh);

            //Assert
            result.Should().Be(expected);
        }
    }
}
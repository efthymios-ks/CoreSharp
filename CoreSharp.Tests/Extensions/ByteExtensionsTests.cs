using NUnit.Framework;
using FluentAssertions;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class ByteExtensionsTests
    {
        [Test]
        [TestCase(1, 1, 3, 1, 10, 1)]
        [TestCase(2, 1, 3, 1, 10, 5)]
        [TestCase(3, 1, 3, 1, 10, 10)]
        public void Map_WhenCalled_MapAndReturnValueToNewRange(byte value, byte fromLow, byte fromHigh, byte toLow, byte toHigh, byte expected)
        {
            //Act
            var result = value.Map(fromLow, fromHigh, toLow, toHigh);

            //Assert
            result.Should().Be(expected);
        }
    }
}
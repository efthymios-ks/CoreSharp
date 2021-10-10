using FluentAssertions;
using NUnit.Framework;
using System;
using System.Globalization;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class UlongExtensionsTests
    {
        public static void ToComputerSize_FormatIsNull_ThrowArgumentNullException()
        {
            //Action
            Action action = () => 1ul.ToComputerSize(null, CultureInfo.InvariantCulture);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        public static void ToComputerSize_FormatProviderIsNull_ThrowArgumentNullException()
        {
            //Action
            Action action = () => 1ul.ToComputerSize("{0}", null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(0ul, "0B")]
        [TestCase(512ul, "512B")]
        [TestCase(1024ul, "1KB")]
        [TestCase(1536ul, "1.5KB")]
        [TestCase(1048576ul, "1MB")]
        [TestCase(1572864ul, "1.5MB")]
        [TestCase(1073741824ul, "1GB")]
        [TestCase(1610612736ul, "1.5GB")]
        public void ToComputerSize_WhenCalled_ReturnSiMetricString(ulong value, string expected)
        {
            //Action
            var result = value.ToComputerSizeCI();

            //Assert
            result.Should().Be(expected);
        }
    }
}

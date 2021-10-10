﻿using FluentAssertions;
using NUnit.Framework;
using System;
using System.Globalization;

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

        public static void ToMetricSize_FormatIsNull_ThrowArgumentNullException()
        {
            //Action
            Action action = () => 1.0.ToMetricSize(null, CultureInfo.InvariantCulture);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        public static void ToMetricSize_FormatProviderIsNull_ThrowArgumentNullException()
        {
            //Action
            Action action = () => 1.0.ToMetricSize("{0}", null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
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
}

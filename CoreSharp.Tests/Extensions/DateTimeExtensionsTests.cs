using CoreSharp.Extensions;
using System;
using FluentAssertions;
using NUnit.Framework;
using CoreSharp.Enums;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        //Methods
        [Test]
        public void HasExpired_TimeSpanHasExpired_ReturnTrue()
        {
            //Arrange  
            var elapsed = TimeSpan.FromMinutes(15);
            var from = DateTime.Now.Subtract(elapsed);

            //Act 
            var result = from.HasExpired(elapsed);

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        [TestCase(2021, 4, 10, true)]
        [TestCase(2021, 4, 11, true)]
        [TestCase(2021, 4, 12, false)]
        [TestCase(2021, 4, 13, false)]
        public void IsWeekend_DateIsWeekend_ReturnTrue(int year, int month, int day, bool expected)
        {
            //Arrange
            var date = new DateTime(year, month, day);

            //Act
            var result = date.IsWeekend();

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        [TestCase(2020, 1, 1, true)]
        [TestCase(2022, 1, 1, false)]
        [TestCase(2024, 1, 1, true)]
        [TestCase(2026, 1, 1, false)]
        public void IsInLeapYear_DateIsLeapYear_ReturnTrue(int year, int month, int day, bool expected)
        {
            //Arrange
            var date = new DateTime(year, month, day);

            //Act
            var result = date.IsInLeapYear();

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void Trim_WhenCalled_TrimToGivenPrecisionAndReturn()
        {
            //Arrange 
            var date = new DateTime(2021, 5, 2, 21, 34, 55, 500);
            var precision = DateTimePrecision.Date | DateTimePrecision.Milliseconds;
            var expected = new DateTime(1, 1, 1, 21, 34, 55, 0);

            //Act
            var result = date.Trim(precision);

            result.Should().Be(expected);
        }
    }
}
using System;
using FluentAssertions;
using NUnit.Framework;

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
        [TestCase(2021, -1)]
        [TestCase(2021, 32)]
        public void January_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int year, int day)
        {
            //Act
            Action action = () => day.January(year);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void January_WhenCalled_ReturnJanuaryDate()
        {
            //Arrange 
            int year = 2021;
            int month = 1;
            int day = 1;
            var date = new DateTime(year, month, day);

            //Act
            var result = day.January(year);

            //Assert 
            result.Should().Be(date);
        }

        [Test]
        [TestCase(2021, -1)]
        [TestCase(2021, 30)]
        public void February_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int year, int day)
        {
            //Act
            Action action = () => day.February(year);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void February_WhenCalled_ReturnFebruaryDate()
        {
            //Arrange 
            int year = 2021;
            int month = 2;
            int day = 1;
            var date = new DateTime(year, month, day);

            //Act
            var result = day.February(year);

            //Assert 
            result.Should().Be(date);
        }

        [Test]
        [TestCase(2021, -1)]
        [TestCase(2021, 32)]
        public void March_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int year, int day)
        {
            //Act
            Action action = () => day.March(year);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void March_WhenCalled_ReturnMarchDate()
        {
            //Arrange 
            int year = 2021;
            int month = 3;
            int day = 1;
            var date = new DateTime(year, month, day);

            //Act
            var result = day.March(year);

            //Assert 
            result.Should().Be(date);
        }

        [Test]
        [TestCase(2021, -1)]
        [TestCase(2021, 31)]
        public void April_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int year, int day)
        {
            //Act
            Action action = () => day.April(year);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void April_WhenCalled_ReturnAprilDate()
        {
            //Arrange 
            int year = 2021;
            int month = 4;
            int day = 1;
            var date = new DateTime(year, month, day);

            //Act
            var result = day.April(year);

            //Assert 
            result.Should().Be(date);
        }

        [Test]
        [TestCase(2021, -1)]
        [TestCase(2021, 32)]
        public void May_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int year, int day)
        {
            //Act
            Action action = () => day.May(year);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void May_WhenCalled_ReturnMayDate()
        {
            //Arrange 
            int year = 2021;
            int month = 5;
            int day = 1;
            var date = new DateTime(year, month, day);

            //Act
            var result = day.May(year);

            //Assert 
            result.Should().Be(date);
        }

        [Test]
        [TestCase(2021, -1)]
        [TestCase(2021, 31)]
        public void June_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int year, int day)
        {
            //Act
            Action action = () => day.June(year);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void June_WhenCalled_ReturnJuneDate()
        {
            //Arrange 
            int year = 2021;
            int month = 6;
            int day = 1;
            var date = new DateTime(year, month, day);

            //Act
            var result = day.June(year);

            //Assert 
            result.Should().Be(date);
        }

        [Test]
        [TestCase(2021, -1)]
        [TestCase(2021, 32)]
        public void July_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int year, int day)
        {
            //Act
            Action action = () => day.July(year);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void July_WhenCalled_ReturnJulyDate()
        {
            //Arrange 
            int year = 2021;
            int month = 7;
            int day = 1;
            var date = new DateTime(year, month, day);

            //Act
            var result = day.July(year);

            //Assert 
            result.Should().Be(date);
        }

        [Test]
        [TestCase(2021, -1)]
        [TestCase(2021, 32)]
        public void August_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int year, int day)
        {
            //Act
            Action action = () => day.August(year);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void August_WhenCalled_ReturnAugustDate()
        {
            //Arrange 
            int year = 2021;
            int month = 8;
            int day = 1;
            var date = new DateTime(year, month, day);

            //Act
            var result = day.August(year);

            //Assert 
            result.Should().Be(date);
        }

        [Test]
        [TestCase(2021, -1)]
        [TestCase(2021, 31)]
        public void September_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int year, int day)
        {
            //Act
            Action action = () => day.September(year);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void September_WhenCalled_ReturnSeptemberDate()
        {
            //Arrange 
            int year = 2021;
            int month = 9;
            int day = 1;
            var date = new DateTime(year, month, day);

            //Act
            var result = day.September(year);

            //Assert 
            result.Should().Be(date);
        }

        [Test]
        [TestCase(2021, -1)]
        [TestCase(2021, 32)]
        public void October_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int year, int day)
        {
            //Act
            Action action = () => day.October(year);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void October_WhenCalled_ReturnOctoberDate()
        {
            //Arrange 
            int year = 2021;
            int month = 10;
            int day = 1;
            var date = new DateTime(year, month, day);

            //Act
            var result = day.October(year);

            //Assert 
            result.Should().Be(date);
        }

        [Test]
        [TestCase(2021, -1)]
        [TestCase(2021, 31)]
        public void November_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int year, int day)
        {
            //Act
            Action action = () => day.November(year);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void November_WhenCalled_ReturnNovemberDate()
        {
            //Arrange 
            int year = 2021;
            int month = 11;
            int day = 1;
            var date = new DateTime(year, month, day);

            //Act
            var result = day.November(year);

            //Assert 
            result.Should().Be(date);
        }

        [Test]
        [TestCase(2021, -1)]
        [TestCase(2021, 32)]
        public void December_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int year, int day)
        {
            //Act
            Action action = () => day.December(year);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void December_WhenCalled_ReturnDecemberDate()
        {
            //Arrange 
            int year = 2021;
            int month = 12;
            int day = 1;
            var date = new DateTime(year, month, day);

            //Act
            var result = day.December(year);

            //Assert 
            result.Should().Be(date);
        }
    }
}
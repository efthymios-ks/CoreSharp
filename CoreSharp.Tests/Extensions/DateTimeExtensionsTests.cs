using NUnit.Framework;
using System;
using FluentAssertions;

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
        [TestCase(-1, 2021)]
        [TestCase(32, 2021)]
        public void January_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int day, int year)
        {
            //Act
            Action action = () => day.January(year);

            //Assert 
            action.Should().Throw<ArgumentOutOfRangeException>();
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
        [TestCase(-1, 2021)]
        [TestCase(30, 2021)]
        public void February_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int day, int year)
        {
            //Act
            Action action = () => day.February(year);

            //Assert 
            action.Should().Throw<ArgumentOutOfRangeException>();
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
        [TestCase(-1, 2021)]
        [TestCase(32, 2021)]
        public void March_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int day, int year)
        {
            //Act
            Action action = () => day.March(year);

            //Assert 
            action.Should().Throw<ArgumentOutOfRangeException>();
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
        [TestCase(-1, 2021)]
        [TestCase(31, 2021)]
        public void April_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int day, int year)
        {
            //Act
            Action action = () => day.April(year);

            //Assert 
            action.Should().Throw<ArgumentOutOfRangeException>();
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
        [TestCase(-1, 2021)]
        [TestCase(32, 2021)]
        public void May_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int day, int year)
        {
            //Act
            Action action = () => day.May(year);

            //Assert 
            action.Should().Throw<ArgumentOutOfRangeException>();
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
        [TestCase(-1, 2021)]
        [TestCase(31, 2021)]
        public void June_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int day, int year)
        {
            //Act
            Action action = () => day.June(year);

            //Assert 
            action.Should().Throw<ArgumentOutOfRangeException>();
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
        [TestCase(-1, 2021)]
        [TestCase(32, 2021)]
        public void July_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int day, int year)
        {
            //Act
            Action action = () => day.July(year);

            //Assert 
            action.Should().Throw<ArgumentOutOfRangeException>();
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
        [TestCase(-1, 2021)]
        [TestCase(32, 2021)]
        public void August_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int day, int year)
        {
            //Act
            Action action = () => day.August(year);

            //Assert 
            action.Should().Throw<ArgumentOutOfRangeException>();
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
        [TestCase(-1, 2021)]
        [TestCase(31, 2021)]
        public void September_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int day, int year)
        {
            //Act
            Action action = () => day.September(year);

            //Assert 
            action.Should().Throw<ArgumentOutOfRangeException>();
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
        [TestCase(-1, 2021)]
        [TestCase(32, 2021)]
        public void October_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int day, int year)
        {
            //Act
            Action action = () => day.October(year);

            //Assert 
            action.Should().Throw<ArgumentOutOfRangeException>();
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
        [TestCase(-1, 2021)]
        [TestCase(31, 2021)]
        public void November_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int day, int year)
        {
            //Act
            Action action = () => day.November(year);

            //Assert 
            action.Should().Throw<ArgumentOutOfRangeException>();
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
        [TestCase(-1, 2021)]
        [TestCase(32, 2021)]
        public void December_DayIsOutOfRange_ThrowArgumentOutOfRangeException(int day, int year)
        {
            //Act
            Action action = () => day.December(year);

            //Assert 
            action.Should().Throw<ArgumentOutOfRangeException>();
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
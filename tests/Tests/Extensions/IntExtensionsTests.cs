using FluentAssertions;
using NUnit.Framework;
using System;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class IntExtensionsTests
{
    [Test]
    [TestCase(1, 1, 3, 1, 10, 1)]
    [TestCase(2, 1, 3, 1, 10, 5)]
    [TestCase(3, 1, 3, 1, 10, 10)]
    public void Map_WhenCalled_MapAndReturnValueToNewRange(int value, int fromLow, int fromHigh, int toLow, int toHigh, int expected)
    {
        //Act
        var result = value.Map(fromLow, fromHigh, toLow, toHigh);

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
        const int year = 2021;
        const int month = 1;
        const int day = 1;
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
        const int year = 2021;
        const int month = 2;
        const int day = 1;
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
        const int year = 2021;
        const int month = 3;
        const int day = 1;
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
        const int year = 2021;
        const int month = 4;
        const int day = 1;
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
        const int year = 2021;
        const int month = 5;
        const int day = 1;
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
        const int year = 2021;
        const int month = 6;
        const int day = 1;
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
        const int year = 2021;
        const int month = 7;
        const int day = 1;
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
        const int year = 2021;
        const int month = 8;
        const int day = 1;
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
        const int year = 2021;
        const int month = 9;
        const int day = 1;
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
        const int year = 2021;
        const int month = 10;
        const int day = 1;
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
        const int year = 2021;
        const int month = 11;
        const int day = 1;
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
        const int year = 2021;
        const int month = 12;
        const int day = 1;
        var date = new DateTime(year, month, day);

        //Act
        var result = day.December(year);

        //Assert 
        result.Should().Be(date);
    }
}

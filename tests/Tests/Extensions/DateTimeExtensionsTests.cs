using CoreSharp.Enums;

namespace CoreSharp.Extensions.Tests;

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
        const DateTimeParts precision = DateTimeParts.Date | DateTimeParts.Milliseconds;
        var expected = new DateTime(1, 1, 1, 21, 34, 55, 0);

        //Act
        var result = date.Trim(precision);

        result.Should().Be(expected);
    }

    [Test]
    public void ToStringSortable_WhenCalled_ReturnSortableString()
    {
        //Arrange 
        var date = DateTime.Now;
        var expected = date.ToString("u", CultureInfo.InvariantCulture);

        //Act
        var result = date.ToStringSortable();

        //Assert
        result.Should().Be(expected);
    }

    [Test]
    public void ToStringSortable_WhenCalled_ReturnUtcSortableString()
    {
        //Arrange 
        var date = DateTime.Now;
        var expected = date.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture);

        //Act
        var result = date.ToStringSortableUtc();

        //Assert
        result.Should().Be(expected);
    }

    [Test]
    public void GetElapsedTime_EitherDateHasUnspecifiedKind_ThrowArgumentException()
    {
        //Arrange 
        var date1 = DateTime.Now;
        var date2 = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

        //Act
        Action action = () => date1.GetElapsedTime(date2);

        //Assert
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void GetElapsedTime_WhenCalled_ConvertToUtcAndReturnElapsed()
    {
        //Arrange 
        var endDate = DateTime.UtcNow.AddDays(1);
        var startDate = DateTime.Now;
        var expected = endDate.ToUniversalTime() - startDate.ToUniversalTime();

        //Act
        var result = endDate.GetElapsedTime(startDate);

        //Assert
        result.Should().Be(expected);
    }
}

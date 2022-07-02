using CoreSharp.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Globalization;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class NumberFormatInfoExtensionsTests
{
    [Test]
    public void SetCurrencyPositivePattern_InfoNull_ThrowsArgumentNullException()
    {
        //Arrange
        NumberFormatInfo info = null;

        //Act 
        Action action = () => info.SetCurrencyPositivePattern(default);

        //Assert 
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    [TestCase(CurrencyPositivePattern.NumberCurrency)]
    [TestCase(CurrencyPositivePattern.NumberSpaceCurrency)]
    public void SetCurrencyPositivePattern_WhenCalled_SetsCurrencyPositivePattern(CurrencyPositivePattern input)
    {
        //Arrange
        var info = new NumberFormatInfo();
        var expected = (int)input;

        //Act 
        info.SetCurrencyPositivePattern(input);
        var result = info.CurrencyPositivePattern;

        //Assert 
        result.Should().Be(expected);
    }

    [Test]
    public void SetCurrencyNegativePattern_InfoNull_ThrowsArgumentNullException()
    {
        //Arrange
        NumberFormatInfo info = null;

        //Act 
        Action action = () => info.SetCurrencyNegativePattern(default);

        //Assert 
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    [TestCase(CurrencyNegativePattern.SignNumberCurrency)]
    [TestCase(CurrencyNegativePattern.SignNumberSpaceCurrency)]
    public void SetCurrencyNegativePattern_WhenCalled_SetsCurrencyPositivePattern(CurrencyNegativePattern input)
    {
        //Arrange
        var info = new NumberFormatInfo();
        var expected = (int)input;

        //Act 
        info.SetCurrencyNegativePattern(input);
        var result = info.CurrencyNegativePattern;

        //Assert 
        result.Should().Be(expected);
    }

    [Test]
    public void SetNumberNegativePattern_InfoNull_ThrowsArgumentNullException()
    {
        //Arrange
        NumberFormatInfo info = null;

        //Act 
        Action action = () => info.SetNumberNegativePattern(default);

        //Assert 
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    [TestCase(NumberNegativePattern.SignNumber)]
    [TestCase(NumberNegativePattern.SignSpaceNumber)]
    public void SetNumberNegativePattern_WhenCalled_SetsCurrencyPositivePattern(NumberNegativePattern input)
    {
        //Arrange
        var info = new NumberFormatInfo();
        var expected = (int)input;

        //Act 
        info.SetNumberNegativePattern(input);
        var result = info.NumberNegativePattern;

        //Assert 
        result.Should().Be(expected);
    }
}

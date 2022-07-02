using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using Tests.Dummies.Enums;

namespace CoreSharp.Utilities.Tests;

[TestFixture]
public class EnumXTests
{
    [Test]
    public void GetValues_WhenCalled_ReturnEnumValues()
    {
        //Arrange
        var values = new[] { DummyEnum.Option1, DummyEnum.Option2, DummyEnum.Option3 };

        //Act 
        var result = EnumX.GetValues<DummyEnum>();

        //Assert
        result.Should().Equal(values);
    }

    [Test]
    public void GetDictionary_WhenCalled_ReturnEnumTextValueDictionary()
    {
        //Arrange
        var dictionary = new Dictionary<string, DummyEnum>
        {
            { $"{DummyEnum.Option1}", DummyEnum.Option1 },
            { $"{DummyEnum.Option2}", DummyEnum.Option2 },
            { $"{DummyEnum.Option3}", DummyEnum.Option3 }
        };

        //Act 
        var result = EnumX.GetDictionary<DummyEnum>();

        //Assert
        result.Should().Equal(dictionary);
    }
}

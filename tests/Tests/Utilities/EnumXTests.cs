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
        var values = new[] { DummyType.Option1, DummyType.Option2, DummyType.Option3 };

        //Act 
        var result = EnumX.GetValues<DummyType>();

        //Assert
        result.Should().Equal(values);
    }

    [Test]
    public void GetDictionary_WhenCalled_ReturnEnumTextValueDictionary()
    {
        //Arrange
        var dictionary = new Dictionary<string, DummyType>
        {
            { $"{DummyType.Option1}", DummyType.Option1 },
            { $"{DummyType.Option2}", DummyType.Option2 },
            { $"{DummyType.Option3}", DummyType.Option3 }
        };

        //Act 
        var result = EnumX.GetDictionary<DummyType>();

        //Assert
        result.Should().Equal(dictionary);
    }
}

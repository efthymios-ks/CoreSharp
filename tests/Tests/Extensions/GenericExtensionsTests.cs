﻿using Newtonsoft.Json;
using System.Text.Json;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class GenericExtensionsTests
{
    // Methods
    [Test]
    public void EqualsAny_ItemIsNull_ThrowArgumentException()
    {
        // Arrange 
        const string item = null;

        // Act 
        Action action = () => item.EqualsAny("1", "2");

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void EqualsAny_SourceIsNull_ThrowArgumentNullException()
    {
        // Arrange 
        const string item = "1";

        // Act 
        Action action = () => item.EqualsAny(null);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void EqualsAny_KeySelectorIsNull_ThrowArgumentNullException()
    {
        // Arrange 
        const string item = "1";
        var source = new[] { "1", "2" };

        // Act 
        Action action = () => item.EqualsAny<string, string>(source, null);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase(false, "-1", "1", "2")]
    [TestCase(true, "1", "1", "2")]
    public void EqualsAny_WhenCalled_ReturnTrueIfItemInSource(bool expected, string item, params string[] source)
    {
        // Act 
        var result = item.EqualsAny(source);

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    [TestCase(false, "-1", "1", "2")]
    [TestCase(true, "1", "1", "2")]
    public void EqualsAny_WhenCalledWithKeySelector_ReturnTrue(bool expected, string item, params string[] source)
    {
        // Act 
        var result = item.EqualsAny(source, i => i);

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    public void ToJson_InputIsNull_ThrowArgumentNullException()
    {
        // Arrange
        DummyClass item = null;

        // Act
        Action action = () => item.ToJson();

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void ToJson_OptionsIsNull_ThrowArgumentNullException()
    {
        // Arrange
        var item = new DummyClass();

        // Act
        Action action = () => item.ToJson(options: null);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void ToJson_WhenCalled_ReturnNewReferenceWithSameValues()
    {
        // Arrange 
        var item = new DummyClass(1, "Red");
        const string expected = /*lang=json,strict*/ @"{""Id"":1,""Name"":""Red""}";
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.None
        };

        // Act
        var result = item.ToJson(settings);

        // Assert  
        result.Should().Be(expected);
    }

    [Test]
    public void JsonClone_InputIsNull_ThrowArgumentNullException()
    {
        // Arrange
        DummyClass item = null;

        // Act
        Action action = () => item.JsonClone();

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void JsonClone_OptionsIsNull_ThrowArgumentNullException()
    {
        // Arrange
        var item = new DummyClass();

        // Act
        JsonSerializerOptions options = null;
        Action action = () => item.JsonClone(options);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void JsonClone_WhenCalled_ReturnNewReferenceWithSameValues()
    {
        // Arrange
        var item = new DummyClass(1, "Red");

        // Act
        var result = item.JsonClone();

        // Assert 
        result.Id.Should().Be(item.Id);
        result.Name.Should().Be(item.Name);
    }

    [Test]
    public void JsonEquals_OnlyLeftIsNull_ReturnFalse()
    {
        // Arrange
        DummyClass left = null;
        var right = new DummyClass();

        // Act
        var result = left.JsonEquals(right);

        // Assert 
        result.Should().BeFalse();
    }

    [Test]
    public void JsonEquals_OnlyRightIsNull_ReturnFalse()
    {
        // Arrange
        var left = new DummyClass();
        DummyClass right = null;

        // Act
        var result = left.JsonEquals(right);

        // Assert 
        result.Should().BeFalse();
    }

    [Test]
    public void JsonEquals_BothAreNull_ReturnTrue()
    {
        // Arrange
        DummyClass left = null;
        DummyClass right = null;

        // Act
        var result = left.JsonEquals(right);

        // Assert 
        result.Should().BeTrue();
    }

    [Test]
    public void JsonEquals_PropertiesDontMatch_ReturnFalse()
    {
        // Arrange 
        var left = new DummyClass(1, "Black");
        var right = new DummyClass(1, "White");

        // Act 
        var result = left.JsonEquals(right);

        // Assert 
        result.Should().BeFalse();
    }

    [Test]
    public void JsonEquals_PropertiesMatch_ReturnTrue()
    {
        // Arrange
        var left = new DummyClass(1, "Black");
        var right = new DummyClass(1, "Black");

        // Act
        var result = left.JsonEquals(right);

        // Assert 
        result.Should().BeTrue();
    }

    [Test]
    public void IsNull_ClassIsNull_ReturnTrue()
    {
        // Arrange 
        DummyClass input = null;

        // Act 
        var result = input.IsNull();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsNull_ClassHasValue_ReturnFalse()
    {
        // Arrange 
        var input = new DummyClass();

        // Act 
        var result = input.IsNull();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [TestCase(null, true)]
    [TestCase(0, false)]
    public void IsNull_StructIsNull_ReturnTrue(int? input, bool expected)
    {
        // Act 
        var result = input.IsNull();

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    [TestCase(-1, false)]
    [TestCase(0, true)]
    [TestCase(1, false)]
    [TestCase(default(int), true)]
    public void IsDefault_WhenCalled_ReturnTrueIfInputHasDefaultTypeValue(int input, bool expected)
    {
        // Act
        var result = input.IsDefault();

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    public void Yield_ArgIsNull_ReturnEmptyEnumerable()
    {
        // Arrange
        int? value = null;
        var expected = Enumerable.Empty<int?>();

        // Act
        var result = value.Yield();

        // Assert
        result.Should().Equal(expected);
    }

    [Test]
    public void Yield_WhenCalled_ReturnEnumerable()
    {
        // Arrange
        const int value = 0;
        var expected = new[] { 0 };

        // Act
        var result = value.Yield();

        // Assert
        result.Should().Equal(expected);
    }

    [Test]
    public void GetInnermostField_EntityIsNull_ThrowArgumentNullException()
    {
        // Arange
        Exception exception = null;

        // Act
        Action action = () => exception.GetInnermostField(e => e.InnerException);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void GetInnermostField_FieldSelectorIsNull_ThrowArgumentNullException()
    {
        // Arange
        var exception = new Exception();

        // Act
        Action action = () => exception.GetInnermostField(null);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void GetInnermostField_WhenCalled_ReturnInnermostField()
    {
        // Arange
        var exception1 = new Exception("1");
        var exception2 = new Exception("2", exception1);

        // Act
        var result = exception2.GetInnermostField(e => e.InnerException);

        // Assert
        result.Should().BeSameAs(exception1);
    }
}

using Tests.Internal.Dummies.Enums;

namespace CoreSharp.Utilities.Tests;

[TestFixture]
public class EnumUtilsTests
{
    [Test]
    public void GetValues_WhenCalled_ReturnEnumValues()
    {
        // Arrange
        var expectedValues = new[] { DummyType.Option1, DummyType.Option2, DummyType.Option3 };

        // Act 
        var values = EnumUtils.GetValues<DummyType>();

        // Assert
        values.Should().Equal(expectedValues);
    }

    [Test]
    public void GetDictionary_WhenCalled_ReturnEnumTextValueDictionary()
    {
        // Arrange
        var expectedDictionary = new Dictionary<string, DummyType>
        {
            { $"{DummyType.Option1}", DummyType.Option1 },
            { $"{DummyType.Option2}", DummyType.Option2 },
            { $"{DummyType.Option3}", DummyType.Option3 }
        };

        // Act 
        var dictionary = EnumUtils.GetDictionary<DummyType>();

        // Assert
        dictionary.Should().Equal(expectedDictionary);
    }
}

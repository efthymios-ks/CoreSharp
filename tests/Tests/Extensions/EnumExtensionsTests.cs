using Tests.Internal.Dummies.Enums;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class EnumExtensionsTests
{
    [Test]
    public void GetDisplayName_WhenCalled_ReturnEnumDisplayAttribute()
    {
        // Arrange 
        const DummyType value = DummyType.Option1;
        const string expectedName = "Option 1 Name";

        // Act 
        var name = value.GetDisplayAttributeName();

        name.Should().Be(expectedName);
    }

    [Test]
    public void GetDisplayShortName_WhenCalled_ReturnEnumDisplayAttribute()
    {
        // Arrange 
        const DummyType value = DummyType.Option1;
        const string expectedShortName = "Option 1 ShortName";

        // Act 
        var shortName = value.GetDisplayAttributeShortName();

        shortName.Should().Be(expectedShortName);
    }

    [Test]
    public void GetDisplayDescription_WhenCalled_ReturnEnumDisplayAttribute()
    {
        // Arrange 
        const DummyType value = DummyType.Option1;
        const string expectedDescription = "Option 1 Description";

        // Act 
        var description = value.GetDisplayAttributeDescription();

        description.Should().Be(expectedDescription);
    }
}

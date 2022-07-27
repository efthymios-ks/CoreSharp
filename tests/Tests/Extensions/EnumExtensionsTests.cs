using Tests.Dummies.Enums;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class EnumExtensionsTests
{
    [Test]
    public void GetDisplayName_WhenCalled_ReturnEnumDisplayAttribute()
    {
        //Arrange 
        const DummyType value = DummyType.Option1;
        const string expected = "Option 1 Name";

        //Act 
        var result = value.GetDisplayName();

        result.Should().Be(expected);
    }

    [Test]
    public void GetDisplayShortName_WhenCalled_ReturnEnumDisplayAttribute()
    {
        //Arrange 
        const DummyType value = DummyType.Option1;
        const string expected = "Option 1 ShortName";

        //Act 
        var result = value.GetDisplayShortName();

        result.Should().Be(expected);
    }

    [Test]
    public void GetDisplayDescription_WhenCalled_ReturnEnumDisplayAttribute()
    {
        //Arrange 
        const DummyType value = DummyType.Option1;
        const string expected = "Option 1 Description";

        //Act 
        var result = value.GetDisplayDescription();

        result.Should().Be(expected);
    }
}

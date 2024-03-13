namespace CoreSharp.Extensions.Tests;

public sealed class GuidExtensionsTests
{
    [Test]
    public void IsNullOrEmpty_WhenGuidIsEmpty_ShouldReturnTrue()
    {
        // Arrange
        var guid = Guid.Empty;

        // Act
        var result = guid.IsNullOrEmpty();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsNullOrEmpty_WhenGuidIsNotEmpty_ShouldReturnFalse()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var result = guid.IsNullOrEmpty();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void IsNullOrEmpty_WhenNullableGuidIsNull_ShouldReturnTrue()
    {
        // Arrange
        Guid? guid = null;

        // Act
        var result = guid.IsNullOrEmpty();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsNullOrEmpty_WhenNullableGuidIsEmpty_ShouldReturnTrue()
    {
        // Arrange
        Guid? guid = Guid.Empty;

        // Act
        var result = guid.IsNullOrEmpty();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsNullOrEmpty_WhenNullableGuidIsNotEmpty_ShouldReturnFalse()
    {
        // Arrange
        Guid? guid = Guid.NewGuid();

        // Act
        var result = guid.IsNullOrEmpty();

        // Assert
        result.Should().BeFalse();
    }
}

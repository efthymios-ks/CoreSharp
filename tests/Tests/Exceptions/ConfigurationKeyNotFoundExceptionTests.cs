namespace CoreSharp.Exceptions.Tests;

[TestFixture]
public sealed class ConfigurationKeyNotFoundExceptionTests
{
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void Constructor_WhenKeyIsNull_ShouldThrowArgumentException(string key)
    {
        // Act
        Action action = () => _ = new ConfigurationKeyNotFoundException(key);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_WhenKeyIsNotNull_ShouldSetExceptionMessage()
    {
        // Arrange
        var key = "test_key";

        // Act
        var exception = new ConfigurationKeyNotFoundException(key);

        // Assert
        exception.Message.Should().Be($"Could not find configuration entry for key=`{key}`.");
    }
}

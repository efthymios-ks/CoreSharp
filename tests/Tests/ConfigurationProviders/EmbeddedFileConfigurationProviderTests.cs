using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using NSubstitute;
using System.IO;

namespace CoreSharp.ConfigurationProviders.Tests;

[TestFixture]
public class EmbeddedFileConfigurationProviderTests
{
    [Test]
    public void Constructor_WhenBuilderIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IConfigurationBuilder builder = null;
        var options = new EmbeddedFileConfigurationOptions();

        // Act
        Action action = () => _ = new EmbeddedFileConfigurationProvider(builder, options);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenOptionsIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var builder = new ConfigurationBuilder();
        EmbeddedFileConfigurationOptions options = null;

        // Act
        Action action = () => _ = new EmbeddedFileConfigurationProvider(builder, options);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenScanAssemblyIsNull_ShouldThrowArgumentException()
    {
        // Arrange
        var builder = new ConfigurationBuilder();
        var options = new EmbeddedFileConfigurationOptions
        {
            ScanAssembly = null
        };

        // Act
        Action action = () => _ = new EmbeddedFileConfigurationProvider(builder, options);

        // Assert
        action.Should().Throw<ArgumentException>();
    }
}

namespace CoreSharp.Utilities.Tests;

[TestFixture]
public class FileUtilsTests
{
    [Test]
    public void SanitizeFilePath_WhenFilePathIsNull_ShouldReturnNull()
    {
        // Arrange
        const string filePath = null;

        // Act
        Action action = () => FileUtils.SanitizeFileName(filePath);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase("file1.txt", "file1.txt")]
    [TestCase("file_2.txt", "file_2.txt")]
    [TestCase("<file>3.txt", "_file_3.txt")]
    [TestCase("file?4.txt", "file_4.txt")]
    public void SanitizeFileName_WhenCalled_ShouldReplaceInvalidCharsWithUnderscore(string fileName, string expectedSanitizedFileName)
    {
        // Act
        var sanitizedFileName = FileUtils.SanitizeFileName(fileName);

        // Assert
        sanitizedFileName.Should().Be(expectedSanitizedFileName);
    }
}
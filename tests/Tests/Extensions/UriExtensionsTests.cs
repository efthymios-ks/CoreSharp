namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class UriExtensionsTests
{
    // Fields 
    private readonly Uri _uriNull;

    [Test]
    public void GetQueryParameters_UriIsNull_ThrowArgumentNullException()
    {
        // Act
        Action action = () => _uriNull.GetQueryParameters();

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void GetQueryParameters_WhenCalled_ReturnDictionaryWithQueryParameters()
    {
        // Arrange
        const string url = "https://example.com/?name=efthymios&color=black";
        var uri = new Uri(url);
        var expectedParameters = new Dictionary<string, string>
        {
            { "name", "efthymios" },
            { "color", "black" }
        };

        // Act
        var parameters = uri.GetQueryParameters();

        // Assert 
        parameters.Should().Equal(expectedParameters);
    }

    [Test]
    public void GetFragmentParameters_UriIsNull_ThrowArgumentNullException()
    {
        // Act
        Action action = () => _uriNull.GetFragmentParameters();

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void GetFragmentParameters_WhenCalled_ReturnDictionaryWithFragmentParameters()
    {
        // Arrange
        const string url = "https://example.com/route#name=efthymios&color=black";
        var uri = new Uri(url);
        var expectedParameters = new Dictionary<string, string>
        {
            { "name", "efthymios" },
            { "color", "black" }
        };

        // Act
        var parameters = uri.GetFragmentParameters();

        // Assert 
        parameters.Should().Equal(expectedParameters);
    }
}

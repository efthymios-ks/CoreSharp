namespace CoreSharp.Utilities.Tests;

[TestFixture]
public class UriXTests
{
    [Test]
    public void JoinSegments_SegmentsIsNull_ThrowArgumentNullException()
    {
        //Act
        Action action = () => UriX.JoinSegments(segments: null);

        //Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase("http://google.com/path1/path2/", "http://google.com", "//path1//", "/path2/")]
    [TestCase("https://google.com/path1/path2/", "https://google.com", "//path1//", "/path2/")]
    [TestCase("https://google.com/path1/path2/", "https:///google.com//", "///path1///", "//path2//")]
    public void JoinSegments_WhenCalled_ReturnUnifiedUrlWithTrimmedSlashes(string expected, params string[] segments)
    {
        //Act
        var result = UriX.JoinSegments(segments);

        //Assert 
        result.Should().Be(expected);
    }

    [Test]
    public void Build_BaseUrlIsNullOrWhiteSpace_ThrowArgumentNullException()
    {
        //Arrange
        const string baseUrl = null;
        var parameters = new Dictionary<string, string>
        {
            { "name", "efthymios" },
            { "color", "black" }
        };

        //Act
        Action action = () => UriX.Build(baseUrl, parameters);

        //Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Build_ParametersIsNull_ThrowArgumentNullException()
    {
        //Arrange
        const string baseUrl = "https://example.com/";
        IDictionary<string, string> parameters = null;

        //Act
        Action action = () => UriX.Build(baseUrl, parameters);

        //Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Build_WhenCalled_ReturnQueryString()
    {
        //Arrange
        const string baseUrl = "https://example.com/";
        var parameters = new Dictionary<string, object>
        {
            { "name", "Efthymios Koktsidis" },
            { "count", 10 }
        };
        const string expected = "https://example.com/?name=Efthymios%20Koktsidis&count=10";

        //Act
        var result = UriX.Build(baseUrl, parameters);

        //Assert
        result.Should().Be(expected);
    }
}

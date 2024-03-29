﻿namespace CoreSharp.Utilities.Tests;

[TestFixture]
public class UriUtilsTests
{
    [Test]
    public void JoinSegments_SegmentsIsNull_ThrowArgumentNullException()
    {
        // Act
        Action action = () => Uritils.JoinSegments(segments: null);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase("http://google.com/path1/path2/", "http://google.com", "path1", "path2")]
    [TestCase("http://google.com/path1/path2/", "http://google.com", "//path1//", "/path2/")]
    [TestCase("https://google.com/path1/path2/", "https://google.com", "//path1//", "/path2/")]
    [TestCase("https://google.com/path1/path2/", "https:///google.com//", "///path1///", "//path2//")]
    public void JoinSegments_WhenCalled_ReturnUnifiedUrlWithTrimmedSlashes(string expectedUrl, params string[] segments)
    {
        // Act
        var url = Uritils.JoinSegments(segments);

        // Assert 
        url.Should().Be(expectedUrl);
    }

    [Test]
    public void Build_BaseUrlIsNullOrWhiteSpace_ThrowArgumentNullException()
    {
        // Arrange
        const string baseUrl = null;
        var parameters = new Dictionary<string, string>
        {
            { "name", "efthymios" },
            { "color", "black" }
        };

        // Act
        Action action = () => Uritils.Build(baseUrl, parameters);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Build_ParametersIsNull_ThrowArgumentNullException()
    {
        // Arrange
        const string baseUrl = "https://example.com/";
        IDictionary<string, string> parameters = null;

        // Act
        Action action = () => Uritils.Build(baseUrl, parameters);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Build_BaseUrlHasNoQuery_MergeAndReturnBaseAndQueryString()
    {
        // Arrange
        const string baseUrl = "https://example.com/";
        var parameters = new Dictionary<string, object>
        {
            { "name", "Efthymios Koktsidis" },
            { "count", 10 }
        };
        const string expectedUrl = "https://example.com/?name=Efthymios%20Koktsidis&count=10";

        // Act
        var url = Uritils.Build(baseUrl, parameters);

        // Assert
        url.Should().Be(expectedUrl);
    }

    [Test]
    public void Build_BaseUrlHasUniqueQuery_MergeAndReturnBaseUrlAndQueryWithProvidedQueryString()
    {
        // Arrange
        const string baseUrl = "https://example.com?age=28";
        var parameters = new Dictionary<string, object>
        {
            { "name", "Efthymios Koktsidis" },
            { "count", 10 }
        };
        const string expectedUrl = "https://example.com/?age=28&name=Efthymios%20Koktsidis&count=10";

        // Act
        var url = Uritils.Build(baseUrl, parameters);

        // Assert
        url.Should().Be(expectedUrl);
    }

    [Test]
    public void Build_BaseUrlHasCommonQueryWithProvided_OverrideProvidedQueryOntoBaseAndMergeReturn()
    {
        // Arrange
        const string baseUrl = "https://example.com?age=28";
        var parameters = new Dictionary<string, object>
        {
            { "name", "Efthymios Koktsidis" },
            { "count", 10 },
            { "age", 30 }
        };
        const string expectedUrl = "https://example.com/?age=30&name=Efthymios%20Koktsidis&count=10";

        // Act
        var url = Uritils.Build(baseUrl, parameters);

        // Assert
        url.Should().Be(expectedUrl);
    }
}

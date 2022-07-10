using CoreSharp.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.Extensions;

[TestFixture]
public class UriExtensionsTests
{
    //Fields 
    private readonly Uri _uriNull = null;

    [Test]
    public void GetQueryParameters_UriIsNull_ThrowArgumentNullException()
    {
        //Act
        Action action = () => _uriNull.GetQueryParameters();

        //Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void GetQueryParameters_WhenCalled_ReturnDictionaryWithQueryParameters()
    {
        //Arrange
        const string url = "https://example.com/?name=efthymios&color=black";
        var uri = new Uri(url);
        var expected = new Dictionary<string, string>
        {
            { "name", "efthymios" },
            { "color", "black" }
        };

        //Act
        var result = uri.GetQueryParameters();

        //Assert 
        result.Should().Equal(expected);
    }

    [Test]
    public void GetFragmentParameters_UriIsNull_ThrowArgumentNullException()
    {
        //Act
        Action action = () => _uriNull.GetFragmentParameters();

        //Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void GetFragmentParameters_WhenCalled_ReturnDictionaryWithFragmentParameters()
    {
        //Arrange
        const string url = "https://example.com/route#name=efthymios&color=black";
        var uri = new Uri(url);
        var expected = new Dictionary<string, string>
        {
            { "name", "efthymios" },
            { "color", "black" }
        };

        //Act
        var result = uri.GetFragmentParameters();

        //Assert 
        result.Should().Equal(expected);
    }
}

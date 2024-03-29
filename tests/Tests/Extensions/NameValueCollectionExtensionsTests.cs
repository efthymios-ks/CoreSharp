﻿using System.Collections.Specialized;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class NameValueCollectionExtensionsTests
{
    // Fields
    private readonly NameValueCollection _sourceNull;

    // Methods 
    [Test]
    public void ToUrlQueryString_ParametersInNull_ThrowArgumentNullException()
    {
        // Act
        Action action = () => _sourceNull.ToUrlQueryString();

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void ToUrlQueryString_WhenCalled_ReturnQueryString()
    {
        // Arrange
        var parameters = new NameValueCollection
        {
            { "name", "Efthymios Koktsidis" },
            { "color", "Black" },
            { "count", "10" }
        };
        const string expected = "?name=Efthymios%20Koktsidis&color=Black&count=10";

        // Act
        var result = parameters.ToUrlQueryString();

        // Assert
        result.Should().Be(expected);
    }
}

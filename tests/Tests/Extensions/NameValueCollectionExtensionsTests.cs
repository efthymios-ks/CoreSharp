using CoreSharp.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Specialized;

namespace Tests.Extensions;

[TestFixture]
public class NameValueCollectionExtensionsTests
{
    //Fields
    private readonly NameValueCollection _sourceNull = null;

    //Methods 
    [Test]
    public void ToUrlQueryString_ParametersInNull_ThrowArgumentNullException()
    {
        //Act
        Action action = () => _sourceNull.ToUrlQueryString();

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void ToUrlQueryString_WhenCalled_ReturnQueryString()
    {
        //Arrange
        var parameters = new NameValueCollection
        {
            { "name", "Efthymios Koktsidis" },
            { "color", "Black" },
            { "count", "10" }
        };
        const string expected = "?name=Efthymios%20Koktsidis&color=Black&count=10";

        //Act
        var result = parameters.ToUrlQueryString();

        //Assert
        result.Should().Be(expected);
    }
}

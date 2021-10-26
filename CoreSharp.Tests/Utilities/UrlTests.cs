using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CoreSharp.Utilities.Tests
{
    [TestFixture]
    public class UrlTests
    {
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
            Action action = () => Url.Build(baseUrl, parameters);

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
            Action action = () => Url.Build(baseUrl, parameters);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Build_EncodeIsFalse_BuildAndReturnUrlWithParametersAsIs()
        {
            //Arrange
            const string baseUrl = "https://example.com/";
            var parameters = new Dictionary<string, object>
            {
                { "name", "Efthymios Koktsidis" },
                { "count", 10 }
            };
            const bool encode = false;
            const string expected = "https://example.com/?name=Efthymios Koktsidis&count=10";

            //Act
            var result = Url.Build(baseUrl, parameters, encode);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void Build_EncodeIsTrue_BuildAndReturnUrlWithEncodedParameters()
        {
            //Arrange
            const string baseUrl = "https://example.com/";
            var parameters = new Dictionary<string, object>
            {
                { "name", "Efthymios Koktsidis" },
                { "count", 10 }
            }; 
            const string expected = "https://example.com/?name=Efthymios+Koktsidis&count=10";

            //Act
            var result = Url.Build(baseUrl, parameters);

            //Assert
            result.Should().Be(expected);
        }
    }
}

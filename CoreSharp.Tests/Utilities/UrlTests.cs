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
            string baseUrl = null;
            var parameters = new Dictionary<string, string>()
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
            string baseUrl = @"https://example.com/";
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
            string baseUrl = @"https://example.com/";
            var parameters = new Dictionary<string, object>()
            {
                { "name", "Efthymios Koktsidis" },
                { "count", 10 }
            };
            bool encode = false;
            string expected = @"https://example.com/?name=Efthymios Koktsidis&count=10";

            //Act
            var result = Url.Build(baseUrl, parameters, encode);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void Build_EncodeIsTrue_BuildAndReturnUrlWithEncodedParameters()
        {
            //Arrange
            string baseUrl = @"https://example.com/";
            var parameters = new Dictionary<string, object>()
            {
                { "name", "Efthymios Koktsidis" },
                { "count", 10 }
            };
            bool encode = true;
            string expected = @"https://example.com/?name=Efthymios+Koktsidis&count=10";

            //Act
            var result = Url.Build(baseUrl, parameters, encode);

            //Assert
            result.Should().Be(expected);
        }
    }
}
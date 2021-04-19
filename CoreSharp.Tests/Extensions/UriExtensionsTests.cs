using CoreSharp.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class UriExtensionsTests
    {
        //Fields 
        private readonly Uri uriNull = null;

        [Test]
        public void GetQueryParameters_UriIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => uriNull.GetQueryParameters();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetQueryParameters_WhenCalled_ReturnDictionaryWithQueryParameters()
        {
            //Arrange
            string url = @"https://example.com/?name=efthymios&color=black";
            var uri = new Uri(url);
            var expected = new Dictionary<string, string>()
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
        public void BuildUri_BaseUrlIsNullOrWhiteSpace_ThrowArgumentNullException()
        {
            //Arrange
            string baseUrl = null;
            var parameters = new Dictionary<string, string>()
            {
                { "name", "efthymios" },
                { "color", "black" }
            };

            //Act
            Action action = () => UriExtensions.BuildUri(baseUrl, parameters);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void BuildUri_ParametersIsNull_ThrowArgumentNullException()
        {
            //Arrange
            string baseUrl = @"https://example.com/";
            IDictionary<string, string> parameters = null;

            //Act
            Action action = () => UriExtensions.BuildUri(baseUrl, parameters);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void BuildUri_EncodeIsFalse_BuildAndReturnUrlWithParametersAsIs()
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
            var result = UriExtensions.BuildUri(baseUrl, parameters, encode);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void BuildUri_EncodeIsTrue_BuildAndReturnUrlWithEncodedParameters()
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
            var result = UriExtensions.BuildUri(baseUrl, parameters, encode);

            //Assert
            result.Should().Be(expected);
        }
    }
}
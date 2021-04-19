using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using CoreSharp.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class NameValueCollectionExtensionsTests
    {
        //Fields
        private readonly NameValueCollection sourceNull = null;

        //Methods 
        [Test]
        public void ToUrlQueryString_ParametersInNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => sourceNull.ToUrlQueryString();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToUrlQueryString_EncodeIsFalse_ReturnQueryStringWithValuesAsIs()
        {
            //Arrange
            var parameters = new NameValueCollection();
            parameters.Add("name", "Efthymios Koktsidis");
            parameters.Add("color", "Black");
            parameters.Add("count", $"10");
            bool encode = false;
            string expected = "name=Efthymios Koktsidis&color=Black&count=10";

            //Act
            var result = parameters.ToUrlQueryString(encode);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ToUrlQueryString_EncodeIsTrue_ReturnQueryStringWithEncodedValues()
        {
            //Arrange
            var parameters = new NameValueCollection();
            parameters.Add("name", "Efthymios Koktsidis");
            parameters.Add("color", "Black");
            parameters.Add("count", $"10");
            bool encode = true;
            string expected = "name=Efthymios+Koktsidis&color=Black&count=10";

            //Act
            var result = parameters.ToUrlQueryString(encode);

            //Assert
            result.Should().Be(expected);
        }
    }
}
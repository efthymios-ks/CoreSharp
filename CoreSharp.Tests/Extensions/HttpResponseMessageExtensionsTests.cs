using CoreSharp.Models;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class HttpResponseMessageExtensionsTests
    {
        [Test]
        public void EnsureSuccessAsync_ResponseIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            HttpResponseMessage response = null;

            //Act
            Func<Task> action = async () => await response.EnsureSuccessAsync();

            //Assert
            action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Test]
        public void EnsureSuccessAsync_ResponseStatusIsOk_DoNothing()
        {
            //Arrange 
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            //Act
            Func<Task> action = async () => await response.EnsureSuccessAsync();

            //Assert
            action.Should().NotThrowAsync();
        }

        [Test]
        public void EnsureSuccessAsync_ResponseStatusHasError_ThrowHttpResponseException()
        {
            //Arrange 
            var statusCode = HttpStatusCode.BadRequest;
            string contentValue = "Content message";
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(contentValue)
            };

            //Act
            Func<Task> action = async () => await response.EnsureSuccessAsync();

            //Assert
            var exception = action.Should().ThrowExactlyAsync<HttpResponseException>().GetAwaiter().GetResult().Which;
            exception.StatusCode.Should().Be(statusCode);
            exception.Message.Should().Be(contentValue);
        }
    }
}
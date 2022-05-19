using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Tests.Dummies.Entities;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class StreamExtensionsTests
    {
        //Fields
        private readonly Stream _streamNull;

        [Test]
        public void FromJson_OptionsIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => _streamNull.FromJson<DummyClass>();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public async Task FromJson_WhenCalled_MapItemPropertiesAndReturnTrue()
        {
            //Arrange
            var dummy = new DummyClass(1, "Black");
            await using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, dummy);

            //Act
            var result = stream.FromJson<DummyClass>();

            //Assert 
            result.Id.Should().Be(dummy.Id);
            result.Name.Should().Be(dummy.Name);
        }

        [Test]
        public async Task ToArrayAsync_StreamIsNull_ThrowArgumentNullException()
        {
            //Act
            Func<Task> action = () => _streamNull.ToArrayAsync();

            //Assert
            await action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Test]
        public async Task ToArrayAsync_WhenCalled_ConvertToByteArray()
        {
            //Arrange
            var byteArray = new byte[] { 1, 2, 3 };
            await using var stream = new MemoryStream(byteArray);

            //Act
            var result = await stream.ToArrayAsync();

            //Assert 
            result.Should().Equal(byteArray);
        }
    }
}

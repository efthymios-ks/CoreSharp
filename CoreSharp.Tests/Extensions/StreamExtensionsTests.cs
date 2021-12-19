using CoreSharp.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class StreamExtensionsTests
    {
        //Fields
        private readonly Stream _streamNull = null;

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
    }
}

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
        private readonly Stream StreamNull = null;

        [Test]
        public void ToEntity_OptionsIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StreamNull.ToEntity<DummyClass>();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public async Task ToEntity_WhenCalled_MapItemPropertiesAndReturnTrue()
        {
            //Arrange
            var dummy = new DummyClass(1, "Black");
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, dummy);

            //Act
            var result = stream.ToEntity<DummyClass>();

            //Assert 
            result.Id.Should().Be(dummy.Id);
            result.Name.Should().Be(dummy.Name);
        }
    }
}
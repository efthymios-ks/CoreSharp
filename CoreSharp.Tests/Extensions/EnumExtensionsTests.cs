using CoreSharp.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class EnumExtensionsTests
    {
        [Test]
        public void GetDisplayName_WhenCalled_ReturnEnumDisplayAttribute()
        {
            //Arrange 
            const DummyEnum Value = DummyEnum.Option1;
            const string Expected = "Option 1 Name";

            //Act 
            var result = Value.GetDisplayName();

            result.Should().Be(Expected);
        }

        [Test]
        public void GetDisplayShortName_WhenCalled_ReturnEnumDisplayAttribute()
        {
            //Arrange 
            const DummyEnum Value = DummyEnum.Option1;
            const string Expected = "Option 1 ShortName";

            //Act 
            var result = Value.GetDisplayShortName();

            result.Should().Be(Expected);
        }

        [Test]
        public void GetDisplayDescription_WhenCalled_ReturnEnumDisplayAttribute()
        {
            //Arrange 
            const DummyEnum Value = DummyEnum.Option1;
            const string Expected = "Option 1 Description";

            //Act 
            var result = Value.GetDisplayDescription();

            result.Should().Be(Expected);
        }
    }
}

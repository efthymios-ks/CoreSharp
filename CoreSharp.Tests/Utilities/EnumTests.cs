using CoreSharp.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace CoreSharp.Utilities.Tests
{
    [TestFixture]
    public class EnumTests
    {
        [Test]
        public void GetValues_WhenCalled_ReturnEnumValues()
        {
            //Arrange
            var values = new[] { DummyEnum.Option1, DummyEnum.Option2, DummyEnum.Option3 };

            //Act 
            var result = Enum.GetValues<DummyEnum>();

            //Assert
            result.Should().Equal(values);
        }

        [Test]
        public void GetDictionary_WhenCalled_ReturnEnumTextValueDictionary()
        {
            //Arrange
            var dictionary = new Dictionary<string, DummyEnum>
            {
                { $"{DummyEnum.Option1}", DummyEnum.Option1 },
                { $"{DummyEnum.Option2}", DummyEnum.Option2 },
                { $"{DummyEnum.Option3}", DummyEnum.Option3 }
            };

            //Act 
            var result = Enum.GetDictionary<DummyEnum>();

            //Assert
            result.Should().Equal(dictionary);
        }
    }
}

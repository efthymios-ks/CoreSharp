using System;
using System.Collections.Generic;
using CoreSharp.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class EnumExtensionsTests
    {
        [Test]
        public void GetValues_TypeIsNotEnum_ThrowArgumentException()
        {
            //Act 
            Action action = () => EnumExtensions.GetValues<DummyNotAnEnum>();

            //Assert
            action.Should().ThrowExactly<ArgumentException>();
        }

        [Test]
        public void GetValues_WhenCalled_ReturnEnumValues()
        {
            //Arrange
            var values = new[] { DummyEnum.Option1, DummyEnum.Option2, DummyEnum.Option3 };

            //Act 
            var result = EnumExtensions.GetValues<DummyEnum>();

            //Assert
            result.Should().Equal(values);
        }

        [Test]
        public void GetDictionary_TypeIsNotEnum_ThrowArgumentException()
        {
            //Act 
            Action action = () => EnumExtensions.GetDictionary<DummyNotAnEnum>();

            //Assert
            action.Should().ThrowExactly<ArgumentException>();
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
            var result = EnumExtensions.GetDictionary<DummyEnum>();

            //Assert
            result.Should().Equal(dictionary);
        }

        [Test]
        public void GetDescription_TypeIsNotEnum_ThrowArgumentException()
        {
            //Arrange 
            var item = new DummyNotAnEnum();

            //Act 
            Action action = () => item.GetDescription();

            //Assert
            action.Should().ThrowExactly<ArgumentException>();
        }

        [Test]
        public void GetDescription_WhenCalled_ReturnEnumDescriptionAttribute()
        {
            //Arrange 
            var item = DummyEnum.Option1;
            string expected = "Description 1";

            //Act 
            var result = item.GetDescription();

            result.Should().Be(expected);
        }

        [Test]
        public void GetDisplayName_TypeIsNotEnum_ThrowArgumentException()
        {
            //Arrange 
            var item = new DummyNotAnEnum();

            //Act 
            Action action = () => item.GetDisplayName();

            //Assert
            action.Should().ThrowExactly<ArgumentException>();
        }

        [Test]
        public void GetDisplayName_WhenCalled_ReturnEnumDisplayAttribute()
        {
            //Arrange 
            var item = DummyEnum.Option1;
            string expected = "Option 1";

            //Act 
            var result = item.GetDisplayName();

            result.Should().Be(expected);
        }
    }
}
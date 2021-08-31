using CoreSharp.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class EnumExtensionsTests
    {
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
using NUnit.Framework;
using System;
using FluentAssertions;
using CoreSharp.Tests.Dummies;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class GenericExtensionsTests
    {
        //Methods
        [Test]
        public void IsIn_ItemIsNull_ThrowArgumentException()
        {
            //Arrange 
            string item = null;

            //Act 
            Action action = () => item.IsIn("1", "2");

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void IsIn_SourceIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            string item = "1";

            //Act 
            Action action = () => item.IsIn(null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetPropertyName_PropertySelectorIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            var item = new DummyClass();

            //Act
            Action action = () => item.GetPropertyName<DummyClass, int>(null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetPropertyName_WhenCalled_ReturnPropertyName()
        {
            //Arrange 
            var item = new DummyClass();

            //Act
            var result = item.GetPropertyName(i => i.Id);

            //Assert 
            result.Should().Be(nameof(item.Id));
        }

        [Test]
        public void JsonClone_InputIsNull_ThrowArgumentNullException()
        {
            //Arrange
            DummyClass item = null;

            //Act
            Action action = () => item.JsonClone();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void JsonClone_WhenCalled_ReturnNewReferenceWithSameValues()
        {
            //Arrange
            var item = new DummyClass(1, "Red");

            //Act
            var result = item.JsonClone();

            //Assert 
            result.Id.Should().Be(item.Id);
            result.Name.Should().Be(item.Name);
        }
    }
}
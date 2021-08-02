using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class ExpandoObjectExtensionsTests
    {
        [Test]
        public void ToDictionary_InputIsNull_ThrowNewArgumentNullException()
        {
            //Arrange 
            ExpandoObject expando = null;

            //Act 
            Action action = () => expando.ToDictionary();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToDictionary_WhenCalled_ReturnDictionary()
        {
            //Arrange 
            var source = new Dictionary<string, object>();
            source.Add("id", 1);
            source.Add("color", "black");
            var expando = new ExpandoObject() as IDictionary<string, object>;
            foreach (var item in source)
                expando.Add(item);

            //Act 
            var dictionary = ((ExpandoObject)expando).ToDictionary();

            //Assert
            dictionary.Should().Equal(source);
        }
    }
}
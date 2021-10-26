﻿using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace CoreSharp.Utilities.Tests
{
    [TestFixture]
    public class StringTests
    {
        [Test]
        public void FirstNotEmpty_WhenCalled_ReturnFirstNotEmpty()
        {
            //Arrange 
            var emptyValues = new[] { null, string.Empty, "", " " };
            const string expected = "1";
            var values = emptyValues.Append(expected).Concat(emptyValues);

            //Act 
            var result = String.FirstNotEmpty(values);

            //Assert 
            result.Should().Be(expected);
        }
    }
}

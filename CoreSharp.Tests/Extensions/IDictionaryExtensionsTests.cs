using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class IDictionaryExtensionsTests
    {
        //Fields
        private readonly IDictionary<string, int> dictionaryNull = null;
        private readonly IDictionary<string, int> dictionary = new Dictionary<string, int>();

        //Methods 
        [SetUp]
        public void SetUp()
        {
            dictionary.Clear();
            dictionary.Add("1", 1);
            dictionary.Add("2", 2);
            dictionary.Add("3", 3);
        }

        [Test]
        public void TryGet_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => dictionaryNull.TryGet("1", out _);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void TryGet_KeyNotFound_ReturnFalse()
        {
            //Act 
            var result = dictionary.TryGet("-1", out _);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void TryGet_KeyFound_ReturnTrueAndValue()
        {
            //Act 
            var result = dictionary.TryGet("1", out int value);

            //Assert
            result.Should().BeTrue();
            value.Should().Be(1);
        }

        [Test]
        public void TryAdd_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => dictionaryNull.TryAdd("1", 1);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void TryAdd_KeyFound_ReturnFalseAndAddNothing()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var entry in dictionary)
                expected.Add(entry.Key, entry.Value);

            //Act 
            var added = dictionary.TryAdd("1", 1);

            //Assert
            added.Should().BeFalse();
            dictionary.Should().Equal(expected);
        }

        [Test]
        public void TryAdd_KeyNotFound_ReturnTrueAndAddValue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var entry in dictionary)
                expected.Add(entry.Key, entry.Value);
            expected.Add("4", 4);

            //Act 
            var added = dictionary.TryAdd("4", 4);

            //Assert
            added.Should().BeTrue();
            dictionary.Should().Equal(expected);
        }

        [Test]
        public void TryRemove_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => dictionaryNull.TryRemove("1", out _);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void TryRemove_KeyNotFound_ReturnFalseAndRemoveNothing()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var entry in dictionary)
                expected.Add(entry.Key, entry.Value);

            //Act 
            var result = dictionary.TryRemove("-1", out _);

            //Assert
            result.Should().BeFalse();
            dictionary.Should().Equal(expected);
        }

        [Test]
        public void TryRemove_KeyFound_RemoveAndReturnValueAndTrue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var entry in dictionary)
                expected.Add(entry.Key, entry.Value);
            expected.Remove("3");

            //Act 
            var removed = dictionary.TryRemove("3", out int value);

            //Assert
            removed.Should().BeTrue();
            value.Should().Be(3);
            dictionary.Should().Equal(expected);
        }

        [Test]
        public void TryUpdate_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => dictionaryNull.TryUpdate("-1", 100);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void TryUpdate_UpdateActionIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            Func<string, int, int> updateAction = null;

            //Act 
            Action action = () => dictionary.TryUpdate("1", updateAction);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void TryUpdate_KeyNotFound_ReturnFalse()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var entry in dictionary)
                expected.Add(entry.Key, entry.Value);

            //Act 
            var result = dictionary.TryUpdate("-1", 100);

            //Assert
            result.Should().BeFalse();
            dictionary.Should().Equal(expected);
        }

        [Test]
        public void TryUpdate_KeyFoundAndUpdateIsValue_ReturnTrueAndUpdateValue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var entry in dictionary)
                expected.Add(entry.Key, entry.Value);
            expected["1"] = 100;

            //Act 
            var result = dictionary.TryUpdate("1", 100);

            //Assert
            result.Should().BeTrue();
            dictionary.Should().Equal(expected);
        }

        [Test]
        public void TryUpdate_KeyFoundAndUpdateIsFunction_ReturnTrueAndUpdateValue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var entry in dictionary)
                expected.Add(entry.Key, entry.Value);
            static int updateAction(string key, int value) => value = value * 2;
            expected["1"] = updateAction("1", 1);

            //Act 
            var result = dictionary.TryUpdate("1", updateAction);

            //Assert
            result.Should().BeTrue();
            dictionary.Should().Equal(expected);
        }
    }
}
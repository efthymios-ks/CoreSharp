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
            static int updateAction(string key, int value) => value = 2;
            expected["1"] = 2;

            //Act 
            var result = dictionary.TryUpdate("1", updateAction);

            //Assert
            result.Should().BeTrue();
            dictionary.Should().Equal(expected);
        }

        [Test]
        public void AddOrUpdate_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => dictionaryNull.AddOrUpdate("-1", 100);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void AddOrUpdate_UpdateActionIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            Func<string, int, int> updateAction = null;

            //Act 
            Action action = () => dictionary.AddOrUpdate("1", 100, updateAction);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void AddOrUpdate_KeyNotFound_AddAndReturnValue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var entry in dictionary)
                expected.Add(entry.Key, entry.Value);
            expected.Add("4", 4);

            //Act 
            var result = dictionary.AddOrUpdate("4", 4, -4);

            //Assert
            result.Should().Be(4);
            dictionary.Should().Equal(expected);
        }

        [Test]
        public void AddOrUpdate_KeyFoundAndUpdateIsValue_UpdateAndReturnValue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var entry in dictionary)
                expected.Add(entry.Key, entry.Value);
            expected["3"] = 300;

            //Act 
            var result = dictionary.AddOrUpdate("3", -3, 300);

            //Assert
            result.Should().Be(300);
            dictionary.Should().Equal(expected);
        }

        [Test]
        public void AddOrUpdate_KeyFoundAndUpdateIsFunction_UpdateAndReturnValue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var entry in dictionary)
                expected.Add(entry.Key, entry.Value);
            expected["3"] = 300;

            //Act 
            var result = dictionary.AddOrUpdate("3", -3, v => 300);

            //Assert
            result.Should().Be(300);
            dictionary.Should().Equal(expected);
        }

        [Test]
        public void GetOrAdd_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => dictionaryNull.GetOrAdd("1", 100);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetOrAdd_KeyFound_ReturnValueWithoutAdding()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var entry in dictionary)
                expected.Add(entry.Key, entry.Value);

            //Act 
            var result = dictionary.GetOrAdd("1", 100);

            //Assert
            result.Should().Be(1);
            dictionary.Should().Equal(expected);
        }

        [Test]
        public void GetOrAdd_KeyNotFound_AddAndReturnValue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var entry in dictionary)
                expected.Add(entry.Key, entry.Value);
            expected.Add("4", 4);

            //Act 
            var result = dictionary.GetOrAdd("4", 4);

            //Assert
            result.Should().Be(4);
            dictionary.Should().Equal(expected);
        }

        [Test]
        public void ToEnumerable_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => dictionaryNull.ToEnumerable();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToEnumerable_WhenCalled_ReturnKeyValuePairsEnumerable()
        {
            //Arrange
            var expected = new[]
            {
                new KeyValuePair<string, int>("1", 1),
                new KeyValuePair<string, int>("2", 2),
                new KeyValuePair<string, int>("3", 3)
            };

            //Act 
            var result = dictionary.ToEnumerable();

            //Assert
            result.Should().Equal(expected);
        }

        [Test]
        public void ContainsValue_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => dictionaryNull.ContainsValue(1);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(0, false)]
        [TestCase(2, true)]
        [TestCase(4, false)]
        public void ContainsValue_ContainsValue_ReturnTrue(int value, bool expected)
        {
            //Act 
            var result = dictionary.ContainsValue(value);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ToUrlQueryString_ParametersInNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => dictionaryNull.ToUrlQueryString();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToUrlQueryString_EncodeIsFalse_ReturnQueryStringWithValuesAsIs()
        {
            //Arrange
            var parameters = new Dictionary<string, object>()
            {
                { "name", "Efthymios Koktsidis" },
                { "color", "Black" },
                { "count", 10 }
            };
            bool encode = false;
            string expected = "name=Efthymios Koktsidis&color=Black&count=10";

            //Act
            var result = parameters.ToUrlQueryString(encode);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ToUrlQueryString_EncodeIsTrue_ReturnQueryStringWithEncodedValues()
        {
            //Arrange
            var parameters = new Dictionary<string, object>()
            {
                { "name", "Efthymios Koktsidis" },
                { "color", "Black" },
                { "count", 10 }
            };
            bool encode = true;
            string expected = "name=Efthymios+Koktsidis&color=Black&count=10";

            //Act
            var result = parameters.ToUrlQueryString(encode);

            //Assert
            result.Should().Be(expected);
        }
    }
}
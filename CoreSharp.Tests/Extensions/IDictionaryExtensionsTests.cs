using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class IDictionaryExtensionsTests
    {
        //Fields
        private readonly IDictionary<string, int> _dictionaryNull = null;
        private readonly IDictionary<string, int> _dictionary = new Dictionary<string, int>();

        //Methods 
        [SetUp]
        public void SetUp()
        {
            _dictionary.Clear();
            _dictionary.Add("1", 1);
            _dictionary.Add("2", 2);
            _dictionary.Add("3", 3);
        }

        [Test]
        public void TryGet_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _dictionaryNull.TryGet("1", out _);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void TryGet_KeyNotFound_ReturnFalse()
        {
            //Act 
            var result = _dictionary.TryGet("-1", out _);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void TryGet_KeyFound_ReturnTrueAndValue()
        {
            //Act 
            var result = _dictionary.TryGet("1", out var value);

            //Assert
            result.Should().BeTrue();
            value.Should().Be(1);
        }

        [Test]
        public void TryAdd_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _dictionaryNull.TryAdd("1", 1);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void TryAdd_KeyFound_ReturnFalseAndAddNothing()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var (key, value) in _dictionary)
                expected.Add(key, value);

            //Act 
            var added = _dictionary.TryAdd("1", 1);

            //Assert
            added.Should().BeFalse();
            _dictionary.Should().Equal(expected);
        }

        [Test]
        public void TryAdd_KeyNotFound_ReturnTrueAndAddValue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var (key, value) in _dictionary)
                expected.Add(key, value);
            expected.Add("4", 4);

            //Act 
            var added = _dictionary.TryAdd("4", 4);

            //Assert
            added.Should().BeTrue();
            _dictionary.Should().Equal(expected);
        }

        [Test]
        public void TryRemove_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _dictionaryNull.TryRemove("1", out _);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void TryRemove_KeyNotFound_ReturnFalseAndRemoveNothing()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var (key, value) in _dictionary)
                expected.Add(key, value);

            //Act 
            var result = _dictionary.TryRemove("-1", out _);

            //Assert
            result.Should().BeFalse();
            _dictionary.Should().Equal(expected);
        }

        [Test]
        public void TryRemove_KeyFound_RemoveAndReturnValueAndTrue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var (key, i) in _dictionary)
                expected.Add(key, i);
            expected.Remove("3");

            //Act 
            var removed = _dictionary.TryRemove("3", out var value);

            //Assert
            removed.Should().BeTrue();
            value.Should().Be(3);
            _dictionary.Should().Equal(expected);
        }

        [Test]
        public void TryUpdate_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _dictionaryNull.TryUpdate("-1", 100);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void TryUpdate_UpdateActionIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            Func<string, int, int> updateAction = null;

            //Act 
            Action action = () => _dictionary.TryUpdate("1", updateAction);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void TryUpdate_KeyNotFound_ReturnFalse()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var (key, value) in _dictionary)
                expected.Add(key, value);

            //Act 
            var result = _dictionary.TryUpdate("-1", 100);

            //Assert
            result.Should().BeFalse();
            _dictionary.Should().Equal(expected);
        }

        [Test]
        public void TryUpdate_KeyFoundAndUpdateIsValue_ReturnTrueAndUpdateValue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var (key, value) in _dictionary)
                expected.Add(key, value);
            expected["1"] = 100;

            //Act 
            var result = _dictionary.TryUpdate("1", 100);

            //Assert
            result.Should().BeTrue();
            _dictionary.Should().Equal(expected);
        }

        [Test]
        public void TryUpdate_KeyFoundAndUpdateIsFunction_ReturnTrueAndUpdateValue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var (key, value) in _dictionary)
                expected.Add(key, value);
            static int UpdateAction(string _, int __) => 2;

            expected["1"] = 2;

            //Act 
            var result = _dictionary.TryUpdate("1", UpdateAction);

            //Assert
            result.Should().BeTrue();
            _dictionary.Should().Equal(expected);
        }

        [Test]
        public void AddOrUpdate_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _dictionaryNull.AddOrUpdate("-1", 100);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void AddOrUpdate_UpdateActionIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            Func<string, int, int> updateAction = null;

            //Act 
            Action action = () => _dictionary.AddOrUpdate("1", 100, updateAction);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void AddOrUpdate_KeyNotFound_AddAndReturnValue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var (key, value) in _dictionary)
                expected.Add(key, value);
            expected.Add("4", 4);

            //Act 
            var result = _dictionary.AddOrUpdate("4", 4, -4);

            //Assert
            result.Should().Be(4);
            _dictionary.Should().Equal(expected);
        }

        [Test]
        public void AddOrUpdate_KeyFoundAndUpdateIsValue_UpdateAndReturnValue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var (key, value) in _dictionary)
                expected.Add(key, value);
            expected["3"] = 300;

            //Act 
            var result = _dictionary.AddOrUpdate("3", -3, 300);

            //Assert
            result.Should().Be(300);
            _dictionary.Should().Equal(expected);
        }

        [Test]
        public void AddOrUpdate_KeyFoundAndUpdateIsFunction_UpdateAndReturnValue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var (key, value) in _dictionary)
                expected.Add(key, value);
            expected["3"] = 300;

            //Act 
            var result = _dictionary.AddOrUpdate("3", -3, _ => 300);

            //Assert
            result.Should().Be(300);
            _dictionary.Should().Equal(expected);
        }

        [Test]
        public void GetOrAdd_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _dictionaryNull.GetOrAdd("1", 100);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetOrAdd_KeyFound_ReturnValueWithoutAdding()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var (key, value) in _dictionary)
                expected.Add(key, value);

            //Act 
            var result = _dictionary.GetOrAdd("1", 100);

            //Assert
            result.Should().Be(1);
            _dictionary.Should().Equal(expected);
        }

        [Test]
        public void GetOrAdd_KeyNotFound_AddAndReturnValue()
        {
            //Arrange
            var expected = new Dictionary<string, int>();
            foreach (var (key, value) in _dictionary)
                expected.Add(key, value);
            expected.Add("4", 4);

            //Act 
            var result = _dictionary.GetOrAdd("4", 4);

            //Assert
            result.Should().Be(4);
            _dictionary.Should().Equal(expected);
        }

        [Test]
        public void ToEnumerable_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _dictionaryNull.ToEnumerable();

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
            var result = _dictionary.ToEnumerable();

            //Assert
            result.Should().Equal(expected);
        }

        [Test]
        public void ContainsValue_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _dictionaryNull.ContainsValue(1);

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
            var result = _dictionary.ContainsValue(value);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ToUrlQueryString_ParametersInNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => _dictionaryNull.ToUrlQueryString();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToUrlQueryString_EncodeIsFalse_ReturnQueryStringWithValuesAsIs()
        {
            //Arrange
            var parameters = new Dictionary<string, object>
            {
                { "name", "Efthymios Koktsidis" },
                { "color", "Black" },
                { "count", 10 }
            };
            const bool encode = false;
            const string expected = "name=Efthymios Koktsidis&color=Black&count=10";

            //Act
            var result = parameters.ToUrlQueryString(encode);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ToUrlQueryString_EncodeIsTrue_ReturnQueryStringWithEncodedValues()
        {
            //Arrange
            var parameters = new Dictionary<string, object>
            {
                { "name", "Efthymios Koktsidis" },
                { "color", "Black" },
                { "count", 10 }
            };
            const string expected = "name=Efthymios+Koktsidis&color=Black&count=10";

            //Act
            var result = parameters.ToUrlQueryString();

            //Assert
            result.Should().Be(expected);
        }
    }
}

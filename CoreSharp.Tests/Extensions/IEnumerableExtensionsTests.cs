using CoreSharp.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class IEnumerableExtensionsTests
    {
        //Fields
        private readonly IEnumerable<DummyClass> _sourceNull = null;
        private readonly IEnumerable<DummyClass> _sourceEmpty = Enumerable.Empty<DummyClass>();

        //Methods 
        [Test]
        public void IsEmpty_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.IsEmpty();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void IsEmpty_SourceIsEmpty_ReturnTrue()
        {
            //Act 
            var result = _sourceEmpty.IsEmpty();

            //Assert 
            result.Should().BeTrue();
        }

        [Test]
        public void IsNullOrEmpty_SourceIsNull_ReturnTrue()
        {
            //Act 
            var result = _sourceNull.IsNullOrEmpty();

            //Assert 
            result.Should().BeTrue();
        }

        [Test]
        public void IsNullOrEmpty_SourceIsEmpty_ReturnTrue()
        {
            //Act 
            var result = _sourceEmpty.IsNullOrEmpty();

            //Assert 
            result.Should().BeTrue();
        }

        [Test]
        public void OrEmpty_SourceIsNull_ReturnEmpty()
        {
            //Act
            var result = _sourceNull.OrEmpty();

            //Assert
            result.Should().Equal(_sourceEmpty);
        }

        [Test]
        public void ConvertAll_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.ConvertAll<string>();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ConvertAll_WhenCalled_ReturnConvertedValues()
        {
            //Arrange
            var source = new[]
            {
                1,
                2,
                3
            };
            var expected = new[]
            {
                "1",
                "2",
                "3"
            };

            //Act 
            var result = source.ConvertAll<string>();

            //Assert 
            result.Should().Equal(expected);
        }

        [Test]
        public void Except_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.Except(d => d.Id == 0);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Except_FilterIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceEmpty.Except(null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Except_WhenCalled_ReturnSourceExceptFilteredItems()
        {
            //Arrange
            var item1 = new DummyClass(1);
            var item2 = new DummyClass(2);
            var item3 = new DummyClass(3);
            var source = new[] { item1, item2, item3 };
            var expected = new[] { item2, item3 };

            //Act  
            var result = source.Except(d => d.Id == 1);

            //Assert
            result.Should().Equal(expected);
        }

        [Test]
        public void Distinct_SourceIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => _sourceNull.Distinct(d => d.Id);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Distinct_FilterIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => _sourceEmpty.Distinct<DummyClass, int>(null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Distinct_WhenCalled_ReturnDistinctItemsByGivenKey()
        {
            //Arrange
            var item1 = new DummyClass(1);
            var item2 = new DummyClass(1);
            var item3 = new DummyClass(2);
            var source = new[] { item1, item2, item3 };
            var expected = new[] { item1, item3 };

            //Act 
            var result = source.Distinct(d => d.Id);

            //Assert
            result.Should().Equal(expected);
        }

        [Test]
        public void StringJoin_SourceIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => _sourceNull.StringJoin(" ", "{0}", CultureInfo.InvariantCulture);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void StringJoinCI_WhenCalled_ReturnStringFormatWithInvariantCultureArgument()
        {
            //Arrange
            const string separator = " ";
            const string format = "{0}";
            var values = new[] { 1000, 2000 };
            const string expected = "1000 2000";

            //Act
            var result = values.StringJoinCI(separator, format);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void StringJoin_WhenCalled_JoinItemsToStringWithGivenOptions()
        {
            //Arrange 
            var source = new[] { 1.111, 2.222, 3.333 };
            const string expected = "1.1 2.2 3.3";

            //Act 
            var result = source.StringJoin(" ", "{0:F1}", CultureInfo.InvariantCulture);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ToHashSet_SourceIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => _sourceNull.ToHashSet();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToHashSet_ComparerIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => _sourceEmpty.ToHashSet(null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToHashSet_WhenCalled_ReturnHashSetWithUniqueItems()
        {
            //Arrange 
            var source = new[] { 1, 1, 2, 2, 3, 3, 4, 4 };
            var expected = new HashSet<int> { 1, 2, 3, 4 };

            //Act 
            var result = source.ToHashSet();

            //Assert
            result.Should().BeOfType<HashSet<int>>();
            result.Should().Equal(expected);
        }

        [Test]
        public void ToCollection_SourceIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => _sourceNull.ToCollection();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToCollection_WhenCalled_ReturnCollectionWithAllItems()
        {
            //Arrange 
            var source = new[] { 1, 1, 2, 3, 4 };

            //Act 
            var result = source.ToCollection();

            //Assert
            result.Should().BeOfType<Collection<int>>();
            result.Should().Equal(source);
        }

        [Test]
        public void ToObservableCollection_SourceIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => _sourceNull.ToObservableCollection();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToObservableCollection_WhenCalled_ReturnObservableCollectionWithAllItems()
        {
            //Arrange 
            var source = new[] { 1, 1, 2, 3, 4 };

            //Act 
            var result = source.ToObservableCollection();

            //Assert
            result.Should().BeOfType<ObservableCollection<int>>();
            result.Should().Equal(source);
        }

        [Test]
        public void TakeSkip_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.TakeSkip(1, -1);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void TakeSkip_SequenceIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => _sourceEmpty.TakeSkip(null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void TakeSkip_WhenCalled_TakePositiveSkipNegativeChunks()
        {
            //Arrange 
            var source = new[] { 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5 };
            var sequence = new[] { 1, -2, 3, -4 };
            var expected = new[] { 1, 3, 3, 3 };

            //Act 
            var result = source.TakeSkip(sequence);

            //Assert 
            result.Should().Equal(expected);
        }

        [Test]
        public void Except_LeftIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => _sourceNull.Except(_sourceEmpty, d => d.Id);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Except_RightIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => _sourceEmpty.Except(_sourceNull, d => d.Id);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ExceptBy_KeySelectorIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => _sourceEmpty.Except<DummyClass, int>(_sourceEmpty, null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Except_WhenCalled_ReturnLeftItemsNotInRight()
        {
            //Arrange
            var item1 = new DummyClass(1);
            var item2 = new DummyClass(2);
            var item3 = new DummyClass(3);
            var left = new[] { item1, item2, item3 };
            var right = new[] { item1, item2 };
            var expected = new[] { item3 };

            //Act 
            var result = left.Except(right, d => d.Id);

            //Assert
            result.Should().Equal(expected);
        }

        [Test]
        public void Intersect_LeftIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => _sourceNull.Intersect(_sourceEmpty, d => d.Id);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Intersect_RightIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => _sourceEmpty.Intersect(_sourceNull, d => d.Id);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Intersect_KeySelectorIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => _sourceEmpty.Intersect<DummyClass, int>(_sourceEmpty, null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Intersect_WhenCalled_ReturnCommonItems()
        {
            //Arrange
            var item1 = new DummyClass(1);
            var item2 = new DummyClass(2);
            var item3 = new DummyClass(3);
            var item4 = new DummyClass(4);
            var left = new[] { item1, item2, item3, item4 };
            var right = new[] { item1, item4 };
            var expected = new[] { item1, item4 };

            //Act 
            var result = left.Intersect(right, d => d.Id);

            //Assert
            result.Should().Equal(expected);
        }

        [Test]
        public void Flatten_SourceIsNull_ThrowArgumentNullException()
        {
            //Arrange
            IEnumerable<IEnumerable<int>> sourceNull = null;

            //Act  
            Action action = () => sourceNull.Flatten();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Flatten_WhenCalled_ReturnSourceFlattenedToOneDimension()
        {
            //Arrange
            var inner1 = new[] { 1, 2, 3 };
            var inner2 = new[] { 4, 5, 6 };
            var outer = new[] { inner1, inner2 };
            var expected = new[] { 1, 2, 3, 4, 5, 6 };

            //Act  
            var result = outer.Flatten();

            //Assert 
            result.Should().Equal(expected);
        }

        [Test]
        public void Append_SourceIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            var item = new DummyClass();

            //Act  
            Action action = () => _sourceNull.Append(item);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Append_ItemsIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => _sourceEmpty.Append(null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Append_WhenCalled_ReturnSourceWithAppendedItems()
        {
            //Arrange
            var source = new[] { 1, 2, 3 };
            var items = new[] { 4, 5, 6 };
            var expected = new[] { 1, 2, 3, 4, 5, 6 };

            //Act  
            var result = source.Append(items);

            //Assert 
            result.Should().Equal(expected);
        }

        [Test]
        public void ForEach_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.ForEach(_ => { });

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ForEach_ActionIsNull_ThrowArgumentNullException()
        {
            //Arrange
            Action<DummyClass> itemAction = null;

            //Act 
            Action action = () => _sourceEmpty.ForEach(itemAction);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ForEach_WhenCalled_PerformActionOnEachItem()
        {
            //Arrange 
            var item1 = new DummyClass(1);
            var item2 = new DummyClass(2);
            var item3 = new DummyClass(3);
            var source = new[] { item1, item2, item3 };

            //Act   
            source.ForEach(d => d.Id *= 2);

            //Assert 
            item1.Id.Should().Be(2);
            item2.Id.Should().Be(4);
            item3.Id.Should().Be(6);
        }

        [Test]
        public void Mutate_SourceIsNull_ThrowArgumentNullException()
        {
            //Act   
            Action action = () => _sourceNull.Mutate();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Mutate_WhenCalled_ReturnSourceCopy()
        {
            //Arrange
            var source = new[] { 1, 2, 3, 4, 5 };

            //Act
            var result = source.Mutate();

            //Assert 
            result.Should().Equal(source);
        }

        [Test]
        public void Contains_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.Contains(null, d => d.Id);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Contains_ItemIsNull_ThrowArgumentNullException()
        {
            //Arrange
            var item = new DummyClass();

            //Act 
            Action action = () => _sourceEmpty.Contains<DummyClass, int>(item, null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Contains_KeySelectorIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            var item = new DummyClass();

            //Act 
            Action action = () => _sourceEmpty.Contains<DummyClass, int>(item, null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Contains_ContainsItemByKey_ReturnTrue()
        {
            //Arrange 
            var item1 = new DummyClass(1);
            var item2 = new DummyClass(2);
            var source = new[] { item1, item2 };
            var checkItem = new DummyClass(1);

            //Act 
            var result = source.Contains(checkItem, d => d.Id);

            //Assert 
            result.Should().BeTrue();
        }

        [Test]
        public void GetPage_SourceIsNull_ThrowArgumentNullException()
        {
            //Arrange
            const int pageIndex = 0;
            const int pageSize = 0;

            //Act 
            Action action = () => _sourceNull.GetPage(pageIndex, pageSize);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(5, -1, 1)]
        [TestCase(5, 1, -1)]
        public void GetPage_PageArgsIsOutOfRange_ThrowArgumentNullException(int itemCount, int pageIndex, int pageSize)
        {
            //Arrange
            var source = new int[itemCount];

            //Act 
            Action action = () => source.GetPage(pageIndex, pageSize);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void GetPage_WhenCalled_ReturnItemPage()
        {
            //Arrange
            var source = new[] { 1, 2, 3, 4, 5 };
            const int pageIndex = 1;
            const int pageSize = 2;
            var expected = new[] { 3, 4 };

            //Act 
            var result = source.GetPage(pageIndex, pageSize);

            //Assert 
            result.Should().Equal(expected);
        }

        [Test]
        public void GetPages_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.GetPages(2);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void GetPages_PageSizeIsEqualOrLessThanZero_ThrowArgumentNullException(int pageSize)
        {
            //Act 
            Action action = () => _sourceEmpty.GetPages(pageSize);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void GetPages_WhenCalled_ReturnItemGroupsByPageIndex()
        {
            var source = new[] { 1, 2, 3, 4, 5 };
            const int pageSize = 2;
            const int expectedCount = 3;
            var group1 = new[] { 1, 2 };
            var group2 = new[] { 3, 4 };
            var group3 = new[] { 5 };

            //Act 
            var result = source.GetPages(pageSize).ToArray();
            var result1 = result[0];
            var result2 = result[1];
            var result3 = result[2];

            //Assert 
            result.Should().HaveCount(expectedCount);
            result1.Should().Equal(group1);
            result2.Should().Equal(group2);
            result3.Should().Equal(group3);
        }

        [Test]
        public void ContainsAll_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.ContainsAll();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ContainsAll_ItemsIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceEmpty.ContainsAll(items: null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ContainsAll_AllItemsInSource_ReturnTrue()
        {
            var source = new[] { 1, 2, 3, 4, 5 };
            var items = new[] { 1, 3, 5 };

            //Act 
            var result = source.ContainsAll(items);

            //Assert 
            result.Should().BeTrue();
        }

        [Test]
        public void GetDuplicates_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.GetDuplicates();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetDuplicates_KeySelectorIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceEmpty.GetDuplicates<DummyClass, int>(null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetDuplicates_KeySelectorGiven_ReturnDictionaryWithDuplicateKeysAndCount()
        {
            //Arrange 
            var source = new[]
            {
                new DummyClass(1),
                new DummyClass(1),
                new DummyClass(2),
                new DummyClass(2),
                new DummyClass(3)
            };
            var expected = new Dictionary<int, int>
            {
                { 1, 2 },
                { 2, 2 }
            };

            //Act 
            var result = source.GetDuplicates(i => i.Id);

            //Assert 
            result.Should().Equal(expected);
        }

        [Test]
        public void GetDuplicates_NoKeySelector_ReturnDictionaryWithDuplicateKeysAndCount()
        {
            //Arrange 
            var source = new[]
            {
                1, 1, 2, 2, 3
            };
            var expected = new Dictionary<int, int>
            {
                { 1, 2 },
                { 2, 2 }
            };

            //Act 
            var result = source.GetDuplicates();

            //Assert 
            result.Should().Equal(expected);
        }

        [Test]
        public void HasDuplicates_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.HasDuplicates();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void HasDuplicates_KeySelectorIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceEmpty.HasDuplicates<DummyClass, int>(null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void HasDuplicates_KeySelectorGiven_ReturnTrueIsHasDuplicates()
        {
            //Arrange 
            var source = new[]
            {
                new DummyClass(1),
                new DummyClass(1),
                new DummyClass(2),
                new DummyClass(2),
                new DummyClass(3)
            };

            //Act 
            var result = source.HasDuplicates(i => i.Id);

            //Assert 
            result.Should().BeTrue();
        }

        [Test]
        public void HasDuplicates_NoKeySelector_ReturnTrueIsHasDuplicates()
        {
            //Arrange 
            var source = new[]
            {
                1, 1, 2, 2, 3
            };

            //Act 
            var result = source.HasDuplicates();

            //Assert 
            result.Should().BeTrue();
        }

        [Test]
        public void ToDataTable_SourceIsNull_ThrowNewArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.ToDataTable();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToDataTable_WhenCalled_ReturnDataTableWithEntries()
        {
            //Arrange
            var source = new[]
            {
                new DummyClass(1, "Black"),
                new DummyClass(2, "White")
            };
            var expected = new DataTable(nameof(DummyClass));
            expected.Columns.Add(nameof(DummyClass.Id), typeof(int));
            expected.Columns.Add(nameof(DummyClass.Name), typeof(string));
            expected.Rows.Add(1, "Black");
            expected.Rows.Add(2, "White");

            //Act 
            var result = source.ToDataTable();

            //Assert 
            result.Rows.Count.Should().Be(expected.Rows.Count);
            result.Columns.Count.Should().Be(expected.Columns.Count);
            for (var i = 0; i < result.Columns.Count; i++)
            {
                result.Columns[i].ColumnName.Should().Be(expected.Columns[i].ColumnName);
                result.Columns[i].DataType.Should().Be(expected.Columns[i].DataType);
            }

            for (var rowindex = 0; rowindex < result.Rows.Count; rowindex++)
            {
                var resultRow = result.Rows[rowindex].ItemArray;
                var expectedRow = expected.Rows[rowindex].ItemArray;
                resultRow.Should().Equal(expectedRow);
            }
        }

        [Test]
        public void StartsWith_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.StartsWith(_sourceEmpty);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void StartsWith_SequenceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceEmpty.StartsWith(_sourceNull);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void StartsWith_SourceStartsWithSequence_ReturnTrue()
        {
            //Arrange
            var source = new[] { 1, 2, 3, 4 };
            var sequence = new[] { 1, 2 };

            //Act 
            var result = source.StartsWith(sequence);

            //Assert 
            result.Should().BeTrue();
        }

        [Test]
        public void StartsWith_SourceDoesntStartWithSequence_ReturnFalse()
        {
            //Arrange
            var source = new[] { 1, 2, 3, 4 };
            var sequence = new[] { -1, 2 };

            //Act 
            var result = source.StartsWith(sequence);

            //Assert 
            result.Should().BeFalse();
        }

        [Test]
        public void EndsWith_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.EndsWith(_sourceEmpty);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void EndsWith_SequenceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceEmpty.EndsWith(_sourceNull);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void EndsWith_SourceEndsWithSequence_ReturnTrue()
        {
            //Arrange
            var source = new[] { 1, 2, 3, 4 };
            var sequence = new[] { 3, 4 };

            //Act 
            var result = source.EndsWith(sequence);

            //Assert 
            result.Should().BeTrue();
        }

        [Test]
        public void EndsWithWith_SourceDoesntStartWithSequence_ReturnFalse()
        {
            //Arrange
            var source = new[] { 1, 2, 3, 4 };
            var sequence = new[] { 3, -4 };

            //Act 
            var result = source.EndsWith(sequence);

            //Assert 
            result.Should().BeFalse();
        }

        [Test]
        public void Chunk_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.Chunk(1);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Chunk_SizeIsLessThanOne_ThrowArgumentOutOfRangeException()
        {
            //Act 
            Action action = () => _sourceEmpty.Chunk(0);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Chunk_WhenCalled_SplitSourceIntoChunks()
        {
            //Arrange
            var source = new[] { 1, 1, 2, 2, 3, 3 }.AsEnumerable();
            const int size = 2;
            var expected = new[]
            {
                new [] { 1, 1 }.AsEnumerable(),
                new [] { 2, 2 }.AsEnumerable(),
                new [] { 3, 3 }.AsEnumerable()
            };

            //Act 
            var result = source.Chunk(size).ToArray();

            //Assert
            result.Should().HaveCount(expected.Length);
            for (var i = 0; i < result.Length; i++)
                result[i].Should().Equal(expected[i]);
        }

        [Test]
        public void FirstOr_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.FirstOr(null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void FirstOr_ItemsFound_ReturnFirst()
        {
            //Arrange
            var source = new[]
            {
                new DummyClass(1, "1"),
                new DummyClass(2, "2-1"),
                new DummyClass(2, "2-2"),
                new DummyClass(3, "3")
            };

            //Act 
            var result = source.FirstOr(d => d.Id == 2, null);

            //Assert 
            result.Id.Should().Be(2);
            result.Name.Should().Be("2-1");
        }

        [Test]
        public void FirstOr_ItemsNotFound_ReturnFallbackValue()
        {
            //Arrange
            var source = new[]
            {
                new DummyClass(1, "1"),
                new DummyClass(2, "2-1"),
                new DummyClass(2, "2-2"),
                new DummyClass(3, "3")
            };
            var fallbackValue = new DummyClass(-1, "Fallback");

            //Act 
            var result = source.FirstOr(d => d.Id == -2, fallbackValue);

            //Assert 
            result.Id.Should().Be(fallbackValue.Id);
            result.Name.Should().Be(fallbackValue.Name);
        }

        [Test]
        public void LastOr_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.LastOr(null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void LastOr_ItemsFound_ReturnLast()
        {
            //Arrange
            var source = new[]
            {
                new DummyClass(1, "1"),
                new DummyClass(2, "2-1"),
                new DummyClass(2, "2-2"),
                new DummyClass(3, "3")
            };

            //Act 
            var result = source.LastOr(d => d.Id == 2, null);

            //Assert 
            result.Id.Should().Be(2);
            result.Name.Should().Be("2-2");
        }

        [Test]
        public void LastOr_ItemsNotFound_ReturnFallbackValue()
        {
            //Arrange
            var source = new[]
            {
                new DummyClass(1, "1"),
                new DummyClass(2, "2-1"),
                new DummyClass(2, "2-2"),
                new DummyClass(3, "3")
            };
            var fallbackValue = new DummyClass(-1, "Fallback");

            //Act 
            var result = source.LastOr(d => d.Id == -2, fallbackValue);

            //Assert 
            result.Id.Should().Be(fallbackValue.Id);
            result.Name.Should().Be(fallbackValue.Name);
        }

        [Test]
        public void SingleOr_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.SingleOr(null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void SingleOr_ItemFound_ReturnSingle()
        {
            //Arrange
            var source = new[]
            {
                new DummyClass(1, "1"),
                new DummyClass(2, "2"),
                new DummyClass(3, "3")
            };

            //Act 
            var result = source.SingleOr(d => d.Id == 2, null);

            //Assert 
            result.Id.Should().Be(2);
            result.Name.Should().Be("2");
        }

        [Test]
        public void SingleOr_ItemsFound_ReturnSingle()
        {
            //Arrange
            var source = new[]
            {
                new DummyClass(1, "1"),
                new DummyClass(2, "2-1"),
                new DummyClass(2, "2-2"),
                new DummyClass(3, "3")
            };

            //Act 
            Action action = () => source.SingleOr(d => d.Id == 2, null);

            //Assert 
            action.Should().ThrowExactly<InvalidOperationException>();
        }

        [Test]
        public void SingleOr_ItemNotFound_ReturnFallbackValue()
        {
            //Arrange
            var source = new[]
            {
                new DummyClass(1, "1"),
                new DummyClass(2, "2-1"),
                new DummyClass(2, "2-2"),
                new DummyClass(3, "3")
            };
            var fallbackValue = new DummyClass(-1, "Fallback");

            //Act 
            var result = source.SingleOr(d => d.Id == -2, fallbackValue);

            //Assert 
            result.Id.Should().Be(fallbackValue.Id);
            result.Name.Should().Be(fallbackValue.Name);
        }

        [Test]
        public void Map_SourceIsEmpty_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.Map(_ => { });

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Map_MapFunctionIsEmpty_ThrowArgumentNullException()
        {
            //Arrange
            Action<DummyClass> mapFunction = null;

            //Act 
            Action action = () => _sourceEmpty.Map(mapFunction);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Map_WhenCalled_MapAndReturnItems()
        {
            //Arrange
            var source = new[]
            {
                //Id = Index 
                new DummyClass(0),
                new DummyClass(1)
            };

            //Act 
            var result = source.Map((item, index) => item.Name = $"{index}");

            //Assert 
            result.Should().HaveCount(2);
            foreach (var item in result)
                $"{item.Id}".Should().Be(item.Name);
        }
    }
}

using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using CoreSharp.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;
using System.Data;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class IEnumerableExtensionsTests
    {
        //Fields
        private readonly IEnumerable<DummyClass> sourceNull = null;
        private readonly IEnumerable<DummyClass> sourceEmpty = Enumerable.Empty<DummyClass>();

        //Methods 
        [Test]
        public void IsEmpty_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => sourceNull.IsEmpty();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void IsEmpty_SourceIsEmpty_ReturnTrue()
        {
            //Act 
            var result = sourceEmpty.IsEmpty();

            //Assert 
            result.Should().BeTrue();
        }

        [Test]
        public void IsNullOrEmpty_SourceIsNull_ReturnTrue()
        {
            //Act 
            var result = sourceNull.IsNullOrEmpty();

            //Assert 
            result.Should().BeTrue();
        }

        [Test]
        public void IsNullOrEmpty_SourceIsEmpty_ReturnTrue()
        {
            //Act 
            var result = sourceEmpty.IsNullOrEmpty();

            //Assert 
            result.Should().BeTrue();
        }

        [Test]
        public void NullToEmpty_SourceIsNull_ReturnEmpty()
        {
            //Act
            var result = sourceNull.NullToEmpty();

            //Assert
            result.Should().Equal(sourceEmpty);
        }

        [Test]
        public void ConvertAll_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => sourceNull.ConvertAll<string>();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ConvertAll_WhenCalled_ReturnConvertedValues()
        {
            //Arrange
            var source = new int[]
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
        public void Exclude_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => sourceNull.Exclude(d => d.Id == 0);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Exclude_FilterIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => sourceEmpty.Exclude(null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Exclude_WhenCalled_ReturnSourceExceptFilteredItems()
        {
            //Arrange
            var item1 = new DummyClass(1);
            var item2 = new DummyClass(2);
            var item3 = new DummyClass(3);
            var source = new[] { item1, item2, item3 };
            var expected = new[] { item2, item3 };

            //Act  
            var result = source.Exclude(d => d.Id == 1);

            //Assert
            result.Should().Equal(expected);
        }

        [Test]
        public void Distinct_SourceIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceNull.Distinct(d => d.Id);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Distinct_FilterIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.Distinct<DummyClass, int>(null);

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
            Action action = () => sourceNull.StringJoin(" ", "{0}", CultureInfo.InvariantCulture);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void StringJoinCI_WhenCalled_ReturnStringFormatWithInvariantCultureArgument()
        {
            //Arrange
            string separator = " ";
            string format = "{0}";
            var values = new[] { 1000, 2000 };
            string expected = "1000 2000";

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
            var expected = "1.1 2.2 3.3";

            //Act 
            var result = source.StringJoin(" ", "{0:F1}", CultureInfo.InvariantCulture);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ToHashSet_SourceIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceNull.ToHashSet();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToHashSet_ComparerIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.ToHashSet(null);

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
            Action action = () => sourceNull.ToCollection();

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
            Action action = () => sourceNull.ToObservableCollection();

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
            Action action = () => sourceNull.TakeSkip(1, -1);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void TakeSkip_SequenceIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.TakeSkip(null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void TakeSkip_WhenCalled_TakePositiveSkipNegativeChunks()
        {
            //Arrange 
            var source = new[] { 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5 };
            var sequence = new[] { 1, -2, 3, -4 };
            var expected = new[] { 1, 3, 3, 3, };

            //Act 
            var result = source.TakeSkip(sequence);

            //Assert 
            result.Should().Equal(expected);
        }

        [Test]
        public void Except_LeftIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceNull.Except(sourceEmpty, d => d.Id);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Except_RightIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.Except(sourceNull, d => d.Id);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ExceptBy_KeySelectorIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.Except<DummyClass, int>(sourceEmpty, null);

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
            Action action = () => sourceNull.Intersect(sourceEmpty, d => d.Id);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Intersect_RightIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.Intersect(sourceNull, d => d.Id);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Intersect_KeySelectorIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.Intersect<DummyClass, int>(sourceEmpty, null);

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
            result.Should().Equals(expected);
        }

        [Test]
        public void Append_SourceIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            var item = new DummyClass();

            //Act  
            Action action = () => sourceNull.Append(item);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Append_ItemsIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.Append(null);

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
            Action action = () => sourceNull.ForEach(d => d.Id.ToString());

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ForEach_ActionIsNull_ThrowArgumentNullException()
        {
            //Arrange
            Action<DummyClass> itemAction = null;

            //Act 
            Action action = () => sourceEmpty.ForEach(itemAction);

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
            Action action = () => sourceNull.Mutate();

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
            Action action = () => sourceNull.Contains(null, d => d.Id);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Contains_ItemIsNull_ThrowArgumentNullException()
        {
            //Arrange
            var item = new DummyClass();

            //Act 
            Action action = () => sourceEmpty.Contains<DummyClass, int>(item, null);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Contains_KeySelectorIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            var item = new DummyClass();

            //Act 
            Action action = () => sourceEmpty.Contains<DummyClass, int>(item, null);

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
            int pageIndex = 0;
            int pageSize = 0;

            //Act 
            Action action = () => sourceNull.GetPage(pageIndex, pageSize);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(5, -1, 2)]
        [TestCase(5, 10, 2)]
        [TestCase(5, 10, -1)]
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
            var pageIndex = 1;
            var pageSize = 2;
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
            Action action = () => sourceNull.GetPages(2);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void GetPages_PageSizeIsEqualOrLessThanZero_ThrowArgumentNullException(int pageSize)
        {
            //Act 
            Action action = () => sourceEmpty.GetPages(pageSize);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void GetPages_WhenCalled_ReturnItemGroupsByPageIndex()
        {
            var source = new int[] { 1, 2, 3, 4, 5 };
            var pageSize = 2;
            int expectedCount = 3;
            var group1 = new[] { 1, 2 };
            var group2 = new[] { 3, 4 };
            var group3 = new[] { 5 };

            //Act 
            var result = source.GetPages(pageSize);
            var result1 = result.ElementAt(0);
            var result2 = result.ElementAt(1);
            var result3 = result.ElementAt(2);

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
            Action action = () => sourceNull.ContainsAll();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ContainsAll_ItemsIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => sourceEmpty.ContainsAll(items: null);

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
        public void ToCsv_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => sourceNull.ToCsv();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToCsv_SourceIsNull_IncludeHeaderIsFalse_ReturnCsvWithoutHeader()
        {
            //Arrange
            var source = new List<DummyClass>
            {
                new DummyClass(1, "Black"),
                new DummyClass(2, "White")
            };
            var expected = $"1,Black{Environment.NewLine}2,White{Environment.NewLine}";

            //Act 
            var result = source.ToCsv(',', false);

            //Assert 
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetDuplicates_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => sourceNull.GetDuplicates();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetDuplicates_KeySelectorIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => sourceEmpty.GetDuplicates<DummyClass, int>(null);

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
                new DummyClass(3),
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
            Action action = () => sourceNull.HasDuplicates();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void HasDuplicates_KeySelectorIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => sourceEmpty.HasDuplicates<DummyClass, int>(null);

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
                new DummyClass(3),
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
            Action action = () => sourceNull.ToDataTable();

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
            var expected = new DataTable(typeof(DummyClass).Name);
            expected.Columns.Add(nameof(DummyClass.Id), typeof(int));
            expected.Columns.Add(nameof(DummyClass.Name), typeof(string));
            expected.Rows.Add(1, "Black");
            expected.Rows.Add(2, "White");

            //Act 
            var result = source.ToDataTable();

            //Assert 
            result.Rows.Count.Should().Be(expected.Rows.Count);
            result.Columns.Count.Should().Be(expected.Columns.Count);
            for (int i = 0; i < result.Columns.Count; i++)
            {
                result.Columns[i].ColumnName.Should().Be(expected.Columns[i].ColumnName);
                result.Columns[i].DataType.Should().Be(expected.Columns[i].DataType);
            }
            for (int rowindex = 0; rowindex < result.Rows.Count; rowindex++)
            {
                var resultRow = result.Rows[rowindex].ItemArray;
                var expectedRow = expected.Rows[rowindex].ItemArray;
                resultRow.Should().Equal(expectedRow);
            }
        }
    }
}
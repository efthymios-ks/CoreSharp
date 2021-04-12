using CoreSharp.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using System.Globalization;
using System.Collections.ObjectModel;
using Moq;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class IEnumerableExtensionsTests
    {
        //Fields
        private readonly IEnumerable<Dummy> sourceNull = null;
        private readonly IEnumerable<Dummy> sourceEmpty = Enumerable.Empty<Dummy>();

        //Methods 
        [Test]
        public void IsEmpty_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => sourceNull.IsEmpty();

            //Assert 
            action.Should().Throw<ArgumentNullException>();
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
            action.Should().Throw<ArgumentNullException>();
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
            Action action = () => sourceNull.Exclude(null);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Exclude_FilterIsNull_ThrowArgumentNullException()
        {
            //Act  
            Predicate<Dummy> filter = null;
            Action action = () => sourceEmpty.Exclude(filter);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Exclude_WhenCalled_ReturnSourceExceptFilteredItems()
        {
            //Arrange
            var item1 = new Dummy(1);
            var item2 = new Dummy(2);
            var item3 = new Dummy(3);
            var source = new[] { item1, item2, item3 };
            var expected = new[] { item2, item3 };

            //Act  
            var result = source.Exclude(i => i.Id == 1);

            //Assert
            result.Should().Equal(expected);
        }

        [Test]
        public void DistinctBy_SourceIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceNull.DistinctBy<Dummy, int>(null);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void DistinctBy_FilterIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.DistinctBy<Dummy, int>(null);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void DistinctBy_WhenCalled_ReturnDistinctItemsByGivenKey()
        {
            //Arrange
            var item1 = new Dummy(1);
            var item2 = new Dummy(1);
            var item3 = new Dummy(2);
            var source = new[] { item1, item2, item3 };
            var expected = new[] { item1, item3 };

            //Act 
            var result = source.DistinctBy(i => i.Id);

            //Assert
            result.Should().Equal(expected);
        }

        [Test]
        public void StringJoin_SourceIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceNull.StringJoin(" ", "{0}", CultureInfo.InvariantCulture);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void StringJoin_WhenCalled_JoinItemsToStringWithGivenSettings()
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
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ToHashSet_WhenCalled_ReturnHashSetWithUniqueItems()
        {
            //Arrange 
            var source = new[] { 1, 1, 2, 3, 4 };
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
            action.Should().Throw<ArgumentNullException>();
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
            action.Should().Throw<ArgumentNullException>();
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
            Action action = () => sourceNull.TakeSkip();

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void TakeSkip_ChunksIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.TakeSkip(null);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
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
        public void ExceptBy_LeftIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceNull.ExceptBy(sourceEmpty, d => d.Id);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ExceptBy_RightIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.ExceptBy(sourceNull, d => d.Id);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ExceptBy_KeySelectorIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.ExceptBy<Dummy, int>(sourceEmpty, null);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ExceptBy_WhenCalled_ReturnLeftItemsNotInRight()
        {
            //Arrange
            var item1 = new Dummy(1);
            var item2 = new Dummy(2);
            var item3 = new Dummy(3);
            var left = new[] { item1, item2, item3 };
            var right = new[] { item1, item2 };
            var expected = new[] { item3 };

            //Act 
            var result = left.ExceptBy(right, i => i.Id);

            //Assert
            result.Should().Equal(expected);
        }

        [Test]
        public void IntersectBy_LeftIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceNull.IntersectBy(sourceEmpty, d => d.Id);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void IntersectBy_RightIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.IntersectBy(sourceNull, d => d.Id);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void IntersectBy_KeySelectorIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.IntersectBy<Dummy, int>(sourceEmpty, null);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void IntersectBy_WhenCalled_ReturnCommonItems()
        {
            //Arrange
            var item1 = new Dummy(1);
            var item2 = new Dummy(2);
            var item3 = new Dummy(3);
            var item4 = new Dummy(4);
            var left = new[] { item1, item2, item3, item4 };
            var right = new[] { item1, item4 };
            var expected = new[] { item1, item4 };

            //Act 
            var result = left.IntersectBy(right, i => i.Id);

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
            action.Should().Throw<ArgumentNullException>();
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
            //Act  
            Action action = () => sourceNull.Append();

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Append_ItemsIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.Append(null);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
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
            Action<Dummy> itemAction = null;
            Action action = () => sourceNull.ForEach(itemAction);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ForEach_ActionIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action<Dummy> itemAction = null;
            Action action = () => sourceEmpty.ForEach(itemAction);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ForEach_WhenCalled_PerformActionOnEachItem()
        {
            //Arrange 
            var item1 = new Dummy(1);
            var item2 = new Dummy(2);
            var item3 = new Dummy(3);
            var source = new[] { item1, item2, item3 };

            //Act   
            source.ForEach(d => d.Id = d.Id * 2);

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
            action.Should().Throw<ArgumentNullException>();
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
            //Arrange 
            Func<Dummy, int> keySelector = null;

            //Act 
            Action action = () => sourceNull.Contains(null, keySelector);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Contains_ItemIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            Func<Dummy, int> keySelector = null;

            //Act 
            Action action = () => sourceEmpty.Contains(null, keySelector);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Contains_KeySelectorIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            var item = new Dummy(1);
            Func<Dummy, int> keySelector = null;

            //Act 
            Action action = () => sourceEmpty.Contains(item, keySelector);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Contains_ContainsItemByKey_ReturnTrue()
        {
            //Arrange 
            var item1 = new Dummy(1);
            var item2 = new Dummy(2);
            var source = new[] { item1, item2 };
            var checkItem = new Dummy(1);

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
            action.Should().Throw<ArgumentNullException>();
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
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void GetPage_WhenCalled_ReturnItemPage()
        {
            //Arrange
            var source = new int[] { 1, 2, 3, 4, 5 };
            var pageIndex = 0;
            var pageSize = 2;
            var expected = new[] { 1, 2 };

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
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void GetPages_PageSizeIsEqualOrLessThanZero_ThrowArgumentNullException(int pageSize)
        {
            //Act 
            Action action = () => sourceEmpty.GetPages(pageSize);

            //Assert 
            action.Should().Throw<ArgumentOutOfRangeException>();
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
            var resultArray = result.ToArray();
            var result1 = resultArray[0];
            var result2 = resultArray[1];
            var result3 = resultArray[2];

            //Assert 
            result.Should().HaveCount(expectedCount);
            result1.Should().Equal(group1);
            result2.Should().Equal(group2);
            result3.Should().Equal(group3);
        }

        //Nested
        private class Dummy
        {
            //Constructors 
            public Dummy(int id)
            {
                Id = id;
            }

            //Properties 
            public int Id { get; set; }

            public override string ToString()
            {
                return $"{Id}";
            }
        }
    }
}
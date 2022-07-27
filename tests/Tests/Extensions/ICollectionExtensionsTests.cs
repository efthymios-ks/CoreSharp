namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class ICollectionExtensionsTests
{
    //Methods
    [Test]
    public void AddRange_SourceIsNull_ThrowArgumentNullException()
    {
        //Arrange
        ICollection<int> source = null;

        //Act
        Action action = () => source.AddRange();

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void AddRange_ItemsIsNull_ThrowArgumentNullException()
    {
        //Arrange
        ICollection<int> source = new List<int>();
        IEnumerable<int> items = null;

        //Act
        Action action = () => source.AddRange(items);

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void AddRange_WhenCalled_AddItemsToCollection()
    {
        //Arrange
        ICollection<int> source = new Collection<int> { 1, 2, 3 };
        var items = new[] { 4, 5, 6 };
        var expected = new Collection<int> { 1, 2, 3, 4, 5, 6 };

        //Act
        source.AddRange(items);

        //Assert
        source.Should().Equal(expected);
    }

    [Test]
    public void TryAdd_SourceIsNull_ThrowArgumentNullException()
    {
        //Arrange
        ICollection<int> source = null;

        //Act
        Action action = () => source.TryAdd(0);

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void TryAdd_ItemIsNull_ThrowArgumentNullException()
    {
        //Arrange
        var source = new List<int?>();

        //Act
        Action action = () => source.TryAdd(null);

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void TryAdd_KeySelectorIsNull_ThrowArgumentNullException()
    {
        //Arrange
        var source = new List<int>();
        Func<int, int> keySelector = null;

        //Act
        Action action = () => source.TryAdd(0, keySelector);

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void TryAdd_ItemExists_DontAddAndReturnFalse()
    {
        //Arrange
        var source = new List<int>()
        {
            1
        };

        //Act
        var result = source.TryAdd(1);

        //Assert
        result.Should().BeFalse();
        source.Should().HaveCount(1);
        source[0].Should().Be(1);
    }

    [Test]
    public void TryAdd_ItemDoesntExist_AddAndReturnTrue()
    {
        //Arrange
        var source = new List<int>()
        {
            1
        };

        //Act
        var result = source.TryAdd(2);

        //Assert
        result.Should().BeTrue();
        source.Should().HaveCount(2);
        source[0].Should().Be(1);
        source[1].Should().Be(2);
    }
}

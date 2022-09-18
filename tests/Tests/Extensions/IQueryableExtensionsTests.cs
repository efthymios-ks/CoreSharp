namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class IQueryableExtensionsTests
{
    // Fields
    private readonly IQueryable<DummyClass> _sourceNull;
    private readonly IQueryable<DummyClass> _sourceEmpty = Enumerable.Empty<DummyClass>().AsQueryable();

    // Methods
    [Test]
    public void GetPage_SourceIsNull_ThrowArgumentNullException()
    {
        // Arrange
        const int pageNumber = 0;
        const int pageSize = 0;

        // Act 
        Action action = () => _sourceNull.GetPage(pageNumber, pageSize);

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase(5, -1, 1)]
    [TestCase(5, 1, -1)]
    public void GetPage_PageArgsIsOutOfRange_ThrowArgumentNullException(int sourceCount, int pageNumber, int pageSize)
    {
        // Arrange
        var source = new int[sourceCount].AsQueryable();

        // Act 
        Action action = () => source.GetPage(pageNumber, pageSize);

        // Assert 
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    public void GetPage_WhenCalled_ReturnItemPage()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 }.AsQueryable();
        const int pageIndex = 1;
        const int pageSize = 2;
        var expected = new[] { 3, 4 }.AsQueryable();

        // Act 
        var result = source.GetPage(pageIndex, pageSize);

        // Assert 
        result.Should().Equal(expected);
    }

    [Test]
    public void FilterFlexible_SourceIsNull_ThrowArgumentNullException()
    {
        // Act 
        Action action = () => _sourceNull.FilterFlexible(i => i.Name, "a");

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void FilterFlexible_PropertySelectorIsNull_ThrowArgumentNullException()
    {
        // Act 
        Action action = () => _sourceEmpty.FilterFlexible(null, "a");

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void FilterFlexible_FilterIsNullOrEmpty_ReturnEmptyQueryable()
    {
        // Arrange
        var source = new[] { "a", "b" }.AsQueryable();
        var expected = Enumerable.Empty<string>().AsQueryable();

        // Act 
        var result = source.FilterFlexible("");

        // Assert 
        result.Should().Equal(expected);
    }

    [Test]
    public void FilterFlexible_WhenCalled_ReturnFilteredItems()
    {
        // Arrange 
        var source = new[] { "a", "b", "ab", ".a.b.", " A B " }.AsQueryable();
        var expected = new[] { "ab", ".a.b.", " A B " }.AsQueryable();
        const string filter = "ab";

        // Act 
        var result = source.FilterFlexible(filter);

        // Assert 
        result.Should().Equal(expected);
    }
}

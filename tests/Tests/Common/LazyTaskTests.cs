namespace CoreSharp.Common.Tests;

[TestFixture]
public class LazyTaskTests
{
    // Methods
    [Test]
    public void Constructor_ValueFactoryIsNull_ThrowArgumentNullException()
    {
        // Act 
        Action action = () => _ = new LazyTask<string>(null);

        // Arrange
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task GetValueAsync_WhenCalled_ReturnsValue()
    {
        // Arrange 
        const string value = "1";
        static Task<string> GetValueAsync()
            => Task.FromResult(value);
        var lazyTask = new LazyTask<string>(GetValueAsync);

        // Act 
        var lazyValue = await lazyTask.GetValueAsync();

        // Arrange
        lazyValue.Should().Be(value);
    }

    [Test]
    public async Task GetValueAsync_WhenCalledTwice_InitializedOnlyOnce()
    {
        // Arrange 
        var initializeCount = 0;
        Task<string> GetValueAsync()
        {
            initializeCount++;
            return Task.FromResult<string>(null);
        }

        var lazyTask = new LazyTask<string>(GetValueAsync);

        // Act 
        await lazyTask.GetValueAsync();
        await lazyTask.GetValueAsync();

        // Arrange
        initializeCount.Should().Be(1);
    }

    [Test]
    public void GetValueAsync_BeforeCalled_IsValueInitializedIsFalse()
    {
        // Arrange  
        static Task<string> GetValueAsync()
            => Task.FromResult<string>(null);
        var lazyTask = new LazyTask<string>(GetValueAsync);

        // Act 
        var isValueInitialized = lazyTask.IsValueInitialized;

        // Arrange
        isValueInitialized.Should().BeFalse();
    }

    [Test]
    public async Task GetValueAsync_WhenCalled_SetIsValueInitializedToTrue()
    {
        // Arrange  
        static Task<string> GetValueAsync()
            => Task.FromResult<string>(null);
        var lazyTask = new LazyTask<string>(GetValueAsync);

        // Act 
        await lazyTask.GetValueAsync();
        var isValueInitialized = lazyTask.IsValueInitialized;

        // Arrange
        isValueInitialized.Should().BeTrue();
    }
}
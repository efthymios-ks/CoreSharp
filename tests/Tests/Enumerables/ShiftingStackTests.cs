using CoreSharp.Enumerables;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class ShiftingStackTests
{
    [Test]
    public void Constructor_WhenMaxCapacityIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        Action action = () => _ = new ShiftingStack<int>(0);

        // Act & Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void Constructor_WhenSourceIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEnumerable<int> source = null;

        // Act
        Action action = () => _ = new ShiftingStack<int>(5, source);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenSourceIsProvided_ShouldInitializeStackWithSource()
    {
        // Arrange
        var source = new[] { 1, 2, 3 };

        // Act
        var stack = new ShiftingStack<int>(5, source);

        // Assert
        stack.Should().ContainInOrder(source);
    }

    [Test]
    public void Push_WhenMaxCapacityIsMet_ShouldShiftItemsOutBottom()
    {
        // Arrange
        var stack = new ShiftingStack<int>(3, new[] { 1, 2, 3 });

        // Act
        stack.Push(4);

        // Assert
        stack.Should().ContainInOrder(2, 3, 4);
    }

    [Test]
    public void Pop_WhenStackIsEmpty_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var stack = new ShiftingStack<int>(3);

        // Act
        Action action = () => stack.Pop();

        // Assert
        action.Should().ThrowExactly<InvalidOperationException>();
    }

    [Test]
    public void Pop_WhenStackIsNotEmpty_ShouldRemoveAndReturnTopElement()
    {
        // Arrange 
        var stack = new ShiftingStack<int>(3, new[] { 1, 2, 3 });

        // Act
        var poppedElement = stack.Pop();

        // Assert
        poppedElement.Should().Be(3);
        stack.Should().ContainInOrder(1, 2);
    }

    [Test]
    public void Peek_WhenStackIsEmpty_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var stack = new ShiftingStack<int>(3);

        // Act
        Action action = () => stack.Peek();

        // Assert
        action.Should().ThrowExactly<InvalidOperationException>();
    }

    [Test]
    public void Peek_WhenStackIsNotEmpty_ShouldReturnTopElementWithoutRemovingIt()
    {
        // Arrange
        var stack = new ShiftingStack<int>(3, new[] { 1, 2, 3 });

        // Act
        var peekedElement = stack.Peek();

        // Assert
        peekedElement.Should().Be(3);
        stack.Should().ContainInOrder(1, 2, 3);
    }

    [Test]
    public void TryPop_WhenStackIsEmpty_ShouldReturnFalseAndDefaultElement()
    {
        // Arrange
        var stack = new ShiftingStack<int>(3);

        // Act
        var result = stack.TryPop(out var element);

        // Assert
        result.Should().BeFalse();
        element.Should().Be(default);
    }

    [Test]
    public void TryPop_WhenStackIsNotEmpty_ShouldRemoveAndReturnTopElementAndReturnTrue()
    {
        // Arrange
        var stack = new ShiftingStack<int>(3, new[] { 1, 2, 3 });

        // Act
        var result = stack.TryPop(out var poppedElement);

        // Assert
        result.Should().BeTrue();
        poppedElement.Should().Be(3);
        stack.Should().ContainInOrder(1, 2);
    }

    [Test]
    public void TryPeek_WhenStackIsEmpty_ShouldReturnFalseAndDefaultElement()
    {
        // Arrange
        var stack = new ShiftingStack<int>(3);

        // Act
        var result = stack.TryPeek(out var element);

        // Assert
        result.Should().BeFalse();
        element.Should().Be(default);
    }

    [Test]
    public void TryPeek_WhenStackIsNotEmpty_ShouldReturnTopElementWithoutRemovingItAndReturnTrue()
    {
        // Arrange
        var stack = new ShiftingStack<int>(3, new[] { 1, 2, 3 });

        // Act
        var result = stack.TryPeek(out var peekedElement);

        // Assert
        result.Should().BeTrue();
        peekedElement.Should().Be(3);
        stack.Should().ContainInOrder(1, 2, 3);
    }
}
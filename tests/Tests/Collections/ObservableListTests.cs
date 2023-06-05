using CoreSharp.Collections.Events;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace CoreSharp.Collections.Tests;

[TestFixture]
public class ObservableListTests
{
    [Test]
    [SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>")]
    public void Add_DoesNotContainItem_AddItem()
    {
        // Arrange  
        var list = new ObservableList<int> { 1 };

        // Act
        list.Add(2);

        // Assert 
        list.Should().Equal(new[] { 1, 2 });
    }

    [Test]
    public void Add_DoesNotContainItem_NotifyItemAdded()
    {
        // Arrange 
        NotifyListChangedEventArgs<int> capturedArgs = null;
        var list = new ObservableList<int> { 1 };
        list.ListChanged += (_, args)
            => capturedArgs = args;

        // Act
        list.Add(2);

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Add);
        capturedArgs.Index.Should().Be(1);
        capturedArgs.NewValue.Should().Be(2);
        capturedArgs.OldValue.Should().Be(default);
    }

    [Test]
    public void Remove_DoesNotContainItem_DoNothing()
    {
        // Arrange 
        NotifyListChangedEventArgs<int> capturedArgs = null;
        var list = new ObservableList<int> { 1 };
        list.ListChanged += (_, args)
            => capturedArgs = args;

        // Act
        list.Remove(2);

        // Assert 
        capturedArgs.Should().BeNull();
        list.Should().Equal(new[] { 1 });
    }

    [Test]
    public void Remove_DoesNotContainItem_ReturnFalse()
    {
        // Arrange  
        var list = new ObservableList<int> { 1 };

        // Act
        var removed = list.Remove(2);

        // Assert 
        removed.Should().BeFalse();
    }

    [Test]
    public void Remove_ContainsItem_RemoveItem()
    {
        // Arrange 
        var list = new ObservableList<int> { 1, 2 };

        // Act
        list.Remove(2);

        // Assert 
        list.Should().Equal(new[] { 1 });
    }

    [Test]
    public void Remove_ContainsItem_ReturnTrue()
    {
        // Arrange 
        var list = new ObservableList<int> { 1 };

        // Act
        var removed = list.Remove(1);

        // Assert 
        removed.Should().BeTrue();
    }

    [Test]
    public void Remove_ContainsItem_NotifyItemRemoved()
    {
        // Arrange 
        NotifyListChangedEventArgs<int> capturedArgs = null;
        var list = new ObservableList<int> { 1, 2 };
        list.ListChanged += (_, args)
            => capturedArgs = args;

        // Act
        list.Remove(2);

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Remove);
        capturedArgs.Index.Should().Be(1);
        capturedArgs.NewValue.Should().Be(default);
        capturedArgs.OldValue.Should().Be(2);
    }

    [Test]
    public void Clear_DoesNotContainItems_DoNothing()
    {
        // Arrange 
        NotifyListChangedEventArgs<int> capturedArgs = null;
        var list = new ObservableList<int>();
        list.ListChanged += (_, args)
            => capturedArgs = args;

        // Act
        list.Clear();

        // Assert 
        capturedArgs.Should().BeNull();
        list.Should().BeEmpty();
    }

    [Test]
    public void Clear_ContainsItems_ClearItems()
    {
        // Arrange 
        var list = new ObservableList<int> { 1 };

        // Act
        list.Clear();

        // Assert 
        list.Should().BeEmpty();
    }

    [Test]
    public void Clear_ContainsItems_NotifyItemsCleared()
    {
        // Arrange 
        NotifyListChangedEventArgs<int> capturedArgs = null;
        var list = new ObservableList<int> { 1 };
        list.ListChanged += (_, args)
            => capturedArgs = args;

        // Act
        list.Clear();

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Reset);
        capturedArgs.NewValue.Should().Be(default);
        capturedArgs.OldValue.Should().Be(default);
    }
}
using CoreSharp.Collections.Events;
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
        ListChangedEventArgs<int> capturedArgs = null;
        var list = new ObservableList<int> { 1 };
        list.Changed += (_, args)
            => capturedArgs = args;

        // Act
        list.Add(2);

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(CollectionChangedAction.Add);
        capturedArgs.Index.Should().Be(1);
        capturedArgs.NewItem.Should().Be(2);
        capturedArgs.OldItem.Should().Be(default);
    }

    [Test]
    public void Remove_DoesNotContainItem_DoNothing()
    {
        // Arrange 
        ListChangedEventArgs<int> capturedArgs = null;
        var list = new ObservableList<int> { 1 };
        list.Changed += (_, args)
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
        ListChangedEventArgs<int> capturedArgs = null;
        var list = new ObservableList<int> { 1, 2 };
        list.Changed += (_, args)
            => capturedArgs = args;

        // Act
        list.Remove(2);

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(CollectionChangedAction.Remove);
        capturedArgs.Index.Should().Be(1);
        capturedArgs.OldItem.Should().Be(2);
        capturedArgs.NewItem.Should().Be(default);
    }

    [Test]
    public void Clear_DoesNotContainItems_DoNothing()
    {
        // Arrange 
        ListChangedEventArgs<int> capturedArgs = null;
        var list = new ObservableList<int>();
        list.Changed += (_, args)
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
        ListChangedEventArgs<int> capturedArgs = null;
        var list = new ObservableList<int> { 1 };
        list.Changed += (_, args)
            => capturedArgs = args;

        // Act
        list.Clear();

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(CollectionChangedAction.Clear);
        capturedArgs.OldItem.Should().Be(default);
        capturedArgs.NewItem.Should().Be(default);
    }
}
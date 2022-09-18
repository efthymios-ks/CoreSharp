using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace CoreSharp.Collections.Tests;

[TestFixture]
public class ObservableHashSetTests
{
    [Test]
    public void Add_ContainsItem_DoNothing()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var originalItems = new[] { 1, 2 };
        var hashSet = new ObservableHashSet<int>();
        foreach (var item in originalItems)
            hashSet.Add(item);
        hashSet.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        hashSet.Add(1);

        // Assert 
        capturedArgs.Should().BeNull();
        hashSet.Should().Equal(originalItems);
    }

    [Test]
    public void Add_ContainsItem_ReturnFalse()
    {
        // Arrange 
        var hashSet = new ObservableHashSet<int> { 1 };

        // Act
        var added = hashSet.Add(1);

        // Assert 
        added.Should().BeFalse();
    }

    [Test]
    [SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>")]
    public void Add_DoesNotContainItem_AddItem()
    {
        // Arrange  
        var hashSet = new ObservableHashSet<int> { 1 };

        // Act
        hashSet.Add(2);

        // Assert 
        hashSet.Should().Equal(new[] { 1, 2 });
    }

    [Test]
    public void Add_DoesNotContainItem_ReturnTre()
    {
        // Arrange  
        var hashSet = new ObservableHashSet<int> { 1 };

        // Act
        var added = hashSet.Add(2);

        // Assert 
        added.Should().BeTrue();
    }

    [Test]
    public void Add_DoesNotContainItem_NotifyItemAdded()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var hashSet = new ObservableHashSet<int> { 1 };
        hashSet.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        hashSet.Add(2);

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Add);
        capturedArgs.NewItems.Count.Should().Be(1);
        capturedArgs.NewItems[0].Should().Be(2);
    }

    [Test]
    public void Remove_DoesNotContainItem_DoNothing()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var hashSet = new ObservableHashSet<int> { 1 };
        hashSet.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        hashSet.Remove(2);

        // Assert 
        capturedArgs.Should().BeNull();
        hashSet.Should().Equal(new[] { 1 });
    }

    [Test]
    public void Remove_DoesNotContainItem_ReturnFalse()
    {
        // Arrange  
        var hashSet = new ObservableHashSet<int> { 1 };

        // Act
        var removed = hashSet.Remove(2);

        // Assert 
        removed.Should().BeFalse();
    }

    [Test]
    public void Remove_ContainsItem_RemoveItem()
    {
        // Arrange 
        var hashSet = new ObservableHashSet<int> { 1, 2 };

        // Act
        hashSet.Remove(2);

        // Assert 
        hashSet.Should().Equal(new[] { 1 });
    }

    [Test]
    public void Remove_ContainsItem_ReturnTrue()
    {
        // Arrange 
        var hashSet = new ObservableHashSet<int> { 1 };

        // Act
        var removed = hashSet.Remove(1);

        // Assert 
        removed.Should().BeTrue();
    }

    [Test]
    public void Remove_ContainsItem_NotifyRemove()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var hashSet = new ObservableHashSet<int> { 1, 2 };
        hashSet.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        hashSet.Remove(2);

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Remove);
        capturedArgs.OldItems.Count.Should().Be(1);
        capturedArgs.OldItems[0].Should().Be(2);
    }

    [Test]
    public void Clear_DoesNotContainItems_DoNothing()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var hashSet = new ObservableHashSet<int>();
        hashSet.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        hashSet.Clear();

        // Assert 
        capturedArgs.Should().BeNull();
        hashSet.Should().BeEmpty();
    }

    [Test]
    public void Clear_ContainsItems_ClearItems()
    {
        // Arrange 
        var hashSet = new ObservableHashSet<int> { 1 };

        // Act
        hashSet.Clear();

        // Assert 
        hashSet.Should().BeEmpty();
    }

    [Test]
    public void Clear_ContainsItems_NotifyReset()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var hashSet = new ObservableHashSet<int> { 1 };
        hashSet.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        hashSet.Clear();

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Reset);
        capturedArgs.OldItems.Should().BeNull();
        capturedArgs.NewItems.Should().BeNull();
    }
}
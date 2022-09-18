using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace CoreSharp.Collections.Tests;

[TestFixture]
public class ObservableDictionaryTests
{
    [Test]
    public void IndexerGet_ContainsItem_ReturnValue()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var originalItems = new KeyValuePair<string, int>[]
        {
            new ("1", 1)
        };
        var dictionary = new ObservableDictionary<string, int>();
        foreach (var item in originalItems)
            dictionary.Add(item.Key, item.Value);
        dictionary.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        var value = dictionary["1"];

        // Assert 
        value.Should().Be(1);
    }

    [Test]
    public void IndexerGet_DoesNotContainItem_ThrowKeyNotFoundException()
    {
        // Arrange 
        var dictionary = new ObservableDictionary<string, int>();

        // Act
        Action action = () => _ = dictionary["1"];

        // Assert 
        action.Should().ThrowExactly<KeyNotFoundException>();
    }

    [Test]
    [SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>")]
    public void IndexerSet_DoesNotContainItem_AddItem()
    {
        // Arrange 
        var dictionary = new ObservableDictionary<string, int>();

        // Act
        dictionary["1"] = 1;

        // Assert 
        dictionary.Count.Should().Be(1);
        dictionary["1"].Should().Be(1);
    }

    [Test]
    public void IndexerSet_DoesNotContainItem_NotifyItemAdded()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>();
        dictionary.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        dictionary["1"] = 1;

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Add);
        capturedArgs.NewItems.Count.Should().Be(1);
        capturedArgs.NewItems[0].Should().Be(new KeyValuePair<string, int>("1", 1));
    }

    [Test]
    public void IndexerSet_ContainsItem_UpdateItem()
    {
        // Arrange 
        var dictionary = new ObservableDictionary<string, int>
        {
            { "1", 1 }
        };

        // Act
        dictionary["1"] = 11;

        // Assert 
        dictionary.Count.Should().Be(1);
        dictionary["1"].Should().Be(11);
    }

    [Test]
    public void IndexerSet_ContainsItem_NotifyItemUpdated()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>
        {
            { "1", 1 }
        };
        dictionary.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        dictionary["1"] = 11;

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Replace);
        capturedArgs.NewItems.Count.Should().Be(1);
        capturedArgs.NewItems[0].Should().Be(new KeyValuePair<string, int>("1", 11));
    }

    [Test]
    public void Add_ContainsItem_DoNothing()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var originalItems = new KeyValuePair<string, int>[]
        {
            new ("1", 1),
            new ("2", 2)
        };
        var dictionary = new ObservableDictionary<string, int>();
        foreach (var item in originalItems)
            dictionary.Add(item.Key, item.Value);
        dictionary.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        dictionary.Add("1", 1);

        // Assert 
        capturedArgs.Should().BeNull();
        dictionary.Should().Equal(originalItems);
    }

    [Test]
    [SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>")]
    public void Add_DoesNotContainItem_AddItem()
    {
        // Arrange  
        var dictionary = new ObservableDictionary<string, int>
        {
            { "1", 1 }
        };

        // Act
        dictionary.Add("2", 2);

        // Assert 
        dictionary.Count.Should().Be(2);
        dictionary.Should().Equal(new KeyValuePair<string, int>[]
        {
            new ("1", 1),
            new("2", 2)
        });
    }

    [Test]
    public void Add_DoesNotContainItem_NotifyItemAdded()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>();
        dictionary.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        dictionary.Add("1", 1);

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Add);
        capturedArgs.NewItems.Count.Should().Be(1);
        capturedArgs.NewItems[0].Should().Be(new KeyValuePair<string, int>("1", 1));
    }

    [Test]
    public void TryAdd_ContainsItem_DoNothing()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>
        {
           { "1", 1 }
        };
        dictionary.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        dictionary.TryAdd("1", 1);

        // Assert 
        capturedArgs.Should().BeNull();
        dictionary.Should().Equal(new KeyValuePair<string, int>[] { new("1", 1) });
    }

    [Test]
    public void TryAdd_ContainsItem_ReturnFalse()
    {
        // Arrange 
        var dictionary = new ObservableDictionary<string, int>
        {
           { "1", 1 }
        };

        // Act
        var added = dictionary.TryAdd("1", 1);

        // Assert 
        added.Should().BeFalse();
    }

    [Test]
    public void TryAdd_DoesNotContainItem_AddItem()
    {
        // Arrange  
        var dictionary = new ObservableDictionary<string, int>
        {
            { "1", 1 }
        };

        // Act
        dictionary.TryAdd("2", 2);

        // Assert 
        dictionary.Count.Should().Be(2);
        dictionary.Should().Equal(new KeyValuePair<string, int>[]
        {
            new ("1", 1),
            new ("2", 2)
        });
    }

    [Test]
    public void TryAdd_DoesNotContainItem_ReturnTrue()
    {
        // Arrange 
        var dictionary = new ObservableDictionary<string, int>
        {
            { "1", 1 }
        };

        // Act
        var added = dictionary.TryAdd("2", 2);

        // Assert 
        added.Should().BeTrue();
    }

    [Test]
    public void TryAdd_DoesNotContainItem_NotifyItemAdded()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>
        {
            { "1", 1 }
        };
        dictionary.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        dictionary.TryAdd("2", 2);

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Add);
        capturedArgs.NewItems.Count.Should().Be(1);
        capturedArgs.NewItems[0].Should().Be(new KeyValuePair<string, int>("2", 2));
    }

    [Test]
    public void Remove_DoesNotContainItem_DoNothing()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>
        {
            { "1", 1 }
        };
        dictionary.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        dictionary.Remove("2");

        // Assert 
        capturedArgs.Should().BeNull();
        dictionary.Should().Equal(new KeyValuePair<string, int>[] { new("1", 1) });
    }

    [Test]
    public void Remove_DoesNotContainItem_ReturnFalse()
    {
        // Arrange 
        var dictionary = new ObservableDictionary<string, int>
        {
            { "1", 1 }
        };

        // Act
        var removed = dictionary.Remove("2");

        // Assert 
        removed.Should().BeFalse();
    }

    [Test]
    public void Remove_ContainsItem_RemoveItem()
    {
        // Arrange 
        var dictionary = new ObservableDictionary<string, int>
        {
            { "1", 1 }
        };

        // Act
        dictionary.Remove("1", out var removedValue);

        // Assert 
        dictionary.Should().BeEmpty();
        removedValue.Should().Be(1);
    }

    [Test]
    public void Remove_ContainsItem_ReturnTrue()
    {
        // Arrange 
        var dictionary = new ObservableDictionary<string, int>
        {
            { "1", 1 }
        };

        // Act
        var removed = dictionary.Remove("1");

        // Assert 
        removed.Should().BeTrue();
    }

    [Test]
    public void Remove_ContainsItem_NotifyRemove()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>
        {
            { "1", 1 }
        };
        dictionary.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        dictionary.Remove("1");

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Remove);
        capturedArgs.OldItems.Count.Should().Be(1);
        capturedArgs.OldItems[0].Should().Be(new KeyValuePair<string, int>("1", 1));
    }

    [Test]
    public void Clear_DoesNotContainItems_DoNothing()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>();
        dictionary.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act
        dictionary.Clear();

        // Assert 
        capturedArgs.Should().BeNull();
        dictionary.Should().BeEmpty();
    }

    [Test]
    public void Clear_ContainsItems_ClearItems()
    {
        // Arrange 
        var dictionary = new ObservableDictionary<string, int>()
        {
            { "1", 1 }
        };

        // Act
        dictionary.Clear();

        // Assert  
        dictionary.Should().BeEmpty();
    }

    [Test]
    public void Clear_ContainsItems_NotifyReset()
    {
        // Arrange 
        NotifyCollectionChangedEventArgs capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>()
        {
            { "1", 1 }
        };
        dictionary.CollectionChanged += (_, args)
            => capturedArgs = args;

        // Act 
        dictionary.Clear();

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Reset);
        capturedArgs.OldItems.Should().BeNull();
        capturedArgs.NewItems.Should().BeNull();
    }
}
using CoreSharp.Collections.Events;
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
        NotifyDictionaryChangedEventArgs<string, int> capturedArgs = null;
        var originalItems = new KeyValuePair<string, int>[]
        {
            new ("1", 1)
        };
        var dictionary = new ObservableDictionary<string, int>();
        foreach (var item in originalItems)
        {
            dictionary.Add(item.Key, item.Value);
        }

        dictionary.DictionaryChanged += (_, args)
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
        NotifyDictionaryChangedEventArgs<string, int> capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>();
        dictionary.DictionaryChanged += (_, args)
            => capturedArgs = args;

        // Act
        dictionary["1"] = 1;

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Add);
        capturedArgs.Key.Should().Be("1");
        capturedArgs.NewValue.Should().Be(1);
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
    public void IndexerSet_ContainsItem_NotifyItemReplaced()
    {
        // Arrange 
        NotifyDictionaryChangedEventArgs<string, int> capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>
        {
            { "1", 1 }
        };
        dictionary.DictionaryChanged += (_, args)
            => capturedArgs = args;

        // Act
        dictionary["1"] = 11;

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Replace);
        capturedArgs.Key.Should().Be("1");
        capturedArgs.NewValue.Should().Be(11);
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
        NotifyDictionaryChangedEventArgs<string, int> capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>();
        dictionary.DictionaryChanged += (_, args)
            => capturedArgs = args;

        // Act
        dictionary.Add("1", 1);

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Add);
        capturedArgs.Key.Should().Be("1");
        capturedArgs.NewValue.Should().Be(1);
        capturedArgs.OldValue.Should().Be(default);
    }

    [Test]
    public void TryAdd_ContainsItem_DoNothing()
    {
        // Arrange 
        NotifyDictionaryChangedEventArgs<string, int> capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>
        {
           { "1", 1 }
        };
        dictionary.DictionaryChanged += (_, args)
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
        NotifyDictionaryChangedEventArgs<string, int> capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>
        {
            { "1", 1 }
        };
        dictionary.DictionaryChanged += (_, args)
            => capturedArgs = args;

        // Act
        dictionary.TryAdd("2", 2);

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Add);
        capturedArgs.Key.Should().Be("2");
        capturedArgs.NewValue.Should().Be(2);
        capturedArgs.OldValue.Should().Be(default);
    }

    [Test]
    public void Remove_DoesNotContainItem_DoNothing()
    {
        // Arrange 
        NotifyDictionaryChangedEventArgs<string, int> capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>
        {
            { "1", 1 }
        };
        dictionary.DictionaryChanged += (_, args)
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
    public void Remove_ContainsItem_NotifyItemRemoved()
    {
        // Arrange 
        NotifyDictionaryChangedEventArgs<string, int> capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>
        {
            { "1", 1 }
        };
        dictionary.DictionaryChanged += (_, args)
            => capturedArgs = args;

        // Act
        dictionary.Remove("1");

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Remove);
        capturedArgs.Key.Should().Be("1");
        capturedArgs.NewValue.Should().Be(default);
        capturedArgs.OldValue.Should().Be(1);
    }

    [Test]
    public void Clear_DoesNotContainItems_DoNothing()
    {
        // Arrange 
        NotifyDictionaryChangedEventArgs<string, int> capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>();
        dictionary.DictionaryChanged += (_, args)
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
    public void Clear_ContainsItems_NotifyItemsCleared()
    {
        // Arrange 
        NotifyDictionaryChangedEventArgs<string, int> capturedArgs = null;
        var dictionary = new ObservableDictionary<string, int>()
        {
            { "1", 1 }
        };
        dictionary.DictionaryChanged += (_, args)
            => capturedArgs = args;

        // Act 
        dictionary.Clear();

        // Assert 
        capturedArgs.Should().NotBeNull();
        capturedArgs.Action.Should().Be(NotifyCollectionChangedAction.Reset);
        capturedArgs.NewValue.Should().Be(default);
        capturedArgs.OldValue.Should().Be(default);
    }
}
using CoreSharp.EqualityComparers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="IEnumerable{TElement}"/> extensions.
/// </summary>
public static class IEnumerableExtensions
{
    /// <summary>
    /// Check if collection is empty.
    /// </summary>
    public static bool IsEmpty<TElement>(this IEnumerable<TElement> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return !source.Any();
    }

    /// <summary>
    /// Check if collection is null or empty.
    /// </summary>
    public static bool IsNullOrEmpty<TElement>(this IEnumerable<TElement> source)
        => source?.Any() is not true;

    /// <summary>
    /// Return <see cref="Enumerable.Empty{TElement}"/> if source is null.
    /// </summary>
    public static IEnumerable<TElement> OrEmpty<TElement>(this IEnumerable<TElement> source)
        => source ?? Enumerable.Empty<TElement>();

    /// <summary>
    /// Convert items to provided type.
    /// </summary>
    public static IEnumerable<TElement> ConvertAll<TElement>(this IEnumerable source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.ConvertAllInternal<TElement>();
    }

    private static IEnumerable<TElement> ConvertAllInternal<TElement>(this IEnumerable source)
        => from object item in source select (TElement)Convert.ChangeType(item, typeof(TElement));

    /// <inheritdoc cref="Enumerable.Distinct{TElement}(IEnumerable{TElement})"/>
    public static IEnumerable<TElement> Distinct<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(keySelector);

        var keyEqualityComparer = new KeyEqualityComparer<TElement, TKey>(keySelector);
        return source.Distinct(keyEqualityComparer);
    }

    /// <inheritdoc cref="StringJoin{TElement}(IEnumerable{TElement}, string, string, IFormatProvider)"/>
    public static string StringJoin<TElement>(this IEnumerable<TElement> source)
        => source.StringJoin(null, null, null);

    /// <inheritdoc cref="StringJoin{TElement}(IEnumerable{TElement}, string, string, IFormatProvider)"/>
    public static string StringJoin<TElement>(this IEnumerable<TElement> source, string separator)
        => source.StringJoin(separator, null, null);

    /// <inheritdoc cref="StringJoin{TElement}(IEnumerable{TElement}, string, string, IFormatProvider)"/>
    public static string StringJoin<TElement>(this IEnumerable<TElement> source, string separator, string stringFormat)
        => source.StringJoin(separator, stringFormat, null);

    /// <inheritdoc cref="StringJoin{TElement}(IEnumerable{TElement}, string, string, IFormatProvider)"/>
    public static string StringJoin<TElement>(this IEnumerable<TElement> source, string separator, IFormatProvider formatProvider)
        => source.StringJoin(separator, null, formatProvider);

    /// <inheritdoc cref="StringJoin{TElement}(IEnumerable{TElement}, string, string, IFormatProvider)"/>
    public static string StringJoin<T>(this IEnumerable<T> source, IFormatProvider formatProvider)
        => source.StringJoin(null, null, formatProvider);

    /// <summary>
    /// Concatenates the elements of a specified array or the members of a collection,
    /// using the specified separator between each element or member.
    /// </summary>
    public static string StringJoin<T>(this IEnumerable<T> source, string separator, string stringFormat, IFormatProvider formatProvider)
    {
        ArgumentNullException.ThrowIfNull(source);
        separator ??= " ";
        stringFormat ??= "{0}";
        formatProvider ??= CultureInfo.CurrentCulture;

        // Format items
        var formattedItems = source.Select(i => string.Format(formatProvider, stringFormat, i));

        // Return
        return string.Join(separator, formattedItems);
    }

    /// <inheritdoc cref="StringJoin{TElement}(IEnumerable{TElement}, string, string, IFormatProvider)"/>
    public static string StringJoinCI<TElement>(this IEnumerable<TElement> source)
        => source.StringJoin(" ", string.Empty, CultureInfo.InvariantCulture);

    /// <inheritdoc cref="StringJoin{TElement}(IEnumerable{TElement}, string, string, IFormatProvider)"/>
    public static string StringJoinCI<TElement>(this IEnumerable<TElement> source, string separator)
        => source.StringJoin(separator, string.Empty, CultureInfo.InvariantCulture);

    /// <inheritdoc cref="StringJoin{TElement}(IEnumerable{TElement}, string, string, IFormatProvider)"/>
    public static string StringJoinCI<TElement>(this IEnumerable<TElement> source, string separator, string stringFormat)
        => source.StringJoin(separator, stringFormat, CultureInfo.InvariantCulture);

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="ToHashSet{TElement, TKey}(IEnumerable{TElement}, Func{TElement, TKey})"/>
    public static HashSet<TElement> ToHashSet<TElement>(this IEnumerable<TElement> source)
    {
        var comparer = EqualityComparer<TElement>.Default;
        return source.ToHashSet(comparer);
    }

    /// <inheritdoc cref="ToHashSet{TElement, TKey}(IEnumerable{TElement}, Func{TElement, TKey})"/>
    public static HashSet<TElement> ToHashSet<TElement>(this IEnumerable<TElement> source, IEqualityComparer<TElement> comparer)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(comparer);

        return new HashSet<TElement>(source, comparer);
    }
#endif

    /// <inheritdoc cref="Enumerable.ToHashSet{TSource}(IEnumerable{TSource})"/>
    public static HashSet<TElement> ToHashSet<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(keySelector);

        var comparer = new KeyEqualityComparer<TElement, TKey>(keySelector);
        return source.ToHashSet(comparer);
    }

    /// <summary>
    /// Create a <see cref="Collection{TElement}"/> from an <see cref="IEnumerable{TElement}"/>.
    /// </summary>
    public static Collection<TElement> ToCollection<TElement>(this IEnumerable<TElement> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source is IList<TElement> list
                ? new Collection<TElement>(list)
                : new Collection<TElement>(source.ToList());
    }

    /// <summary>
    /// Create a <see cref="ObservableCollection{TElement}"/> from an <see cref="ObservableCollection{TElement}"/>.
    /// </summary>
    public static ObservableCollection<TElement> ToObservableCollection<TElement>(this IEnumerable<TElement> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ObservableCollection<TElement>(source);
    }

    /// <summary>
    /// Take/Skip items in a given order.
    /// Positive chunk value means take.
    /// Negative chunk value means skip.
    /// <code>
    /// var source = GetSource();
    /// var sequence = source.TakeSkip(2, -3, 1);
    /// </code>
    /// </summary>
    public static IEnumerable<TElement> TakeSkip<TElement>(this IEnumerable<TElement> source, params int[] sequence)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(sequence);

        var result = new List<TElement>();
        var enumeratedItems = 0;
        var sourceArray = source.ToArray();
        foreach (var chunk in sequence)
        {
            var isTaking = chunk > 0;
            var absoluteChunk = Math.Abs(chunk);

            if (isTaking)
            {
                var taken = sourceArray
                    .Skip(enumeratedItems)
                    .Take(absoluteChunk);
                result.AddRange(taken);
            }

            enumeratedItems += absoluteChunk;
        }

        return result;
    }

    /// <summary>
    /// Exclude items from collection satisfying a condition.
    /// </summary>
    public static IEnumerable<TElement> Except<TElement>(this IEnumerable<TElement> source, Predicate<TElement> filter)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(filter);

        return source.Where(i => !filter(i));
    }

    /// <inheritdoc cref="Enumerable.Except{TSource}(IEnumerable{TSource}, IEnumerable{TSource}, IEqualityComparer{TSource}?)"/>
    public static IEnumerable<TElement> Except<TElement, TKey>(this IEnumerable<TElement> left, IEnumerable<TElement> right, Func<TElement, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(keySelector);

        var equalityComparer = new KeyEqualityComparer<TElement, TKey>(keySelector);
        return left.Except(right, equalityComparer);
    }

    /// <inheritdoc cref="Enumerable.Intersect{TSource}(IEnumerable{TSource}, IEnumerable{TSource}, IEqualityComparer{TSource}?)"/>
    public static IEnumerable<TElement> Intersect<TElement, TKey>(this IEnumerable<TElement> left, IEnumerable<TElement> right, Func<TElement, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(keySelector);

        var equalityComparer = new KeyEqualityComparer<TElement, TKey>(keySelector);
        return left.Intersect(right, equalityComparer);
    }

    /// <summary>
    /// Flatten the nested sequence.
    /// </summary>
    public static IEnumerable<TElement> Flatten<TElement>(this IEnumerable<IEnumerable<TElement>> sequence)
    {
        ArgumentNullException.ThrowIfNull(sequence);

        return sequence.SelectMany(source => source);
    }

    /// <inheritdoc cref="ForEach{TElement}(IEnumerable{TElement}, Action{TElement, int})"/>
    public static void ForEach<TElement>(this IEnumerable<TElement> source, Action<TElement> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        void IndexedAction(TElement item, int _) => action(item);
        source.ForEach(IndexedAction);
    }

    /// <summary>
    /// Perform an action to all elements. Each element's index is used in the logic of the action.
    /// </summary>
    public static void ForEach<TElement>(this IEnumerable<TElement> source, Action<TElement, int> action)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(action);

        var index = 0;
        foreach (var item in source)
        {
            action(item, index);
            index++;
        }
    }

    /// <summary>
    /// Mutate sequence reference.
    /// </summary>
    public static IEnumerable<TElement> Mutate<TElement>(this IEnumerable<TElement> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.ToArray();
    }

    /// <inheritdoc cref="Enumerable.Contains{TSource}(IEnumerable{TSource}, TSource, IEqualityComparer{TSource}?)"/>
    public static bool Contains<TElement, TKey>(this IEnumerable<TElement> source, TElement item, Func<TElement, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(keySelector);

        var equalityComparer = new KeyEqualityComparer<TElement, TKey>(keySelector);
        return source.Contains(item, equalityComparer);
    }

    /// <inheritdoc cref="IQueryableExtensions.GetPage{TElement}(IQueryable{TElement}, int, int)"/>
    public static IEnumerable<T> GetPage<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        => (source?.AsQueryable()).GetPage(pageNumber, pageSize);

    /// <summary>
    /// Paginate collection on given size and group them by Group.Key = Page.Index.
    /// </summary>
    public static IEnumerable<IGrouping<int, TElement>> GetPages<TElement>(this IEnumerable<TElement> source, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (pageSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), $"{nameof(pageSize)} has to be positive and non-zero.");
        }

        // ToArray() so we can get item index 
        var sourceArray = source.ToArray();

        // Group by page index 
        return sourceArray.GroupBy(item =>
        {
            var itemIndex = Array.IndexOf(sourceArray, item);
            return itemIndex / pageSize; // PageNumber 
        });
    }

    /// <inheritdoc cref="ContainsAll{TElement}(IEnumerable{TElement}, TElement[])"/>
    public static bool ContainsAll<TElement>(this IEnumerable<TElement> source, IEnumerable<TElement> items)
        => source.ContainsAll(items?.ToArray());

    /// <summary>
    /// Check if source contains all given items.
    /// </summary>
    public static bool ContainsAll<TElement>(this IEnumerable<TElement> source, params TElement[] items)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(items);

        return Array.TrueForAll(items, item => source.Contains(item));
    }

    /// <summary>
    /// Convert collection of items to csv <see cref="Stream"/>.
    /// </summary>
    public static async Task<Stream> ToCsvStreamAsync<TElement>(
        this IEnumerable<TElement> source,
        char separator = ',',
        bool includeHeader = true,
        Encoding encoding = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        encoding ??= Encoding.UTF8;

        var stream = new MemoryStream();

        if (!source.Any())
        {
            return stream;
        }

        var properties = source
            .First()
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);
        using var streamWriter = new StreamWriter(stream, encoding: encoding, leaveOpen: true);

        // Add headers 
        if (includeHeader)
        {
            var names = properties.Select(p => p.Name);
            var row = string.Join(separator, names).AsMemory();
            await streamWriter.WriteLineAsync(row, cancellationToken);
        }

        // Add values 
        foreach (var item in source)
        {
            var values = properties.Select(p => p.GetValue(item));
            var row = string.Join(separator, values).AsMemory();
            await streamWriter.WriteLineAsync(row, cancellationToken);
        }

        await streamWriter.FlushAsync();
        stream.Position = 0;

        return stream;
    }

    /// <inheritdoc cref="GetDuplicates{TElement, TKey}(IEnumerable{TElement}, Func{TElement, TKey})"/>
    public static IDictionary<TElement, int> GetDuplicates<TElement>(this IEnumerable<TElement> source)
       => source.GetDuplicates(i => i);

    /// <summary>
    /// Get key-count combination for duplicate entries.
    /// </summary>
    public static IDictionary<TKey, int> GetDuplicates<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(keySelector);

        // Skip(1).Any() has better performance than Count(), which accesses the whole collection. 
        var duplicates = source.GroupBy(keySelector)
                               .Where(g => g.Skip(1)
                                            .Any());

        return duplicates.ToDictionary(d => d.Key, d => d.Count());
    }

    /// <inheritdoc cref="HasDuplicates{TElement, TKey}(IEnumerable{TElement}, Func{TElement, TKey})"/>
    public static bool HasDuplicates<TElement>(this IEnumerable<TElement> source)
        => source.HasDuplicates(i => i);

    /// <summary>
    /// Check if there are any duplicate entries.
    /// </summary>
    public static bool HasDuplicates<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(keySelector);

        var keyEqualityComparer = new KeyEqualityComparer<TElement, TKey>(keySelector);
        var hashset = new HashSet<TElement>(keyEqualityComparer);

        foreach (var element in source)
        {
            if (!hashset.Add(element))
            {
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc cref="ToDataTable{TElement}(IEnumerable{TElement}, string)"/>
    public static DataTable ToDataTable<TElement>(this IEnumerable<TElement> source)
        where TElement : class
        => source.ToDataTable(typeof(TElement).Name);

    /// <summary>
    /// Convert collection of entities to <see cref="DataTable"/>.
    /// </summary>
    public static DataTable ToDataTable<TElement>(this IEnumerable<TElement> source, string tableName)
        where TElement : class
    {
        ArgumentNullException.ThrowIfNull(source);

        var table = new DataTable(tableName);

        if (!source.Any())
        {
            return table;
        }

        var properties = source
            .First()
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Create columns 
        foreach (var property in properties)
        {
            table.Columns.Add(property.Name, property.PropertyType.GetNullableBaseType());
        }

        // Create rows 
        foreach (var item in source)
        {
            var values = properties.Select(p => p.GetValue(item)).ToArray();
            table.Rows.Add(values);
        }

        return table;
    }

    /// <inheritdoc cref="StartsWith{TElement}(IEnumerable{TElement}, TElement[])"/>
    public static bool StartsWith<TElement>(this IEnumerable<TElement> source, IEnumerable<TElement> sequence)
        => source.StartsWith(sequence?.ToArray());

    /// <summary>
    /// Check if given source starts with given sequence.
    /// </summary>
    public static bool StartsWith<TElement>(this IEnumerable<TElement> source, params TElement[] sequence)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(sequence);

        var head = source.Take(sequence.Length);
        return head.SequenceEqual(sequence);
    }

    /// <inheritdoc cref="EndsWith{TElement}(IEnumerable{TElement}, TElement[])"/>
    public static bool EndsWith<TElement>(this IEnumerable<TElement> source, IEnumerable<TElement> sequence)
        => source.EndsWith(sequence?.ToArray());

    /// <summary>
    /// Check if given source ends with given sequence.
    /// </summary>
    public static bool EndsWith<TElement>(this IEnumerable<TElement> source, params TElement[] sequence)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(sequence);

        var tail = source.TakeLast(sequence.Length);
        return tail.SequenceEqual(sequence);
    }

    /// <inheritdoc cref="IQueryableExtensions.FilterFlexible{TItem}(IQueryable{TItem}, Func{TItem, string}, string)"/>
    public static IEnumerable<string> FilterFlexible(this IEnumerable<string> source, string filter)
        => source.FilterFlexible(i => i, filter);

    /// <inheritdoc cref="IQueryableExtensions.FilterFlexible{TItem}(IQueryable{TItem}, Func{TItem, string}, string)"/>
    public static IEnumerable<TElement> FilterFlexible<TElement>(this IEnumerable<TElement> source, Func<TElement, string> propertySelector, string filter)
        => (source?.AsQueryable()).FilterFlexible(propertySelector, filter);

    /// <inheritdoc cref="FirstOr{TItem}(IEnumerable{TItem}, Func{TItem, bool}, TItem)" />
    public static TElement FirstOr<TElement>(this IEnumerable<TElement> source, TElement fallbackValue)
        => source.FirstOr(_ => true, fallbackValue);

    /// <inheritdoc cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>>
    public static TElement FirstOr<TElement>(this IEnumerable<TElement> source, Func<TElement, bool> predicate, TElement fallbackValue)
    {
        ArgumentNullException.ThrowIfNull(source);

        var result = source.FirstOrDefault(predicate);
        return Equals(result, default(TElement)) ? fallbackValue : result;
    }

    /// <inheritdoc cref="LastOr{TItem}(IEnumerable{TItem}, Func{TItem, bool}, TItem)" />
    public static TElement LastOr<TElement>(this IEnumerable<TElement> source, TElement fallbackValue)
        => source.FirstOr(_ => true, fallbackValue);

    /// <inheritdoc cref="Enumerable.LastOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>>
    public static TElement LastOr<TElement>(this IEnumerable<TElement> source, Func<TElement, bool> predicate, TElement fallbackValue)
    {
        ArgumentNullException.ThrowIfNull(source);

        var result = source.LastOrDefault(predicate);
        return Equals(result, default(TElement)) ? fallbackValue : result;
    }

    /// <inheritdoc cref="SingleOr{TItem}(IEnumerable{TItem}, Func{TItem, bool}, TItem)" />
    public static TElement SingleOr<TElement>(this IEnumerable<TElement> source, TElement fallbackValue)
        => source.SingleOr(_ => true, fallbackValue);

    /// <inheritdoc cref="Enumerable.SingleOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>>
    public static TElement SingleOr<TElement>(this IEnumerable<TElement> source, Func<TElement, bool> predicate, TElement fallbackValue)
    {
        ArgumentNullException.ThrowIfNull(source);

        var result = source.SingleOrDefault(predicate);
        return Equals(result, default(TElement)) ? fallbackValue : result;
    }

    /// <inheritdoc cref="Map{TItem}(IEnumerable{TItem}, Action{TItem, int})"/>>
    public static IEnumerable<TElement> Map<TElement>(this IEnumerable<TElement> source, Action<TElement> mapFunction)
        where TElement : class
    {
        ArgumentNullException.ThrowIfNull(mapFunction);

        return source.Map((item, _) => mapFunction(item));
    }

    /// <summary>
    /// Format collection items with given expression.
    /// </summary>
    public static IEnumerable<TElement> Map<TElement>(this IEnumerable<TElement> source, Action<TElement, int> indexedMapFunction)
        where TElement : class
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(indexedMapFunction);

        var index = 0;
        return source.Select(item =>
        {
            indexedMapFunction(item, index++);
            return item;
        }).ToArray();
    }

    /// <summary>
    /// Returns min item by comparing provided property.
    /// </summary>
    public static TElement Min<TElement, TProperty>(this IEnumerable<TElement> source, Func<TElement, TProperty> propertySelector, IComparer<TProperty> propertyComparer)
        where TProperty : IComparable
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(propertySelector);
        ArgumentNullException.ThrowIfNull(propertyComparer);

        if (!source.Any())
        {
            return default;
        }

        var min = source.First();
        var minValue = propertySelector(min);
        foreach (var item in source.Skip(1))
        {
            var itemValue = propertySelector(item);
            if (propertyComparer.Compare(itemValue, minValue) < 0)
            {
                min = item;
                minValue = itemValue;
            }
        }

        return min;
    }

    /// <summary>
    /// Returns max item by comparing provided property.
    /// </summary>
    public static TElement Max<TElement, TProperty>(this IEnumerable<TElement> source, Func<TElement, TProperty> propertySelector, IComparer<TProperty> propertyComparer)
        where TProperty : IComparable
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(propertySelector);
        ArgumentNullException.ThrowIfNull(propertyComparer);

        if (!source.Any())
        {
            return default;
        }

        var max = source.First();
        var maxValue = propertySelector(max);
        foreach (var item in source.Skip(1))
        {
            var itemValue = propertySelector(item);
            if (propertyComparer.Compare(itemValue, maxValue) > 0)
            {
                max = item;
                maxValue = itemValue;
            }
        }

        return max;
    }

    /// <summary>
    /// Returns elements from a sequence as long as a specified condition is true,
    /// and then skips the remaining elements.
    /// </summary>
    public static IEnumerable<TElement> TakeWhile<TElement>(this IEnumerable<TElement> source, Func<TElement, bool> predicate, bool inclusive)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        return source.TakeWhileInternal(predicate, inclusive);
    }

    private static IEnumerable<TElement> TakeWhileInternal<TElement>(this IEnumerable<TElement> source, Func<TElement, bool> predicate, bool inclusive)
    {
        foreach (var element in source)
        {
            if (predicate(element))
            {
                yield return element;
            }
            else
            {
                if (inclusive)
                {
                    yield return element;
                }

                yield break;
            }
        }
    }

    /// <summary>
    /// Repeat given sequence n-times.
    /// </summary>
    public static IEnumerable<TElement> Repeat<TElement>(this IEnumerable<TElement> source, int count)
    {
        ArgumentNullException.ThrowIfNull(source);
        return count < 0
            ? throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} has to be positive or zero.")
            : source.RepeatInternal(count);
    }

    private static IEnumerable<TElement> RepeatInternal<TElement>(this IEnumerable<TElement> source, int count)
    {
        for (var i = 0; i < count; i++)
        {
            foreach (var element in source)
            {
                yield return element;
            }
        }
    }

    public static string ToStringTable<TElement>(this IEnumerable<TElement> source)
        where TElement : class
    {
        if (source == null || !source.Any())
        {
            return string.Empty;
        }

        var columnSizes = new Dictionary<string, int>();
        var properties = source
            .First()
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Calculate max column lengths
        foreach (var property in properties)
        {
            var maxColumnValueLength = source
                .Select(element => ToString(property.GetValue(element)).Length)
                .Max();

            columnSizes[property.Name] = maxColumnValueLength > property.Name.Length
                ? maxColumnValueLength
                : property.Name.Length;
        }

        const char SpacingCharacter = ' ';
        var stringBuilder = new StringBuilder();

        // Add headers
        foreach (var propertyName in properties.Select(property => property.Name))
        {
            stringBuilder
                .Append(propertyName.PadRight(columnSizes[propertyName]))
                .Append(SpacingCharacter);
        }

        stringBuilder.AppendLine();

        // Add values
        foreach (var element in source)
        {
            foreach (var property in properties)
            {
                var value = ToString(property.GetValue(element));
                stringBuilder
                    .Append(value.PadRight(columnSizes[property.Name]))
                    .Append(SpacingCharacter);
            }

            stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();

        static string ToString(object value)
            => Convert.ToString(value, CultureInfo.InvariantCulture);
    }

#if !NET6_0_OR_GREATER
    /// <inheritdoc cref="Append{TElement}(IEnumerable{TElement}, TElement[])"/>
    public static IEnumerable<TElement> Append<TElement>(this IEnumerable<TElement> source, IEnumerable<TElement> items)
        => source.Append(items?.ToArray());

    /// <summary>
    /// Append items to given source.
    /// </summary>
    public static IEnumerable<TElement> Append<TElement>(this IEnumerable<TElement> source, params TElement[] items)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(items);

        return items.Aggregate(source, Enumerable.Append);
    }
     
    /// <summary>
    /// Split the elements of a sequence into chunks of size at most size.
    /// </summary>
    public static IEnumerable<IEnumerable<TElement>> Chunk<TElement>(this IEnumerable<TElement> source, int size)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (size < 1)
            throw new ArgumentOutOfRangeException(nameof(size), $"{nameof(size)} ({size}) has to be at least 1.");

        return source.ChunkInternal(size);
    }

    /// <inheritdoc cref="Chunk{TItem}(IEnumerable{TItem}, int)"/>
    private static IEnumerable<IEnumerable<TElement>> ChunkInternal<TElement>(this IEnumerable<TElement> source, int size)
    {
        var index = 0;
        var enumerated = source.ToArray();
        while (index < enumerated.Length)
        {
            yield return enumerated.Skip(index).Take(size);
            index += size;
        }
    } 
#endif
}

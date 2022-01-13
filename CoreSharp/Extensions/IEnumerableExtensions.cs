using CoreSharp.Models.EqualityComparers;
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

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/> extensions.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Check if collection is empty.
        /// </summary>
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            return !source.Any();
        }

        /// <summary>
        /// Check if collection is null or empty.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source is null)
                return true;
            else
                return !source.Any();
        }

        /// <summary>
        /// Return <see cref="Enumerable.Empty{TResult}"/> if source is null.
        /// </summary>
        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> source)
            => source ?? Enumerable.Empty<T>();

        /// <summary>
        /// Convert items to provided type.
        /// </summary>
        public static IEnumerable<T> ConvertAll<T>(this IEnumerable source)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            return source.ConvertAllInternal<T>();
        }

        private static IEnumerable<T> ConvertAllInternal<T>(this IEnumerable source)
            => from object item in source select (T)Convert.ChangeType(item, typeof(T));

        /// <summary>
        /// Exclude items from collection satisfying a condition.
        /// </summary>
        public static IEnumerable<T> Exclude<T>(this IEnumerable<T> source, Predicate<T> filter)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            filter = filter ?? throw new ArgumentNullException(nameof(filter));

            return source.Where(i => !filter(i));
        }

        /// <summary>
        /// Return all distinct elements of the given source,
        /// where "distinctness" is determined via a specified key.
        /// </summary>
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            return source.GroupBy(keySelector)
                         .Select(i => i.FirstOrDefault());
        }

        /// <inheritdoc cref="StringJoin{T}(IEnumerable{T}, string, string, IFormatProvider)"/>
        public static string StringJoin<T>(this IEnumerable<T> source)
            => source.StringJoin(" ", string.Empty, null);

        /// <inheritdoc cref="StringJoin{T}(IEnumerable{T}, string, string, IFormatProvider)"/>
        public static string StringJoinCI<T>(this IEnumerable<T> source)
            => source.StringJoin(" ", string.Empty, CultureInfo.InvariantCulture);

        /// <inheritdoc cref="StringJoin{T}(IEnumerable{T}, string, string, IFormatProvider)"/>
        public static string StringJoin<T>(this IEnumerable<T> source, string separator)
            => source.StringJoin(separator, null, null);

        /// <inheritdoc cref="StringJoin{T}(IEnumerable{T}, string, string, IFormatProvider)"/>
        public static string StringJoinCI<T>(this IEnumerable<T> source, string separator)
            => source.StringJoin(separator, string.Empty, CultureInfo.InvariantCulture);

        /// <inheritdoc cref="StringJoin{T}(IEnumerable{T}, string, string, IFormatProvider)"/>
        public static string StringJoin<T>(this IEnumerable<T> source, string separator, string stringFormat)
            => source.StringJoin(separator, stringFormat, null);

        /// <inheritdoc cref="StringJoin{T}(IEnumerable{T}, string, string, IFormatProvider)"/>
        public static string StringJoinCI<T>(this IEnumerable<T> source, string separator, string stringFormat)
            => source.StringJoin(separator, stringFormat, CultureInfo.InvariantCulture);

        /// <inheritdoc cref="StringJoin{T}(IEnumerable{T}, string, string, IFormatProvider)"/>
        public static string StringJoin<T>(this IEnumerable<T> source, string separator, IFormatProvider formatProvider)
            => source.StringJoin(separator, null, formatProvider);

        /// <inheritdoc cref="StringJoin{T}(IEnumerable{T}, string, string, IFormatProvider)"/>
        public static string StringJoin<T>(this IEnumerable<T> source, IFormatProvider formatProvider)
            => source.StringJoin(" ", string.Empty, formatProvider);

        /// <summary>
        /// Concatenates the elements of a specified array or the members of a collection,
        /// using the specified separator between each element or member.
        /// </summary>
        public static string StringJoin<T>(this IEnumerable<T> source, string separator, string stringFormat, IFormatProvider formatProvider)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            separator ??= string.Empty;
            stringFormat ??= "{0}";
            formatProvider ??= CultureInfo.CurrentCulture;

            //Format items
            var formattedItems = source.Select(i => string.Format(formatProvider, stringFormat, i));

            //Return
            return string.Join(separator, formattedItems);
        }

        /// <inheritdoc cref="ToHashSet{T}(IEnumerable{T}, IEqualityComparer{T})"/>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            var comparer = EqualityComparer<T>.Default;
            return source.ToHashSet(comparer);
        }

        /// <inheritdoc cref="ToHashSet{T}(IEnumerable{T}, IEqualityComparer{T})"/>
        public static HashSet<TItem> ToHashSet<TItem, TKey>(this IEnumerable<TItem> source, Func<TItem, TKey> keySelector)
        {
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var comparer = new KeyEqualityComparer<TItem, TKey>(keySelector);
            return source.ToHashSet(comparer);
        }

        /// <summary>
        /// Create a <see cref="HashSet{T}"/> from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

            return new HashSet<T>(source, comparer);
        }

        /// <summary>
        /// Create a <see cref="Collection{T}"/> from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        public static Collection<T> ToCollection<T>(this IEnumerable<T> source)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            if (source is IList<T> list)
                return new Collection<T>(list);
            else
                return new Collection<T>(source.ToList());
        }

        /// <summary>
        /// Create a <see cref="ObservableCollection{T}"/> from an <see cref="ObservableCollection{T}"/>.
        /// </summary>
        public static ObservableCollection<TItem> ToObservableCollection<TItem>(this IEnumerable<TItem> source)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            return new ObservableCollection<TItem>(source);
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
        public static IEnumerable<T> TakeSkip<T>(this IEnumerable<T> source, params int[] sequence)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = sequence ?? throw new ArgumentNullException(nameof(sequence));

            var result = new List<T>();
            var enumeratedItems = 0;
            var sourceArray = source.ToArray();
            foreach (var chunk in sequence)
            {
                var isTaking = (chunk > 0);
                //bool isSkipping = !isTaking;
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

        /// <inheritdoc cref="Enumerable.Except{TSource}(IEnumerable{TSource}, IEnumerable{TSource}, IEqualityComparer{TSource}?)"/>
        public static IEnumerable<TEntity> Except<TEntity, TKey>(this IEnumerable<TEntity> left, IEnumerable<TEntity> right, Func<TEntity, TKey> keySelector)
        {
            _ = left ?? throw new ArgumentNullException(nameof(left));
            _ = right ?? throw new ArgumentNullException(nameof(right));
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var equalityComparer = new KeyEqualityComparer<TEntity, TKey>(keySelector);
            return left.Except(right, equalityComparer);
        }

        /// <inheritdoc cref="Enumerable.Intersect{TSource}(IEnumerable{TSource}, IEnumerable{TSource}, IEqualityComparer{TSource}?)"/>
        public static IEnumerable<TEntity> Intersect<TEntity, TKey>(this IEnumerable<TEntity> left, IEnumerable<TEntity> right, Func<TEntity, TKey> keySelector)
        {
            _ = left ?? throw new ArgumentNullException(nameof(left));
            _ = right ?? throw new ArgumentNullException(nameof(right));
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var equalityComparer = new KeyEqualityComparer<TEntity, TKey>(keySelector);
            return left.Intersect(right, equalityComparer);
        }

        /// <summary>
        /// Flatten the nested sequence.
        /// </summary>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> sequence)
        {
            _ = sequence ?? throw new ArgumentNullException(nameof(sequence));

            return sequence.SelectMany(source => source);
        }

        /// <inheritdoc cref="Append{T}(IEnumerable{T}, T[])"/>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, IEnumerable<T> items)
            => source.Append(items?.ToArray());

        /// <summary>
        /// Append items to given source.
        /// </summary>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, params T[] items)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = items ?? throw new ArgumentNullException(nameof(items));

            return items.Aggregate(source, Enumerable.Append);
        }

        /// <inheritdoc cref="ForEach{T}(IEnumerable{T}, Action{T, int})"/>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            _ = action ?? throw new ArgumentNullException(nameof(action));

            void IndexedAction(T item, int _) => action(item);
            source.ForEach(IndexedAction);
        }

        /// <summary>
        /// Perform an action to all elements. Each element's index is used in the logic of the action.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = action ?? throw new ArgumentNullException(nameof(action));

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
        public static IEnumerable<T> Mutate<T>(this IEnumerable<T> source)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            return source.ToArray();
        }

        /// <inheritdoc cref="Enumerable.Contains{TSource}(IEnumerable{TSource}, TSource, IEqualityComparer{TSource}?)"/>
        public static bool Contains<TEntity, TKey>(this IEnumerable<TEntity> source, TEntity item, Func<TEntity, TKey> keySelector)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = item ?? throw new ArgumentNullException(nameof(item));
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var equalityComparer = new KeyEqualityComparer<TEntity, TKey>(keySelector);
            return source.Contains(item, equalityComparer);
        }

        /// <inheritdoc cref="IQueryableExtensions.GetPage{T}(IQueryable{T}, int, int)"/>
        public static IEnumerable<T> GetPage<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
            => (source?.AsQueryable()).GetPage(pageNumber, pageSize);

        /// <summary>
        /// Paginate collection on given size and group them by Group.Key = Page.Index.
        /// </summary>
        public static IEnumerable<IGrouping<int, T>> GetPages<T>(this IEnumerable<T> source, int pageSize)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), $"{nameof(pageSize)} has to be positive and non-zero.");

            //ToArray() so we can get item index 
            var sourceArray = source.ToArray();

            //Group by page index 
            return sourceArray.GroupBy(item =>
            {
                var itemIndex = Array.IndexOf(sourceArray, item);
                return itemIndex / pageSize; //PageNumber 
            });
        }

        /// <inheritdoc cref="ContainsAll{T}(IEnumerable{T}, T[])"/>
        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> items)
            => source.ContainsAll(items?.ToArray());

        /// <summary>
        /// Check if source contains all given items.
        /// </summary>
        public static bool ContainsAll<T>(this IEnumerable<T> source, params T[] items)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = items ?? throw new ArgumentNullException(nameof(items));

            return items.All(source.Contains);
        }

        /// <summary>
        /// Convert collection of items to csv <see cref="Stream"/>.
        /// </summary>
        public static async Task<Stream> ToCsvStream<TEntity>(
            this IEnumerable<TEntity> source,
            char separator = ',',
            bool includeHeader = true,
            Encoding encoding = null,
            CancellationToken cancellationToken = default)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            encoding ??= Encoding.UTF8;

            var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream, encoding);

            //Add headers 
            if (includeHeader)
            {
                var names = properties?.Select(p => p.Name);
                var row = string.Join(separator, names).AsMemory();
                await streamWriter.WriteLineAsync(row, cancellationToken);
            }

            //Add values 
            foreach (var item in source)
            {
                var values = properties?.Select(p => p.GetValue(item));
                var row = string.Join(separator, values).AsMemory();
                await streamWriter.WriteLineAsync(row, cancellationToken);
            }

            streamWriter.Flush();
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
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            //Skip(1).Any() has better performance than Count(), which accesses the whole collection. 
            var duplicates = source
                .GroupBy(keySelector)
                .Where(g => g.Skip(1).Any());

            return duplicates.ToDictionary(d => d.Key, d => d.Count());
        }

        /// <inheritdoc cref="HasDuplicates{TElement, TKey}(IEnumerable{TElement}, Func{TElement, TKey})"/>
        public static bool HasDuplicates<TElement>(this IEnumerable<TElement> source)
            => source.HasDuplicates(i => i);

        /// <summary>
        /// Check if there are any duplicate entries.
        /// </summary>
        public static bool HasDuplicates<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector)
            => source.GetDuplicates(keySelector).Count > 0;

        /// <inheritdoc cref="ToDataTable{T}(IEnumerable{T}, string)"/>
        public static DataTable ToDataTable<TEntity>(this IEnumerable<TEntity> source)
            where TEntity : class
            => source.ToDataTable(typeof(TEntity).Name);

        /// <summary>
        /// Convert collection of entities to <see cref="DataTable"/>.
        /// </summary>
        public static DataTable ToDataTable<TEntity>(this IEnumerable<TEntity> source, string tableName)
            where TEntity : class
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var table = new DataTable(tableName);

            //Create columns 
            foreach (var property in properties)
                table.Columns.Add(property.Name, property.PropertyType);

            //Create rows 
            foreach (var item in source)
            {
                var values = properties.Select(p => p.GetValue(item)).ToArray();
                table.Rows.Add(values);
            }

            return table;
        }

        /// <inheritdoc cref="StartsWith{T}(IEnumerable{T}, T[])"/>
        public static bool StartsWith<T>(this IEnumerable<T> source, IEnumerable<T> sequence)
            => source.StartsWith(sequence?.ToArray());

        /// <summary>
        /// Check if given source starts with given sequence.
        /// </summary>
        public static bool StartsWith<T>(this IEnumerable<T> source, params T[] sequence)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = sequence ?? throw new ArgumentNullException(nameof(sequence));

            var head = source.Take(sequence.Length);
            return head.SequenceEqual(sequence);
        }

        /// <inheritdoc cref="EndsWith{T}(IEnumerable{T}, T[])"/>
        public static bool EndsWith<T>(this IEnumerable<T> source, IEnumerable<T> sequence)
            => source.EndsWith(sequence?.ToArray());

        /// <summary>
        /// Check if given source ends with given sequence.
        /// </summary>
        public static bool EndsWith<T>(this IEnumerable<T> source, params T[] sequence)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = sequence ?? throw new ArgumentNullException(nameof(sequence));

            var tail = source.TakeLast(sequence.Length);
            return tail.SequenceEqual(sequence);
        }

        /// <inheritdoc cref="IQueryableExtensions.FilterFlexible{TItem}(IQueryable{TItem}, Func{TItem, string}, string)"/>
        public static IEnumerable<string> FilterFlexible(this IEnumerable<string> source, string filter)
            => source.FilterFlexible(i => i, filter);

        /// <inheritdoc cref="IQueryableExtensions.FilterFlexible{TItem}(IQueryable{TItem}, Func{TItem, string}, string)"/>
        public static IEnumerable<TItem> FilterFlexible<TItem>(this IEnumerable<TItem> source, Func<TItem, string> propertySelector, string filter)
            => (source?.AsQueryable()).FilterFlexible(propertySelector, filter);

        /// <summary>
        /// Split the elements of a sequence into chunks of size at most size.
        /// </summary>
        public static IEnumerable<IEnumerable<TItem>> Chunk<TItem>(this IEnumerable<TItem> source, int size)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            if (size < 1)
                throw new ArgumentOutOfRangeException(nameof(size), $"{nameof(size)} ({size}) has to be at least 1.");

            return source.ChunkInternal(size);
        }

        /// <inheritdoc cref="Chunk{TItem}(IEnumerable{TItem}, int)"/>
        private static IEnumerable<IEnumerable<TItem>> ChunkInternal<TItem>(this IEnumerable<TItem> source, int size)
        {
            var index = 0;
            var enumerated = source.ToArray();
            while (index < enumerated.Length)
            {
                yield return enumerated.Skip(index).Take(size);
                index += size;
            }
        }

        /// <inheritdoc cref="FirstOr{TItem}(IEnumerable{TItem}, Func{TItem, bool}, TItem)" />
        public static TItem FirstOr<TItem>(this IEnumerable<TItem> source, TItem fallbackValue)
            => source.FirstOr(_ => true, fallbackValue);

        /// <inheritdoc cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>>
        public static TItem FirstOr<TItem>(this IEnumerable<TItem> source, Func<TItem, bool> predicate, TItem fallbackValue)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            var result = source.FirstOrDefault(predicate);
            return Equals(result, default(TItem)) ? fallbackValue : result;
        }

        /// <inheritdoc cref="LastOr{TItem}(IEnumerable{TItem}, Func{TItem, bool}, TItem)" />
        public static TItem LastOr<TItem>(this IEnumerable<TItem> source, TItem fallbackValue)
            => source.FirstOr(_ => true, fallbackValue);

        /// <inheritdoc cref="Enumerable.LastOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>>
        public static TItem LastOr<TItem>(this IEnumerable<TItem> source, Func<TItem, bool> predicate, TItem fallbackValue)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            var result = source.LastOrDefault(predicate);
            return Equals(result, default(TItem)) ? fallbackValue : result;
        }

        /// <inheritdoc cref="SingleOr{TItem}(IEnumerable{TItem}, Func{TItem, bool}, TItem)" />
        public static TItem SingleOr<TItem>(this IEnumerable<TItem> source, TItem fallbackValue)
            => source.SingleOr(_ => true, fallbackValue);

        /// <inheritdoc cref="Enumerable.SingleOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>>
        public static TItem SingleOr<TItem>(this IEnumerable<TItem> source, Func<TItem, bool> predicate, TItem fallbackValue)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            var result = source.SingleOrDefault(predicate);
            return Equals(result, default(TItem)) ? fallbackValue : result;
        }

        /// <inheritdoc cref="Map{TItem}(IEnumerable{TItem}, Action{TItem, int})"/>>
        public static IEnumerable<TItem> Map<TItem>(this IEnumerable<TItem> source, Action<TItem> mapFunction)
            where TItem : class
        {
            _ = mapFunction ?? throw new ArgumentNullException(nameof(mapFunction));

            return source.Map((item, _) => mapFunction(item));
        }

        /// <summary>
        /// Format collection items with given expression.
        /// </summary>
        public static IEnumerable<TItem> Map<TItem>(this IEnumerable<TItem> source, Action<TItem, int> indexedMapFunction)
            where TItem : class
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = indexedMapFunction ?? throw new ArgumentNullException(nameof(indexedMapFunction));

            var index = 0;
            return source.Select(item =>
            {
                indexedMapFunction(item, index++);
                return item;
            }).ToArray();
        }
    }
}

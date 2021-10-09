using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

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
        /// Return empty collection if source is null.
        /// </summary>
        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> source)
            => source ?? Enumerable.Empty<T>();

        /// <summary>
        /// Convert items to given type.
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

            return source.GroupBy(keySelector).Select(i => i.FirstOrDefault());
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
        /// String.Join collection of items using custom separator, String.Format and FormatProvider.
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

        /// <summary>
        /// Create a HashSet from an IEnumerable.
        /// </summary>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

            return new HashSet<T>(source, comparer);
        }

        /// <summary>
        /// Create a Collection from an IEnumerable.
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
        /// Create a ObservableCollection from an IEnumerable.
        /// </summary>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            return new ObservableCollection<T>(source);
        }

        /// <summary>
        /// Take/Skip items in a given order.
        /// Positive Chunk value means take.
        /// Negative Chunk value means skips.
        /// </summary>
        /// <example>
        /// In this sample we Take(2), then Skip(3), then Take(1).
        /// <code>
        /// var source = GetSource();
        /// var sequence = source.TakeSkip(2, -3, 1);
        /// </code>
        /// </example>
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

        /// <summary>
        /// LINQ Except(), using a key for equality comparison.
        /// </summary>
        public static IEnumerable<TEntity> Except<TEntity, TProperty>(this IEnumerable<TEntity> left, IEnumerable<TEntity> right, Func<TEntity, TProperty> keySelector)
        {
            _ = left ?? throw new ArgumentNullException(nameof(left));
            _ = right ?? throw new ArgumentNullException(nameof(right));
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            return left.Where(l => !right.Any(r => keySelector(r).Equals(keySelector(l))));
        }

        /// <summary>
        /// LINQ Intersect(), using a key for equality comparison.
        /// </summary>
        public static IEnumerable<TEntity> Intersect<TEntity, TProperty>(this IEnumerable<TEntity> left, IEnumerable<TEntity> right, Func<TEntity, TProperty> keySelector)
        {
            _ = left ?? throw new ArgumentNullException(nameof(left));
            _ = right ?? throw new ArgumentNullException(nameof(right));
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            return left.Where(l => right.Any(r => keySelector(r).Equals(keySelector(l))));
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

        /// <summary>
        /// Check if source contains a specific item, using a key for equality comparison.
        /// </summary>
        public static bool Contains<TEntity, TProperty>(this IEnumerable<TEntity> source, TEntity item, Func<TEntity, TProperty> keySelector)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = item ?? throw new ArgumentNullException(nameof(item));
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var itemKey = keySelector(item);
            var sourceKeys = source.Select(i => keySelector(i));
            return sourceKeys.Contains(itemKey);
        }

        /// <inheritdoc cref="IQueryableExtensions.QueryPage{T}(IQueryable{T}, int, int)"/>
        public static IEnumerable<T> GetPage<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
            => (source?.AsQueryable()).QueryPage(pageIndex, pageSize);

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
            return sourceArray.GroupBy(i =>
            {
                var itemIndex = Array.IndexOf(sourceArray, i);
                var pageIndex = itemIndex / pageSize;
                return pageIndex;
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
        /// Converts collection to csv.
        /// </summary>
        public static string ToCsv<T>(this IEnumerable<T> source, char separator = ',', bool includeHeader = true)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            var builder = new StringBuilder();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //Add property names to header 
            if (includeHeader)
            {
                var names = properties?.Select(p => p.Name);
                var row = string.Join(separator, names);
                builder.AppendLine(row);
            }

            //Add values 
            foreach (var item in source)
            {
                var values = properties?.Select(p => p.GetValue(item));
                var row = string.Join(separator, values);
                builder.AppendLine(row);
            }

            return builder.ToString();
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
        public static DataTable ToDataTable<TEntity>(this IEnumerable<TEntity> source) where TEntity : class
            => source.ToDataTable(typeof(TEntity).Name);

        /// <summary>
        /// Convert collection of entities to DataTable.
        /// </summary>
        public static DataTable ToDataTable<TEntity>(this IEnumerable<TEntity> source, string tableName) where TEntity : class
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
    }
}

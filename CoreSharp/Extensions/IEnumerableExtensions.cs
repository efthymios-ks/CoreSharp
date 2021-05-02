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
    /// IEnumerable extensions.
    /// </summary>
    public static partial class IEnumerableExtensions
    {
        /// <summary>
        /// Check if collection is empty. 
        /// </summary>
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            return !source.Any();
        }

        /// <summary>
        /// Check if collection is null empty.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return true;
            else
                return !source.Any();
        }

        /// <summary>
        /// Return empty collection if source is null. 
        /// </summary>
        public static IEnumerable<T> NullToEmpty<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Convert items to given type.
        /// </summary>
        public static IEnumerable<T> ConvertAll<T>(this IEnumerable source)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            var result = new List<T>();
            foreach (var item in source)
                result.Add((T)Convert.ChangeType(item, typeof(T)));
            return result;
        }

        /// <summary>
        /// Exclude items from collection satisfying a condition.
        /// </summary>
        public static IEnumerable<T> Exclude<T>(this IEnumerable<T> source, Predicate<T> filter)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            filter = filter ?? throw new ArgumentNullException(nameof(filter));

            return source.Where(i => !filter(i));
        }

        /// <summary>
        /// Return all distinct elements of the given source, 
        /// where "distinctness" is determined via a specified key.
        /// </summary>
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            return source.GroupBy(keySelector).Select(i => i.FirstOrDefault());
        }

        /// <summary>
        /// String.Join collection of items using separator=` `, String.Format=`{0}` and FormatProvider=`CurrentCulture`. 
        /// </summary>
        public static string StringJoin<T>(this IEnumerable<T> source)
        {
            return source.StringJoin(" ", string.Empty, null);
        }

        /// <summary>
        /// String.Join collection of items using separator=` `, String.Format=`{0}` and FormatProvider=`InvariantCulture`. 
        /// </summary>
        public static string StringJoinCI<T>(this IEnumerable<T> source)
        {
            return source.StringJoin(" ", string.Empty, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// String.Join collection of items using custom separator, String.Format=`{0}` and FormatProvider=`CurrentCulture`. 
        /// </summary>
        public static string StringJoin<T>(this IEnumerable<T> source, string separator)
        {
            return source.StringJoin(separator, string.Empty, null);
        }

        /// <summary>
        /// String.Join collection of items using custom separator, String.Format=`{0}` and FormatProvider=`InvariantCulture`. 
        /// </summary>
        public static string StringJoinCI<T>(this IEnumerable<T> source, string separator)
        {
            return source.StringJoin(separator, string.Empty, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// String.Join collection of items using custom separator, custom String.Format and FormatProvider=`CurrentCulture`.
        /// </summary>
        public static string StringJoin<T>(this IEnumerable<T> source, string separator, string stringFormat)
        {
            return source.StringJoin(separator, stringFormat, null);
        }

        /// <summary>
        /// String.Join collection of items using custom separator, custom String.Format and FormatProvider=`InvariantCulture`.
        /// </summary>
        public static string StringJoinCI<T>(this IEnumerable<T> source, string separator, string stringFormat)
        {
            return source.StringJoin(separator, stringFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// String.Join collection of items using custom separator, String.Format=`{0}` and custom FormatProvider.
        /// </summary>
        public static string StringJoin<T>(this IEnumerable<T> source, string separator, IFormatProvider formatProvider)
        {
            return source.StringJoin(separator, string.Empty, formatProvider);
        }

        /// <summary>
        /// String.Join collection of items using separator=` `, String.Format=`{0}` and custom FormatProvider.
        /// </summary>
        public static string StringJoin<T>(this IEnumerable<T> source, IFormatProvider formatProvider)
        {
            return source.StringJoin(" ", string.Empty, formatProvider);
        }

        /// <summary>
        /// String.Join collection of items using custom separator, String.Format and FormatProvider.
        /// </summary>
        public static string StringJoin<T>(this IEnumerable<T> source, string separator, string stringFormat, IFormatProvider formatProvider)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            //Validate Separator
            if (string.IsNullOrEmpty(separator))
                separator = " ";

            //Validate StringFormat
            if (string.IsNullOrEmpty(stringFormat))
                stringFormat = "{0}";

            //Validate FormatProvider 
            formatProvider ??= CultureInfo.CurrentCulture;

            //Convert items
            var convertedItems = source.Select(i => string.Format(formatProvider, stringFormat, i));

            //Return
            return string.Join(separator, convertedItems);
        }

        /// <summary>
        /// Convert IEnumerable Source to HashSet.
        /// </summary>
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
            source = source ?? throw new ArgumentNullException(nameof(source));
            comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));

            return new HashSet<T>(source, comparer);
        }

        /// <summary>
        /// Create a Collection from an IEnumerable. 
        /// </summary>
        public static Collection<T> ToCollection<T>(this IEnumerable<T> source)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            var collection = new Collection<T>();
            collection.AddRange(source);
            return collection;
        }

        /// <summary>
        /// Create a ObservableCollection from an IEnumerable. 
        /// </summary>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

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
            source = source ?? throw new ArgumentNullException(nameof(source));
            sequence = sequence ?? throw new ArgumentNullException(nameof(sequence));

            var result = new List<T>();
            int enumeratedItems = 0;
            foreach (var chunk in sequence)
            {
                bool isTaking = (chunk > 0);
                //bool isSkipping = !isTaking;
                var absoluteChunk = Math.Abs(chunk);

                if (isTaking)
                {
                    var taken = source
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
        public static IEnumerable<TEntity> ExceptBy<TEntity, TProperty>(this IEnumerable<TEntity> left, IEnumerable<TEntity> right, Func<TEntity, TProperty> keySelector)
        {
            left = left ?? throw new ArgumentNullException(nameof(left));
            right = right ?? throw new ArgumentNullException(nameof(right));
            keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            return left.Where(l => !right.Any(r => keySelector(r).Equals(keySelector(l))));
        }

        /// <summary>
        /// LINQ Intersect(), using a key for equality comparison.
        /// </summary> 
        public static IEnumerable<TEntity> IntersectBy<TEntity, TProperty>(this IEnumerable<TEntity> left, IEnumerable<TEntity> right, Func<TEntity, TProperty> keySelector)
        {
            left = left ?? throw new ArgumentNullException(nameof(left));
            right = right ?? throw new ArgumentNullException(nameof(right));
            keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            return left.Where(l => right.Any(r => keySelector(r).Equals(keySelector(l))));
        }

        /// <summary>
        /// Flatten the nested sequence. 
        /// </summary> 
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> sequence)
        {
            sequence = sequence ?? throw new ArgumentNullException(nameof(sequence));

            return sequence.SelectMany(source => source);
        }

        /// <summary>
        /// Append items to given source. 
        /// </summary> 
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            return source.Append(items?.ToArray());
        }

        /// <summary>
        /// Append items to given source. 
        /// </summary> 
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, params T[] items)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            items = items ?? throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
                source = Enumerable.Append(source, item);

            return source;
        }

        /// <summary>
        /// Perform an action to all elements.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            action = action ?? throw new ArgumentNullException(nameof(action));

            foreach (var item in source)
                action(item);
        }

        /// <summary>
        /// Perform an action to all elements. Each element's index is used in the logic of the action.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            action = action ?? throw new ArgumentNullException(nameof(action));

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
            source = source ?? throw new ArgumentNullException(nameof(source));

            return source.ToArray();
        }

        /// <summary>
        /// Check if source contains a specific item, using a key for equality comparison. 
        /// </summary> 
        public static bool Contains<TEntity, TProperty>(this IEnumerable<TEntity> source, TEntity item, Func<TEntity, TProperty> keySelector)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            item = item ?? throw new ArgumentNullException(nameof(item));
            keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var itemProperty = keySelector(item);
            return source.Any(i => keySelector(i).Equals(itemProperty));
        }

        /// <summary>
        /// Paginate collection on given size and return page of given index. 
        /// </summary> 
        public static IEnumerable<T> GetPage<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var pages = source.GetPages(pageSize);

            int totalPages = pages.Count();
            if (pageIndex < 0 || pageIndex >= totalPages)
                throw new ArgumentOutOfRangeException($"{pageIndex} out of range ({0} to {totalPages - 1}).");

            return pages.FirstOrDefault(p => p.Key == pageIndex);
        }

        /// <summary>
        /// Paginate collection on given size and group them by Group.Key = Page.Index. 
        /// </summary> 
        public static IEnumerable<IGrouping<int, T>> GetPages<T>(this IEnumerable<T> source, int pageSize)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException($"{pageSize} has to be positive and non-zero.");

            //ToArray() so we can get item index 
            var sourceArray = source.ToArray();

            //Group by page index 
            var groups = sourceArray.GroupBy(i =>
            {
                int itemIndex = Array.IndexOf(sourceArray, i);
                var pageIndex = itemIndex / pageSize;
                return pageIndex;
            });

            return groups;
        }

        /// <summary>
        /// Check if source contails all given items. 
        /// </summary> 
        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            return source.ContainsAll(items?.ToArray());
        }

        /// <summary>
        /// Check if source contails all given items. 
        /// </summary> 
        public static bool ContainsAll<T>(this IEnumerable<T> source, params T[] items)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            items = items ?? throw new ArgumentNullException(nameof(items));

            return items.All(i => source.Contains(i));
        }

        /// <summary>
        /// Converts collection to csv. 
        /// </summary> 
        public static string ToCsv<T>(this IEnumerable<T> source, char separator = ',', bool includeHeader = true)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

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

        /// <summary>
        /// Get key-count combination for duplicate entries. 
        /// </summary> 
        public static IDictionary<TElement, int> GetDuplicates<TElement>(this IEnumerable<TElement> source)
        {
            return source.GetDuplicates(i => i);
        }

        /// <summary>
        /// Get key-count combination for duplicate entries based on given key. 
        /// </summary> 
        public static IDictionary<TKey, int> GetDuplicates<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            //Skip(1).Any() has better performance than Count(), which accesses the whole collection. 
            var duplicates = source
                .GroupBy(keySelector)
                .Where(g => g.Skip(1).Any());

            return duplicates.ToDictionary(d => d.Key, d => d.Count());
        }

        /// <summary>
        /// Check if there are any duplicate entries.
        /// </summary> 
        public static bool HasDuplicates<TElement>(this IEnumerable<TElement> source)
        {
            return source.HasDuplicates(i => i);
        }

        /// <summary>
        /// Check if there are any duplicate entries based on given key. 
        /// </summary> 
        public static bool HasDuplicates<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector)
        {
            return source
                .GetDuplicates(keySelector)
                .Any();
        }

        /// <summary>
        /// Convert collection of entities to DataTable. 
        /// </summary> 
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var table = new DataTable(typeof(T).Name);

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
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elusive.Solodex.Core.Extensions
{
    /// <summary>
    /// Defines the  EnumerableExtensions type used to provide extension methods for the IEnumerable type.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns a batches of the larger list in the batch size requested
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">The IEnumerable<typeparam name="TSource"></typeparam> source.</param>
        /// <param name="batchSize">Max size of each subset</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int batchSize)
        {
            var batch = new List<TSource>();
            foreach (var item in source)
            {
                batch.Add(item);
                if (batch.Count == batchSize)
                {
                    yield return batch;
                    batch = new List<TSource>();
                }
            }

            if (batch.Any())
            {
                yield return batch;
            }
        }

        /// <summary>
        /// Performs the specified action on each item in the enumerable collection.
        /// </summary>
        /// <typeparam name="T">Type of item in the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="action">The action to perform on each item.</param>
        [DebuggerStepThrough]
        public static void Each<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }
        
        /// <summary>
        /// A LINQ group by, but also ensures that elements are in increasing order for each group.  Groups are split not only by the group key, but also
        /// by non-increasing element ordering.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="elementSelector">The element selector.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static IEnumerable<TResult> IncreasingGroupBy<T, TKey, TElement, TResult>(
            this IEnumerable<T> source,
            Func<T, TKey> keySelector,
            Func<T, TElement> elementSelector,
            Func<TKey, IEnumerable<TElement>, TResult> resultSelector,
            IComparer<T> comparer) where TKey : class
        {
            var items =
                source.Select(x => new { Key = keySelector(x), SourceItem = x, Element = elementSelector(x), }).ToList();

            // I want a list of the anonymous type, but empty.  Better way?
            var processedItems = items.ToList();
            processedItems.Clear();

            TKey lastKey = default(TKey);
            foreach (var item in items)
            {
                if (lastKey == null || lastKey.Equals(item.Key))
                {
                    lastKey = item.Key;
                    if (processedItems.Select(x => x.SourceItem).All(x => comparer.Compare(x, item.SourceItem) <= 0))
                    {
                        // it is greater than prior elements.
                        processedItems.Add(item);
                    }
                    else
                    {
                        // We've found a smaller element, so we're done with this set.
                        yield return resultSelector(lastKey, processedItems.Select(x => x.Element).ToList());
                        processedItems.Clear();
                        processedItems.Add(item);
                        lastKey = item.Key;
                    }
                }
                else
                {
                    // Different grouping key than last item.  End set and start a new one.
                    yield return resultSelector(lastKey, processedItems.Select(x => x.Element).ToList());
                    processedItems.Clear();
                    processedItems.Add(item);
                    lastKey = item.Key;
                }
            }
            yield return resultSelector(lastKey, processedItems.Select(x => x.Element).ToList());
        }

        /// <summary>
        /// Allows including a collection in a trace statement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string ToTraceString<T>(this IEnumerable<T> source)
        {
            return source.ToTraceString(x => x.ToString(), CultureInfo.CurrentCulture.TextInfo.ListSeparator);
        }

        /// <summary>
        /// Allows including a collection in a trace statement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="converter">The item converter</param>
        /// <returns></returns>
        public static string ToTraceString<T>(this IEnumerable<T> source, Func<T, string> converter)
        {
            return source.ToTraceString(converter, CultureInfo.CurrentCulture.TextInfo.ListSeparator);
        }

        /// <summary>
        /// Allows including a collection in a trace statement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string ToTraceString<T>(this IEnumerable<T> source, string separator)
        {
            return source.ToTraceString(x => x.ToString(), separator);
        }

        /// <summary>
        /// Allows including a collection in a trace statement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="converter">The converter.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string ToTraceString<T>(this IEnumerable<T> source, Func<T, string> converter, string separator)
        {
            return string.Join(separator, source.Select(converter).ToArray());
        }
    }
}
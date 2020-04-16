using System;
using System.Collections.Generic;
using System.Linq;

namespace Acolyte.Collections
{
    public static class AcolyteCollectionExtensions
    {
        public static bool IsNotEmpty<TItem>(this IEnumerable<TItem> collection)
        {
            return collection.Any();
        }

        public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return !source.IsNotEmpty();
        }

        public static IEnumerable<TSource> OrderBySequence<TSource, TOrder>(
            this IEnumerable<TSource> source,
            IEnumerable<TOrder> order,
            Func<TSource, TOrder> sourceKeySelector)
        {
            return source.OrderBySequence(
                order, sourceKeySelector, orderItem => orderItem,
                (sourceItem, orderItem) => sourceItem
            );
        }

        public static IEnumerable<TSource> OrderBySequence<TSource, TOrder, TKey>(
            this IEnumerable<TSource> source,
            IEnumerable<TOrder> order,
            Func<TSource, TKey> sourceKeySelector,
            Func<TOrder, TKey> orderKeySelector)
        {
            return source.OrderBySequence(
                order, sourceKeySelector, orderKeySelector,
                (sourceItem, orderItem) => sourceItem
            );
        }

        public static IEnumerable<TResult> OrderBySequence<TSource, TOrder, TKey, TResult>(
            this IEnumerable<TSource> source,
            IEnumerable<TOrder> order,
            Func<TSource, TKey> sourceKeySelector,
            Func<TOrder, TKey> orderKeySelector,
            Func<TSource, TOrder, TResult> resultSelector)
        {
            return order.Join(
                source, orderKeySelector, sourceKeySelector,
                (orderItem, sourceItem) => resultSelector(sourceItem, orderItem)
            );
        }
    }
}

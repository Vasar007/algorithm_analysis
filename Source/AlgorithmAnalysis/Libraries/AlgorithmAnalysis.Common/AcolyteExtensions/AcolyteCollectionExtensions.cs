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
    }
}

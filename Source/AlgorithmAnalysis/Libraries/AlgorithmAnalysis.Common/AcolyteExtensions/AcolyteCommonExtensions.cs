using System.Collections.Generic;
using Acolyte.Assertions;

namespace Acolyte.Common
{
    public static class AcolyteCommonExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this T value)
        {
            return ToList(value);
        }

        public static T[] ToArray<T>(this T value)
        {
            value.ThrowIfNullValue(nameof(value), assertOnPureValueTypes: false);

            return new[] { value };
        }

        public static List<T> ToList<T>(this T value)
        {
            value.ThrowIfNullValue(nameof(value), assertOnPureValueTypes: false);

            return new List<T> { value };
        }

        public static IReadOnlyList<T> ToReadOnlyList<T>(this T value)
        {
            return value.ToList();
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this T value)
        {
            return value.ToList();
        }
    }
}

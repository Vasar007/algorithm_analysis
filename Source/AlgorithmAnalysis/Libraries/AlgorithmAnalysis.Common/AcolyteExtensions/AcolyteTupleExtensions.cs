using System;
using System.Collections.Generic;
using Acolyte.Assertions;

namespace Acolyte.Common
{
    public static class AcolyteTupleExtensions
    {
        // TODO: add extension methods for tuple of arbitrary length.

        #region ToEnumerable

        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T> tuple)
        {
            return ToList(tuple);
        }

        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T, T> tuple)
        {
            return ToList(tuple);
        }

        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T, T, T> tuple)
        {
            return ToList(tuple);
        }

        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T, T, T, T> tuple)
        {
            return ToList(tuple);
        }

        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T, T, T, T, T> tuple)
        {
            return ToList(tuple);
        }

        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T, T, T, T, T, T> tuple)
        {
            return ToList(tuple);
        }

        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T, T, T, T, T, T, T> tuple)
        {
            return ToList(tuple);
        }

        #endregion

        #region ToArray

        public static T[] ToArray<T>(this Tuple<T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new[]
            {
                tuple.Item1
            };
        }

        public static T[] ToArray<T>(this Tuple<T, T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new[]
             {
                tuple.Item1,
                tuple.Item2
            };
        }

        public static T[] ToArray<T>(this Tuple<T, T, T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new[]
              {
                tuple.Item1,
                tuple.Item2,
                tuple.Item3
            };
        }

        public static T[] ToArray<T>(this Tuple<T, T, T, T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new[]
             {
                tuple.Item1,
                tuple.Item2,
                tuple.Item3,
                tuple.Item4
            };
        }

        public static T[] ToArray<T>(this Tuple<T, T, T, T, T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new[]
             {
                tuple.Item1,
                tuple.Item2,
                tuple.Item3,
                tuple.Item4,
                tuple.Item5
            };
        }

        public static T[] ToArray<T>(this Tuple<T, T, T, T, T, T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new[]
             {
                tuple.Item1,
                tuple.Item2,
                tuple.Item3,
                tuple.Item4,
                tuple.Item5,
                tuple.Item6
            };
        }

        public static T[] ToArray<T>(this Tuple<T, T, T, T, T, T, T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new[]
            {
                tuple.Item1,
                tuple.Item2,
                tuple.Item3,
                tuple.Item4,
                tuple.Item5,
                tuple.Item6,
                tuple.Item7
            };
        }

        #endregion

        #region ToList

        public static List<T> ToList<T>(this Tuple<T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new List<T>
            {
                tuple.Item1
            };
        }

        public static List<T> ToList<T>(this Tuple<T, T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new List<T>
             {
                tuple.Item1,
                tuple.Item2
            };
        }

        public static List<T> ToList<T>(this Tuple<T, T, T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new List<T>
              {
                tuple.Item1,
                tuple.Item2,
                tuple.Item3
            };
        }

        public static List<T> ToList<T>(this Tuple<T, T, T, T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new List<T>
             {
                tuple.Item1,
                tuple.Item2,
                tuple.Item3,
                tuple.Item4
            };
        }

        public static List<T> ToList<T>(this Tuple<T, T, T, T, T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new List<T>
             {
                tuple.Item1,
                tuple.Item2,
                tuple.Item3,
                tuple.Item4,
                tuple.Item5
            };
        }

        public static List<T> ToList<T>(this Tuple<T, T, T, T, T, T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new List<T>
             {
                tuple.Item1,
                tuple.Item2,
                tuple.Item3,
                tuple.Item4,
                tuple.Item5,
                tuple.Item6
            };
        }

        public static List<T> ToList<T>(this Tuple<T, T, T, T, T, T, T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new List<T>
            {
                tuple.Item1,
                tuple.Item2,
                tuple.Item3,
                tuple.Item4,
                tuple.Item5,
                tuple.Item6,
                tuple.Item7
            };
        }

        #endregion

        #region ToReadOnlyList

        public static IReadOnlyList<T> ToReadOnlyList<T>(this Tuple<T> tuple)
        {
            return ToList(tuple);
        }

        public static IReadOnlyList<T> ToReadOnlyList<T>(this Tuple<T, T> tuple)
        {
            return ToList(tuple);
        }

        public static IReadOnlyList<T> ToReadOnlyList<T>(this Tuple<T, T, T> tuple)
        {
            return ToList(tuple);
        }

        public static IReadOnlyList<T> ToReadOnlyList<T>(this Tuple<T, T, T, T> tuple)
        {
            return ToList(tuple);
        }

        public static IReadOnlyList<T> ToReadOnlyList<T>(this Tuple<T, T, T, T, T> tuple)
        {
            return ToList(tuple);
        }

        public static IReadOnlyList<T> ToReadOnlyList<T>(this Tuple<T, T, T, T, T, T> tuple)
        {
            return ToList(tuple);
        }

        public static IReadOnlyList<T> ToReadOnlyList<T>(this Tuple<T, T, T, T, T, T, T> tuple)
        {
            return ToList(tuple);
        }

        #endregion

        #region ToReadOnlyCollection

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this Tuple<T> tuple)
        {
            return ToList(tuple);
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this Tuple<T, T> tuple)
        {
            return ToList(tuple);
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this Tuple<T, T, T> tuple)
        {
            return ToList(tuple);
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this Tuple<T, T, T, T> tuple)
        {
            return ToList(tuple);
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(
            this Tuple<T, T, T, T, T> tuple)
        {
            return ToList(tuple);
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(
            this Tuple<T, T, T, T, T, T> tuple)
        {
            return ToList(tuple);
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(
            this Tuple<T, T, T, T, T, T, T> tuple)
        {
            return ToList(tuple);
        }

        #endregion
    }
}

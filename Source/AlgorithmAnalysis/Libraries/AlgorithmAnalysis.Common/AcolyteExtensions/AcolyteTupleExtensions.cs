using System;
using System.Collections.Generic;
using Acolyte.Assertions;

namespace Acolyte.Common
{
    public static class AcolyteTupleExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this Tuple<T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new[]
            {
                tuple.Item1
            };
        }

        public static IEnumerable<T> AsEnumerable<T>(this Tuple<T, T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new[]
             {
                tuple.Item1,
                tuple.Item2
            };
        }

        public static IEnumerable<T> AsEnumerable<T>(this Tuple<T, T, T> tuple)
        {
            tuple.ThrowIfNull(nameof(tuple));

            return new[]
              {
                tuple.Item1,
                tuple.Item2,
                tuple.Item3
            };
        }

        public static IEnumerable<T> AsEnumerable<T>(this Tuple<T, T, T, T> tuple)
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

        public static IEnumerable<T> AsEnumerable<T>(this Tuple<T, T, T, T, T> tuple)
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

        public static IEnumerable<T> AsEnumerable<T>(this Tuple<T, T, T, T, T, T> tuple)
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

        public static IEnumerable<T> AsEnumerable<T>(this Tuple<T, T, T, T, T, T, T> tuple)
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

        // TODO: add extension methods for tuple of arbitrary length.
    }
}

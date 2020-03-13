using System;
using System.Collections.Generic;

namespace Acolyte.Common
{
    public static class AcolyteValueTupleExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this ValueTuple<T> valueTuple)
        {
            return new[]
            {
                valueTuple.Item1
            };
        }

        public static IEnumerable<T> AsEnumerable<T>(this ValueTuple<T, T> valueTuple)
        {
            return new[]
             {
                valueTuple.Item1,
                valueTuple.Item2
            };
        }

        public static IEnumerable<T> AsEnumerable<T>(this ValueTuple<T, T, T> valueTuple)
        {
            return new[]
              {
                valueTuple.Item1,
                valueTuple.Item2,
                valueTuple.Item3
            };
        }

        public static IEnumerable<T> AsEnumerable<T>(this ValueTuple<T, T, T, T> valueTuple)
        {
            return new[]
             {
                valueTuple.Item1,
                valueTuple.Item2,
                valueTuple.Item3,
                valueTuple.Item4
            };
        }

        public static IEnumerable<T> AsEnumerable<T>(this ValueTuple<T, T, T, T, T> valueTuple)
        {
            return new[]
             {
                valueTuple.Item1,
                valueTuple.Item2,
                valueTuple.Item3,
                valueTuple.Item4,
                valueTuple.Item5
            };
        }

        public static IEnumerable<T> AsEnumerable<T>(this ValueTuple<T, T, T, T, T, T> valueTuple)
        {
            return new[]
             {
                valueTuple.Item1,
                valueTuple.Item2,
                valueTuple.Item3,
                valueTuple.Item4,
                valueTuple.Item5,
                valueTuple.Item6
            };
        }

        public static IEnumerable<T> AsEnumerable<T>(
            this ValueTuple<T, T, T, T, T, T, T> valueTuple)
        {
            return new[]
            {
                valueTuple.Item1,
                valueTuple.Item2,
                valueTuple.Item3,
                valueTuple.Item4,
                valueTuple.Item5,
                valueTuple.Item6,
                valueTuple.Item7
            };
        }

        // TODO: add extension methods for value valueTuple of arbitrary length.
    }
}

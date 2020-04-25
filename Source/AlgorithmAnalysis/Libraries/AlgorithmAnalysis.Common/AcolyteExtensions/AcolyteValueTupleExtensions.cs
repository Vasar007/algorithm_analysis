using System;
using System.Collections.Generic;
using Acolyte.Assertions;

namespace Acolyte.Common
{
    public static class AcolyteValueTupleExtensions
    {
        // TODO: add extension methods for value valueTuple of arbitrary length.

        #region ToEnumerable

        public static IEnumerable<T> ToEnumerable<T>(this ValueTuple<T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IEnumerable<T> ToEnumerable<T>(this ValueTuple<T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IEnumerable<T> ToEnumerable<T>(this ValueTuple<T, T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IEnumerable<T> ToEnumerable<T>(this ValueTuple<T, T, T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IEnumerable<T> ToEnumerable<T>(this ValueTuple<T, T, T, T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IEnumerable<T> ToEnumerable<T>(this ValueTuple<T, T, T, T, T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IEnumerable<T> ToEnumerable<T>(this ValueTuple<T, T, T, T, T, T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        #endregion

        #region ToArray

        public static T[] ToArray<T>(this ValueTuple<T> valueTuple)
        {
            valueTuple.ThrowIfNullValue(nameof(valueTuple), assertOnPureValueTypes: false);

            return new[]
            {
                valueTuple.Item1
            };
        }

        public static T[] ToArray<T>(this ValueTuple<T, T> valueTuple)
        {
            valueTuple.ThrowIfNullValue(nameof(valueTuple), assertOnPureValueTypes: false);

            return new[]
             {
                valueTuple.Item1,
                valueTuple.Item2
            };
        }

        public static T[] ToArray<T>(this ValueTuple<T, T, T> valueTuple)
        {
            valueTuple.ThrowIfNullValue(nameof(valueTuple), assertOnPureValueTypes: false);

            return new[]
              {
                valueTuple.Item1,
                valueTuple.Item2,
                valueTuple.Item3
            };
        }

        public static T[] ToArray<T>(this ValueTuple<T, T, T, T> valueTuple)
        {
            valueTuple.ThrowIfNullValue(nameof(valueTuple), assertOnPureValueTypes: false);

            return new[]
             {
                valueTuple.Item1,
                valueTuple.Item2,
                valueTuple.Item3,
                valueTuple.Item4
            };
        }

        public static T[] ToArray<T>(this ValueTuple<T, T, T, T, T> valueTuple)
        {
            valueTuple.ThrowIfNullValue(nameof(valueTuple), assertOnPureValueTypes: false);

            return new[]
             {
                valueTuple.Item1,
                valueTuple.Item2,
                valueTuple.Item3,
                valueTuple.Item4,
                valueTuple.Item5
            };
        }

        public static T[] ToArray<T>(this ValueTuple<T, T, T, T, T, T> valueTuple)
        {
            valueTuple.ThrowIfNullValue(nameof(valueTuple), assertOnPureValueTypes: false);

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

        public static T[] ToArray<T>(this ValueTuple<T, T, T, T, T, T, T> valueTuple)
        {
            valueTuple.ThrowIfNullValue(nameof(valueTuple), assertOnPureValueTypes: false);

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

        #endregion

        #region ToList

        public static List<T> ToList<T>(this ValueTuple<T> valueTuple)
        {
            valueTuple.ThrowIfNullValue(nameof(valueTuple), assertOnPureValueTypes: false);

            return new List<T>
            {
                valueTuple.Item1
            };
        }

        public static List<T> ToList<T>(this ValueTuple<T, T> valueTuple)
        {
            valueTuple.ThrowIfNullValue(nameof(valueTuple), assertOnPureValueTypes: false);

            return new List<T>
             {
                valueTuple.Item1,
                valueTuple.Item2
            };
        }

        public static List<T> ToList<T>(this ValueTuple<T, T, T> valueTuple)
        {
            valueTuple.ThrowIfNullValue(nameof(valueTuple), assertOnPureValueTypes: false);

            return new List<T>
              {
                valueTuple.Item1,
                valueTuple.Item2,
                valueTuple.Item3
            };
        }

        public static List<T> ToList<T>(this ValueTuple<T, T, T, T> valueTuple)
        {
            valueTuple.ThrowIfNullValue(nameof(valueTuple), assertOnPureValueTypes: false);

            return new List<T>
             {
                valueTuple.Item1,
                valueTuple.Item2,
                valueTuple.Item3,
                valueTuple.Item4
            };
        }

        public static List<T> ToList<T>(this ValueTuple<T, T, T, T, T> valueTuple)
        {
            valueTuple.ThrowIfNullValue(nameof(valueTuple), assertOnPureValueTypes: false);

            return new List<T>
             {
                valueTuple.Item1,
                valueTuple.Item2,
                valueTuple.Item3,
                valueTuple.Item4,
                valueTuple.Item5
            };
        }

        public static List<T> ToList<T>(this ValueTuple<T, T, T, T, T, T> valueTuple)
        {
            valueTuple.ThrowIfNullValue(nameof(valueTuple), assertOnPureValueTypes: false);

            return new List<T>
             {
                valueTuple.Item1,
                valueTuple.Item2,
                valueTuple.Item3,
                valueTuple.Item4,
                valueTuple.Item5,
                valueTuple.Item6
            };
        }

        public static List<T> ToList<T>(this ValueTuple<T, T, T, T, T, T, T> valueTuple)
        {
            valueTuple.ThrowIfNullValue(nameof(valueTuple), assertOnPureValueTypes: false);

            return new List<T>
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

        #endregion

        #region ToReadOnlyList

        public static IReadOnlyList<T> ToReadOnlyList<T>(this ValueTuple<T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IReadOnlyList<T> ToReadOnlyList<T>(this ValueTuple<T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IReadOnlyList<T> ToReadOnlyList<T>(this ValueTuple<T, T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IReadOnlyList<T> ToReadOnlyList<T>(this ValueTuple<T, T, T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IReadOnlyList<T> ToReadOnlyList<T>(this ValueTuple<T, T, T, T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IReadOnlyList<T> ToReadOnlyList<T>(this ValueTuple<T, T, T, T, T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IReadOnlyList<T> ToReadOnlyList<T>(this ValueTuple<T, T, T, T, T, T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        #endregion

        #region ToReadOnlyCollection

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this ValueTuple<T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this ValueTuple<T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this ValueTuple<T, T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(
            this ValueTuple<T, T, T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(
            this ValueTuple<T, T, T, T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(
            this ValueTuple<T, T, T, T, T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(
            this ValueTuple<T, T, T, T, T, T, T> valueTuple)
        {
            return valueTuple.ToList();
        }

        #endregion
    }
}

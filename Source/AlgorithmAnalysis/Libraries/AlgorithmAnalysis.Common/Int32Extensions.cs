using System;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common
{
    public static class Int32Extensions
    {
        public static TEnum AsEnum<TEnum>(this int intValue)
            where TEnum : struct, Enum
        {
            return CastTo<TEnum>.From(intValue);
        }

        public static TEnum AsEnum<TEnum>(this int intValue, int offset)
            where TEnum : struct, Enum
        {
            return CastTo<TEnum>.From(intValue + offset);
        }

        public static int UseOneBasedIndexing(this int zeroBasedIndex)
        {
            zeroBasedIndex.ThrowIfValueIsOutOfRange(nameof(zeroBasedIndex), 0, int.MaxValue);

            return zeroBasedIndex + 1;
        }
    }
}

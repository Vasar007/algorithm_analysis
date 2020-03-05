using System;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common
{
    public static class EnumExtensions
    {
        public static int AsInt32<TEnum>(this TEnum enumValue)
            where TEnum : struct, Enum
        {
            return Convert.ToInt32(enumValue);
        }

        public static int AsInt32<TEnum>(this TEnum enumValue, int offset)
            where TEnum : struct, Enum
        {
            return Convert.ToInt32(enumValue) + offset;
        }

        public static int AsInt32Strict<TEnum>(this TEnum enumValue)
            where TEnum : struct, Enum
        {
            enumValue.ThrowIfEnumValueIsUndefined(nameof(enumValue));

            return Convert.ToInt32(enumValue);
        }

        public static int AsInt32Strict<TEnum>(this TEnum enumValue, int offset)
            where TEnum : struct, Enum
        {
            enumValue.ThrowIfEnumValueIsUndefined(nameof(enumValue));

            return Convert.ToInt32(enumValue) + offset;
        }
    }
}

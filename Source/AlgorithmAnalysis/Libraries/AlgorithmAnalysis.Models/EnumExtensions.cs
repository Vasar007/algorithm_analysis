using System;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Models
{
    public static class EnumExtensions
    {
        public static int AsInt32<TEnum>(this TEnum enumValue)
            where TEnum : struct, Enum
        {
            enumValue.ThrowIfEnumValueIsUndefined(nameof(enumValue));

            return Convert.ToInt32(enumValue);
        }

        public static int AsInt32<TEnum>(this TEnum enumValue, int shift)
            where TEnum : struct, Enum
        {
            enumValue.ThrowIfEnumValueIsUndefined(nameof(enumValue));

            return Convert.ToInt32(enumValue) + shift;
        }
    }
}

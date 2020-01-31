﻿using System;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.DomainLogic
{
    internal static class EnumExtensions
    {
        public static int AsInt32<TEnum>(this TEnum enumValue)
            where TEnum : struct, Enum
        {
            enumValue.ThrowIfEnumValueIsUndefined(nameof(enumValue));

            return Convert.ToInt32(enumValue);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Acolyte.Assertions;
using Acolyte.Collections;

namespace Acolyte.Common
{
    public static class EnumHelperEx
    {
        public static IReadOnlyList<string> GetAllEnumDescriptionValues<TEnum>()
            where TEnum : struct, Enum
        {
            return EnumerableExtensions.ToReadOnlyList(EnumHelper.GetValues<TEnum>()
                .Select(enumValue => enumValue.GetDescription())
);
        }

        public static TEnum GetEnumValueByDescription<TEnum>(string enumDescription)
            where TEnum : struct, Enum
        {
            enumDescription.ThrowIfNull(nameof(enumDescription));

            TEnum enumResult = EnumHelper.GetValues<TEnum>()
                .Select(enumValue => (enumValue: enumValue, description: enumValue.GetDescription()))
                .First(pair => StringComparer.OrdinalIgnoreCase.Equals(pair.description, enumDescription))
                .enumValue;

            return enumResult;
        }
    }
}

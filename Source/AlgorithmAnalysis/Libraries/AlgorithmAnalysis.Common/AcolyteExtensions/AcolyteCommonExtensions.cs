using System.Collections.Generic;
using Acolyte.Assertions;

namespace Acolyte.Common
{
    public static class AcolyteCommonExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this T value)
        {
            value.ThrowIfNullValue(nameof(value), assertOnPureValueTypes: false);

            return new[] { value };
        }
    }
}

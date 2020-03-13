using System;
using System.Linq;

namespace Acolyte.Assertions
{
    public static class AcolyteAssertionsExtensions
    {
        public static T ThrowIfNull<T>(this T? value, string paramName)
             where T : struct
        {
            paramName.ThrowIfNullOrWhiteSpace(nameof(paramName));

            if (!value.HasValue)
            {
                throw new ArgumentNullException(paramName);
            }

            return value.Value;
        }

        public static T[][] ThrowIfDimensionsAreDifferent<T>(this T[][] jaggedArray,
            string paramName)
        {
            paramName.ThrowIfNullOrWhiteSpace(nameof(paramName));
            jaggedArray.ThrowIfNull(nameof(jaggedArray));

            if (jaggedArray.Length == 0) return jaggedArray;

            int fistArrayLength = jaggedArray[0].Length;
            bool allArrayLengthsAreEqual = jaggedArray
                .Skip(1)
                .All(array => fistArrayLength == array.Length);

            if (!allArrayLengthsAreEqual)
            {
                throw new ArgumentException(
                    $"{paramName} contains arrays with different lengths.", paramName
                );
            }

            return jaggedArray;
        }
    }
}

using System;

namespace Acolyte.Assertions
{
    public static class AcolyteExtensions
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
    }
}

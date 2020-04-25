using System;
using Acolyte.Assertions;
using Acolyte.Common;

namespace AlgorithmAnalysis.DesktopApp.Domain.Validation
{
    internal static class ValidationHelper
    {
        public static void AssertIf(bool condition, string message)
        {
            if (condition)
            {
                throw new ApplicationException(message);
            }
        }

        public static void AssertIfGotNullValueFromCollection<T>(T valueToCheck,
            string collectionName)
        {
            collectionName.ThrowIfNull(nameof(collectionName));

            string message =
                "Failed to retrive value from collection. " +
                $"Collection: '{collectionName}'.";
            AssertIf(valueToCheck is null, message);
        }

        public static void AssertIfGotUnknownValue<T>(T valueToCheck, string valueName)
            where T : struct, Enum
        {
            valueName.ThrowIfNull(nameof(valueName));

            string message = $"Got unknown value: '{valueName}'.";
            AssertIf(!valueToCheck.IsDefined(), message);
        }
    }
}

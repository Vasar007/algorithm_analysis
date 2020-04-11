using System;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.DesktopApp.Domain.Validation
{
    internal static class ValidationHelper
    {
        public static void AssertIfNull<T>(T valueToCheck, string collectionName)
        {
            collectionName.ThrowIfNull(nameof(collectionName));

            if (valueToCheck is null)
            {
                string message =
                    "Failed to retrive value from collection. " +
                    $"Collection: {collectionName}";
                throw new ApplicationException(message);
            }
        }

        public static void AssertIf(bool condition, string message)
        {
            if (condition)
            {
                throw new ApplicationException(message);
            }
        }
    }
}

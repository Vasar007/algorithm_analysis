using System;
using System.Linq;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Files
{
    public sealed class SpecificPathResolver
    {
        public SpecificPathResolver()
        {
        }

        public string Resolve(string unresolvedPath)
        {
            unresolvedPath.ThrowIfNullOrWhiteSpace(nameof(unresolvedPath));

            if (ShouldBeParsed(unresolvedPath))
            {
                unresolvedPath = ParseSpecificPath(unresolvedPath);
            }

            return unresolvedPath;
        }

        private static bool ShouldBeParsed(string unresolvedPath)
        {
            int startingCharsNumber = unresolvedPath.Count(
                ch => ch.Equals(CommonConstants.SpecificStartingChar)
            );
            int endingCharsNumber = unresolvedPath.Count(
                ch => ch.Equals(CommonConstants.SpecificEndingChar)
            );

            if (startingCharsNumber == 0 && endingCharsNumber == 0) return false;

            // Now supported only one special expression.
            if (startingCharsNumber == 1 && endingCharsNumber == 1) return true;

            string message =
                "Failed to parse log folder path: invalid path format. " +
                $"Argument: {unresolvedPath}";
            throw new ArgumentException(message, nameof(unresolvedPath));
        }

        public static string ParseSpecificPath(string unresolvedPath)
        {
            if (unresolvedPath.Contains(CommonConstants.CommonApplicationData))
            {
                string valueToReplace =
                    CommonConstants.SpecificStartingChar +
                    CommonConstants.CommonApplicationData +
                    CommonConstants.SpecificEndingChar;

                string newValue = Environment.GetFolderPath(
                    Environment.SpecialFolder.CommonApplicationData
                );

                return unresolvedPath.Replace(valueToReplace, newValue);
            }

            string message =
                "Failed to parse log folder path: invalid specific path value. " +
                $"Argument: {unresolvedPath}";
            throw new ArgumentException(message, nameof(unresolvedPath));
        }
    }
}

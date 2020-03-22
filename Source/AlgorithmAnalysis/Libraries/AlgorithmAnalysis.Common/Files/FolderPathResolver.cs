using System;
using System.Linq;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Files
{
    public sealed class FolderPathResolver
    {
        public FolderPathResolver()
        {
        }

        public string Resolve(string unresolvedPath)
        {
            unresolvedPath.ThrowIfNullOrWhiteSpace(nameof(unresolvedPath));

            if (ShouldBeParsed(unresolvedPath))
            {
                unresolvedPath = ParseSpecialFolder(unresolvedPath);
            }

            return unresolvedPath;
        }

        private static bool ShouldBeParsed(string unresolvedPath)
        {
            int startingCharsNumber = unresolvedPath.Count(
                ch => ch.Equals(CommonConstants.SpecialStartingChar)
            );
            int endingCharsNumber = unresolvedPath.Count(
                ch => ch.Equals(CommonConstants.SpecialEndingChar)
            );

            if (startingCharsNumber == 0 && endingCharsNumber == 0) return false;

            // Now supported only one special expression.
            if (startingCharsNumber == 1 && endingCharsNumber == 1) return true;

            string message = "Failed to parse log folder path: invalid path format.";
            throw new ArgumentException(message, nameof(unresolvedPath));
        }

        public static string ParseSpecialFolder(string unresolvedPath)
        {
            if (unresolvedPath.Contains(CommonConstants.CommonApplicationData))
            {
                string valueToReplace =
                    CommonConstants.SpecialStartingChar +
                    CommonConstants.CommonApplicationData +
                    CommonConstants.SpecialEndingChar;

                string newValue = Environment.GetFolderPath(
                    Environment.SpecialFolder.CommonApplicationData
                );

                return unresolvedPath.Replace(valueToReplace, newValue);
            }

            string message = "Failed to parse log folder path: invalid special folder value.";
            throw new ArgumentException(message, nameof(unresolvedPath));
        }
    }
}

using System;
using System.Linq;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Files
{
    public sealed class SpecificPathResolver : IPathResolver
    {
        private readonly SpecificValueProcessor _specificProcessor;

        public char SpecificStartingChar { get; }

        public char SpecificEndingChar { get; }


        public SpecificPathResolver(
            char specialStartingChar,
            char specialEndingChar)
        {
            SpecificStartingChar = specialStartingChar;
            SpecificEndingChar = specialEndingChar;

            _specificProcessor = new SpecificValueProcessor(specialStartingChar, specialEndingChar);
        }

        public static SpecificPathResolver CreateDefault()
        {
            return new SpecificPathResolver(
                specialStartingChar: CommonConstants.SpecificStartingChar,
                specialEndingChar: CommonConstants.SpecificEndingChar
            );
        }

        #region IPathResolver Implementation

        public string Resolve(string unresolvedPath, PathResolutionOptions? options)
        {
            unresolvedPath.ThrowIfNullOrWhiteSpace(nameof(unresolvedPath));
            options ??= new PathResolutionOptions();

            string resolvedPath = unresolvedPath;
            if (ShouldBeParsed(unresolvedPath))
            {
                // Currently, we support only specific value for common application data.
                resolvedPath = ParseSpecificPath(
                    unresolvedPath, specificValue: CommonConstants.CommonApplicationData, options
                );
            }

            return options.UnifyDirectorySeparatorChars
                ? PathHelper.UnifyDirectorySeparatorChars(resolvedPath)
                : resolvedPath;
        }

        public string Resolve(string unresolvedPath)
        {
            return Resolve(unresolvedPath, options: null);
        }

        #endregion

        private bool ShouldBeParsed(string unresolvedPath)
        {
            int startingCharsNumber = unresolvedPath.Count(
                ch => ch.Equals(SpecificStartingChar)
            );
            int endingCharsNumber = unresolvedPath.Count(
                ch => ch.Equals(SpecificEndingChar)
            );

            if (startingCharsNumber == 0 && endingCharsNumber == 0) return false;

            // Now supported only one special expression.
            if (startingCharsNumber == 1 && endingCharsNumber == 1) return true;

            string message =
                "Failed to parse log folder path: invalid path format. " +
                $"Argument: {unresolvedPath}";
            throw new ArgumentException(message, nameof(unresolvedPath));
        }

        private string ParseSpecificPath(string unresolvedPath, string specificValue,
            PathResolutionOptions options)
        {
            string wrappedSpecificValue = _specificProcessor.WrapSpecificValue(specificValue);

            bool containsSpecific = unresolvedPath.Contains(
                wrappedSpecificValue, StringComparison.InvariantCulture
            );

            if (containsSpecific)
            {
                if (options.ReturnRelativePath)
                {
                    string resolvedPath = unresolvedPath.Replace(
                        wrappedSpecificValue, string.Empty
                    );

                    return PathHelper.TrimStartDirectorySeparatorChar(resolvedPath);
                }

                string resolvedSpecificValue = _specificProcessor.ResolveSpecificValue(
                    specificValue
                );

                return unresolvedPath.Replace(wrappedSpecificValue, resolvedSpecificValue);
            }

            string message =
                "Failed to parse log folder path: invalid specific path value. " +
                $"Argument: {unresolvedPath}";
            throw new ArgumentException(message, nameof(unresolvedPath));
        }
    }
}

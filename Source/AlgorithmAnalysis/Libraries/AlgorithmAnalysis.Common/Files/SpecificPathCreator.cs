using System.IO;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Files
{
    public sealed class SpecificPathCreator
    {
        private readonly SpecificPathResolver _pathResolver;

        public string ApplicationName { get; }

        public char SpecificStartingChar { get; }

        public char SpecificEndingChar { get; }


        public SpecificPathCreator(
            string applicationName,
            char specialStartingChar,
            char specialEndingChar)
        {
            ApplicationName = applicationName.ThrowIfNullOrWhiteSpace(nameof(applicationName));
            SpecificStartingChar = specialStartingChar;
            SpecificEndingChar = specialEndingChar;

            _pathResolver = new SpecificPathResolver();
        }

        public static SpecificPathCreator CreateDefault()
        {
            return new SpecificPathCreator(
                applicationName: CommonConstants.ApplicationName,
                specialStartingChar: CommonConstants.SpecificStartingChar,
                specialEndingChar: CommonConstants.SpecificEndingChar
            );
        }

        public string CreateSpecificPath(string specificValue, string? path, bool appendAdppFolder)
        {
            specificValue.ThrowIfNullOrWhiteSpace(nameof(specificValue));

            string wrappedValue = WrapSpecificValue(specificValue);
            string resolvedPath = _pathResolver.Resolve(wrappedValue);

            return CombineFinalPath(resolvedPath, path, appendAdppFolder);
        }

        public string CreateSpecificPath(string specificValue, bool appendAdppFolder)
        {
            return CreateSpecificPath(specificValue, path: null, appendAdppFolder);
        }

        private string WrapSpecificValue(string specificValue)
        {
            return $"{SpecificStartingChar}{specificValue}{SpecificEndingChar}";
        }

        private string CombineFinalPath(string resolvedPath, string? path, bool appendAdppFolder)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return appendAdppFolder
                    ? Path.Combine(resolvedPath, ApplicationName)
                    : resolvedPath;
            }

            return appendAdppFolder
                ? Path.Combine(resolvedPath, ApplicationName, path)
                : Path.Combine(resolvedPath, path);
        }
    }
}

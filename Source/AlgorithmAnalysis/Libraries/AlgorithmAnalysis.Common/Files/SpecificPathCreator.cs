using System.IO;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Files
{
    public sealed class SpecificPathCreator : IPathCreator
    {
        private readonly IPathResolver _pathResolver;

        private readonly SpecificValueProcessor _specificProcessor;

        public string ApplicationName { get; }

        public char SpecificStartingChar { get; }

        public char SpecificEndingChar { get; }


        public SpecificPathCreator(
            IPathResolver pathResolver,
            string applicationName,
            char specialStartingChar,
            char specialEndingChar)
        {
            _pathResolver = pathResolver.ThrowIfNull(nameof(pathResolver));
            ApplicationName = applicationName.ThrowIfNullOrWhiteSpace(nameof(applicationName));
            SpecificStartingChar = specialStartingChar;
            SpecificEndingChar = specialEndingChar;

            _specificProcessor = new SpecificValueProcessor(specialStartingChar, specialEndingChar);
        }

        public static SpecificPathCreator CreateDefault()
        {
            char specialStartingChar = CommonConstants.SpecificStartingChar;
            char specialEndingChar = CommonConstants.SpecificEndingChar;

            var specificPathResolver = new SpecificPathResolver(
                specialStartingChar, specialEndingChar
            );

            return new SpecificPathCreator(
                pathResolver: specificPathResolver,
                applicationName: CommonConstants.ApplicationName,
                specialStartingChar: specialStartingChar,
                specialEndingChar: specialEndingChar
            );
        }

        #region IPathCreator Implementation

        public string CreateSpecificPath(string specificValue, string? additionalPath,
            PathCreationOptions? options)
        {
            specificValue.ThrowIfNullOrWhiteSpace(nameof(specificValue));
            options ??= new PathCreationOptions();

            string wrappedValue = _specificProcessor.WrapSpecificValue(specificValue);

            string mainPartOfPath = options.ShouldResolvePath
                ? _pathResolver.Resolve(wrappedValue)
                : wrappedValue;

            return CombineFinalPath(mainPartOfPath, additionalPath, options);
        }

        public string CreateSpecificPath(string specificValue,
            PathCreationOptions? options)
        {
            return CreateSpecificPath(specificValue, additionalPath: null, options);
        }

        public string CreateSpecificPath(string specificValue)
        {
            return CreateSpecificPath(specificValue, additionalPath: null, options: null);
        }

        #endregion

        private string CombineFinalPath(string mainPartOfPath, string? additionalPath,
            PathCreationOptions options)
        {
            if (string.IsNullOrWhiteSpace(additionalPath))
            {
                return options.AppendAppFolder
                    ? Path.Combine(mainPartOfPath, ApplicationName)
                    : mainPartOfPath;
            }

            return options.AppendAppFolder
                ? Path.Combine(mainPartOfPath, ApplicationName, additionalPath)
                : Path.Combine(mainPartOfPath, additionalPath);
        }
    }
}

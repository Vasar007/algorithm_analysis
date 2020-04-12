using System;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Files
{
    internal sealed class SpecificValueProcessor
    {
        public char SpecificStartingChar { get; }

        public char SpecificEndingChar { get; }


        public SpecificValueProcessor(
            char specialStartingChar,
            char specialEndingChar)
        {
            SpecificStartingChar = specialStartingChar;
            SpecificEndingChar = specialEndingChar;

            
        }

        public static SpecificValueProcessor CreateDefault()
        {
            return new SpecificValueProcessor(
                specialStartingChar: CommonConstants.SpecificStartingChar,
                specialEndingChar: CommonConstants.SpecificEndingChar
            );
        }

        public string WrapSpecificValue(string specificValue)
        {
            specificValue.ThrowIfNullOrEmpty(nameof(specificValue));

            return $"{SpecificStartingChar}{specificValue}{SpecificEndingChar}";
        }

        public string ResolveSpecificValue(string specificValue)
        {
            specificValue.ThrowIfNullOrEmpty(nameof(specificValue));

            bool isCommonApplicationData = StringComparer.InvariantCulture.Equals(
                specificValue, CommonConstants.CommonApplicationData
            );
            if (isCommonApplicationData)
            {
                return Environment.GetFolderPath(
                    Environment.SpecialFolder.CommonApplicationData
                 );
            }

            throw new ArgumentOutOfRangeException(
                nameof(specificValue), specificValue,
                $"Unknown specific value: '{specificValue}'."
            );
        }
    }
}

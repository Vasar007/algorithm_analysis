using System;
using System.Text;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common.Parsing;

namespace AlgorithmAnalysis.Models
{
    public sealed class AlgorithmType : DescriptiveBase, IEquatable<DescriptiveBase>,
        IEquatable<AlgorithmType>, ILoggable
    {
        public string MinFormulaFormat { get; }

        public string AverageFormulaFormat { get; }

        public string MaxFormulaFormat { get; }

        // Contract: the analysis program is located in the same directory as our app.
        public string AnalysisProgramName { get; }

        public string OutputFilenamePattern { get; }


        private AlgorithmType(
            string description,
            int value,
            string minFormulaFormat,
            string averageFormulaFormat,
            string maxFormulaFormat,
            string analysisProgramName,
            string outputFilenamePattern)
            : base(description, value)
        {
            MinFormulaFormat = minFormulaFormat.ThrowIfNullOrWhiteSpace(nameof(minFormulaFormat));
            AverageFormulaFormat = averageFormulaFormat.ThrowIfNullOrWhiteSpace(nameof(averageFormulaFormat));
            MaxFormulaFormat = maxFormulaFormat.ThrowIfNullOrWhiteSpace(nameof(maxFormulaFormat));
            AnalysisProgramName = analysisProgramName.ThrowIfNullOrWhiteSpace(nameof(analysisProgramName));
            OutputFilenamePattern = outputFilenamePattern.ThrowIfNullOrWhiteSpace(nameof(outputFilenamePattern));
        }

        public static AlgorithmType Create(AlgorithmTypeValue algorithmValue)
        {
            string minFormulaFormat = ParsingManager.TransformRawFormulaToFormulaFormat(
                algorithmValue.MinFormula
            );

            string averageFormulaFormat = ParsingManager.TransformRawFormulaToFormulaFormat(
                algorithmValue.AverageFormula
            );

            string maxFormulaFormat = ParsingManager.TransformRawFormulaToFormulaFormat(
                algorithmValue.MaxFormula
            );

            return new AlgorithmType(
                description: algorithmValue.Description,
                value: algorithmValue.Value,
                minFormulaFormat: minFormulaFormat,
                averageFormulaFormat: averageFormulaFormat,
                maxFormulaFormat: maxFormulaFormat,
                analysisProgramName: algorithmValue.AnalysisProgramName,
                outputFilenamePattern: algorithmValue.OutputFilenamePattern
            );
        }

        #region Object Overridden Methods

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AlgorithmType);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IEquatable<AlgorithmType> Implementation

        public bool Equals(AlgorithmType? other)
        {
            if (other is null) return false;

            if (ReferenceEquals(this, other)) return true;

            return base.IsEqual(other);
        }

        #endregion

        public static bool operator ==(AlgorithmType? left, AlgorithmType? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AlgorithmType? left, AlgorithmType? right)
        {
            return !(left == right);
        }

        #region ILoggable Implementation

        public override string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(AlgorithmType)}]")
                .AppendLine($"Description: {Description}")
                .AppendLine($"Value: '{Value.ToString()}'")
                .AppendLine($"MinFormulaFormat: '{MinFormulaFormat}'")
                .AppendLine($"AverageFormulaFormat: '{AverageFormulaFormat}'")
                .AppendLine($"MaxFormulaFormat: '{MaxFormulaFormat}'");

            return sb.ToString();
        }

        #endregion
    }
}

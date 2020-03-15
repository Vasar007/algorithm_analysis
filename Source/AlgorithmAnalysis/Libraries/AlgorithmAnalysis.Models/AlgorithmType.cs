using System;
using System.Text;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Models
{
    public sealed class AlgorithmType : DescriptiveBase, IEquatable<DescriptiveBase>,
        IEquatable<AlgorithmType>, ILoggable
    {
        public string MinFormulaFormat { get; }

        public string AverageFormulaFormat { get; }

        public string MaxFormulaFormat { get; }


        private AlgorithmType(
            string description,
            int value,
            string minFormulaFormat,
            string averageFormulaFormat,
            string maxFormulaFormat)
            : base(description, value)
        {
            MinFormulaFormat = minFormulaFormat.ThrowIfNullOrWhiteSpace(nameof(minFormulaFormat));
            AverageFormulaFormat = averageFormulaFormat.ThrowIfNullOrWhiteSpace(nameof(averageFormulaFormat));
            MaxFormulaFormat = maxFormulaFormat.ThrowIfNullOrWhiteSpace(nameof(maxFormulaFormat));
        }

        public static AlgorithmType Create(AlgorithmTypeValue algorithmValue)
        {
            return new AlgorithmType(
                description: algorithmValue.Description,
                value: algorithmValue.Value,
                minFormulaFormat: TransformRawFormulaToFormulaFormat(algorithmValue.MinFormula),
                averageFormulaFormat: TransformRawFormulaToFormulaFormat(algorithmValue.AverageFormula),
                maxFormulaFormat: TransformRawFormulaToFormulaFormat(algorithmValue.MaxFormula)
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

        private static string TransformRawFormulaToFormulaFormat(string rawFormula)
        {
            rawFormula.ThrowIfNullOrWhiteSpace(nameof(rawFormula));

            return rawFormula.Replace("x", "{0}", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}

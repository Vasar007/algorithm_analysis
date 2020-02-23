using System;
using System.Text;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class PhaseOnePartOneAnalysisKind : DescriptiveBase, IEquatable<DescriptiveBase>,
        IEquatable<PhaseOnePartOneAnalysisKind>, ILoggable
    {
        public static PhaseOnePartOneAnalysisKind NormalDistribution { get; } = 
            new PhaseOnePartOneAnalysisKind("Normal distribution", 1);

        public static PhaseOnePartOneAnalysisKind BetaDistribution { get; } =
            new PhaseOnePartOneAnalysisKind("Beta distribution", 2);


        private PhaseOnePartOneAnalysisKind(string description, int value)
            : base(description, value)
        {
        }

        #region Object Overridden Methods

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PhaseOnePartOneAnalysisKind);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IEquatable<PhaseOnePartOneAnalysisKind> Implementation

        public bool Equals(PhaseOnePartOneAnalysisKind? other)
        {
            if (other is null) return false;

            if (ReferenceEquals(this, other)) return true;

            return base.IsEqual(other);
        }

        #endregion

        public static bool operator ==(PhaseOnePartOneAnalysisKind? left, PhaseOnePartOneAnalysisKind? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PhaseOnePartOneAnalysisKind? left, PhaseOnePartOneAnalysisKind? right)
        {
            return !(left == right);
        }

        #region ILoggable Implementation

        public override string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(PhaseOnePartOneAnalysisKind)}]")
                .AppendLine($"Description: {Description.ToString()}")
                .AppendLine($"Value: '{Value.ToString()}'");

            return sb.ToString();
        }

        #endregion
    }
}

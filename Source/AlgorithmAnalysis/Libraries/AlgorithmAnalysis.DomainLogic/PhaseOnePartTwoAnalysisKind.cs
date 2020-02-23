using System;
using System.Text;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class PhaseOnePartTwoAnalysisKind : DescriptiveBase, IEquatable<DescriptiveBase>,
        IEquatable<PhaseOnePartTwoAnalysisKind>, ILoggable
    {
        public static PhaseOnePartTwoAnalysisKind BetaDistributionWithScott { get; } =
            new PhaseOnePartTwoAnalysisKind("Beta distribution with Scott's segments formula", 1);

        public static PhaseOnePartTwoAnalysisKind BetaDistributionWithSturges { get; } =
            new PhaseOnePartTwoAnalysisKind("Beta distribution with Sturges's segments formula", 2);


        private PhaseOnePartTwoAnalysisKind(string description, int value)
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
            return Equals(obj as PhaseOnePartTwoAnalysisKind);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IEquatable<PhaseOnePartTwoAnalysisKind> Implementation

        public bool Equals(PhaseOnePartTwoAnalysisKind? other)
        {
            if (other is null) return false;

            if (ReferenceEquals(this, other)) return true;

            return base.IsEqual(other);
        }

        #endregion

        public static bool operator ==(PhaseOnePartTwoAnalysisKind? left, PhaseOnePartTwoAnalysisKind? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PhaseOnePartTwoAnalysisKind? left, PhaseOnePartTwoAnalysisKind? right)
        {
            return !(left == right);
        }

        #region ILoggable Implementation

        public override string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(PhaseOnePartTwoAnalysisKind)}]")
                .AppendLine($"Description: {Description.ToString()}")
                .AppendLine($"Value: '{Value.ToString()}'");

            return sb.ToString();
        }

        #endregion
    }
}

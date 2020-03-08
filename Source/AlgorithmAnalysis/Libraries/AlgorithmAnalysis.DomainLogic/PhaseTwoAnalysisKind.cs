using System;
using System.Text;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class PhaseTwoAnalysisKind : DescriptiveBase, IEquatable<DescriptiveBase>,
        IEquatable<PhaseTwoAnalysisKind>, ILoggable
    {
        public static PhaseTwoAnalysisKind NormalDistribution { get; } = 
            new PhaseTwoAnalysisKind("Normal distribution", 1);

        public static PhaseTwoAnalysisKind BetaDistribution { get; } =
            new PhaseTwoAnalysisKind("Beta distribution", 2);


        private PhaseTwoAnalysisKind(string description, int value)
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
            return Equals(obj as PhaseTwoAnalysisKind);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IEquatable<PhaseTwoAnalysisKind> Implementation

        public bool Equals(PhaseTwoAnalysisKind? other)
        {
            if (other is null) return false;

            if (ReferenceEquals(this, other)) return true;

            return base.IsEqual(other);
        }

        #endregion

        public static bool operator ==(PhaseTwoAnalysisKind? left,
            PhaseTwoAnalysisKind? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PhaseTwoAnalysisKind? left,
            PhaseTwoAnalysisKind? right)
        {
            return !(left == right);
        }

        #region ILoggable Implementation

        public override string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(PhaseTwoAnalysisKind)}]")
                .AppendLine($"Description: {Description.ToString()}")
                .AppendLine($"Value: '{Value.ToString()}'");

            return sb.ToString();
        }

        #endregion
    }
}

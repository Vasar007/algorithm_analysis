using System;
using System.Text;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Math
{
    public sealed class GoodnessOfFitKind : DescriptiveBase, IEquatable<DescriptiveBase>,
        IEquatable<GoodnessOfFitKind>, ILoggable
    {
        public static GoodnessOfFitKind CoefficientOfDetermination { get; } = 
            new GoodnessOfFitKind("Coefficient of determination", 1);

        public static GoodnessOfFitKind RSquaredValue { get; } =
            new GoodnessOfFitKind("R\u00B2 value (Pearson's coefficient)", 2);


        private GoodnessOfFitKind(string description, int value)
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
            return Equals(obj as GoodnessOfFitKind);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IEquatable<GoodnessOfFitKind> Implementation

        public bool Equals(GoodnessOfFitKind? other)
        {
            if (other is null) return false;

            if (ReferenceEquals(this, other)) return true;

            return base.IsEqual(other);
        }

        #endregion

        public static bool operator ==(GoodnessOfFitKind? left,
            GoodnessOfFitKind? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GoodnessOfFitKind? left,
            GoodnessOfFitKind? right)
        {
            return !(left == right);
        }

        #region ILoggable Implementation

        public override string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(GoodnessOfFitKind)}]")
                .AppendLine($"Description: {Description}")
                .AppendLine($"Value: '{Value.ToString()}'");

            return sb.ToString();
        }

        #endregion
    }
}

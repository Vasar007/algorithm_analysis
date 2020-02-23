using System;
using System.Text;

namespace AlgorithmAnalysis.Models
{
    public sealed class AlgorithmType : DescriptiveBase, IEquatable<DescriptiveBase>,
        IEquatable<AlgorithmType>, ILoggable
    {
        public AlgorithmType(string description, int value)
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
                .AppendLine($"Description: {Description.ToString()}")
                .AppendLine($"Value: '{Value.ToString()}'");

            return sb.ToString();
        }

        #endregion
    }
}

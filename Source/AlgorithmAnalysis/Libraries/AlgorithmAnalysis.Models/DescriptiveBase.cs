using System;
using System.Text;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Models
{
    public abstract class DescriptiveBase : IEquatable<DescriptiveBase>, ILoggable
    {
        public string Description { get; }

        public int Value { get; }


        protected DescriptiveBase(string description, int value)
        {
            Description = description.ThrowIfNullOrWhiteSpace(nameof(description));
            Value = value.ThrowIfValueIsOutOfRange(nameof(value), 0, int.MaxValue);
        }

        #region Object Overridden Methods

        public override string ToString()
        {
            return Description;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DescriptiveBase);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Description, Value);
        }

        #endregion

        #region IEquatable<DescriptiveBase> Implementation

        public bool Equals(DescriptiveBase? other)
        {
            if (other is null) return false;

            if (ReferenceEquals(this, other)) return true;

            return IsEqual(other);
        }

        #endregion

        public static bool operator ==(DescriptiveBase? left, DescriptiveBase? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DescriptiveBase? left, DescriptiveBase? right)
        {
            return !(left == right);
        }

        #region ILoggable Implementation

        public virtual string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(DescriptiveBase)}]")
                .AppendLine($"Description: {Description.ToString()}")
                .AppendLine($"Value: '{Value.ToString()}'");

            return sb.ToString();
        }

        #endregion

        protected bool IsEqual(DescriptiveBase other)
        {
            return StringComparer.InvariantCulture.Equals(Description, other.Description) &&
                   Value.Equals(other.Value);
        }
    }
}

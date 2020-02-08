using System;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Models
{
    public sealed class AlgorithmType : IEquatable<AlgorithmType>
    {
        public string Description { get; }

        public int Value { get; }


        public AlgorithmType(string description, int value)
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
            return Equals(obj as AlgorithmType);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Description, Value);
        }

        #endregion

        #region IEquatable<AlgorithmType> Implementation

        public bool Equals(AlgorithmType? other)
        {
            if (other is null) return false;

            if (ReferenceEquals(this, other)) return true;

            return StringComparer.InvariantCulture.Equals(Description, other.Description) &&
                   Value.Equals(other.Value);
        }

        #endregion
    }
}

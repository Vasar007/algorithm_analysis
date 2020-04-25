using System.Collections.Generic;
using System.Linq;
using Acolyte.Assertions;
using Acolyte.Collections;

namespace AlgorithmAnalysis.Models
{
    public static class AlgorithmTypeExtensions
    {
        public static IReadOnlyList<AlgorithmType> GetAlgorithmTypes(
            this IEnumerable<AlgorithmTypeValue> algorithmTypeValues)
        {
            algorithmTypeValues.ThrowIfNull(nameof(algorithmTypeValues));

            return algorithmTypeValues
                .Select(AlgorithmType.Create)
                .ToReadOnlyList();
        }

        public static List<AlgorithmTypeValue> GetAlgorithmTypeValues(
            this IEnumerable<AlgorithmType> algorithmTypes)
        {
            algorithmTypes.ThrowIfNull(nameof(algorithmTypes));

            return algorithmTypes
                .Select(AlgorithmTypeValue.Create)
                .ToList();
        }
    }
}

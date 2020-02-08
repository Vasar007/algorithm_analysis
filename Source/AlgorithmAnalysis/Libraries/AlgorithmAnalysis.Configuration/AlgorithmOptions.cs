using System.Collections.Generic;
using System.Linq;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Configuration
{
    public sealed class AlgorithmOptions : IOptions
    {
        public Dictionary<string, string> AvailableAlgorithms { get; set; } =
            new Dictionary<string, string>();


        public AlgorithmOptions()
        {
        }

        public IReadOnlyList<AlgorithmType> GetAlgorithmTypes()
        {
            return AvailableAlgorithms
                .Select(rawValue => new AlgorithmType(rawValue.Value, int.Parse(rawValue.Key)))
                .ToList();
        }
    }
}

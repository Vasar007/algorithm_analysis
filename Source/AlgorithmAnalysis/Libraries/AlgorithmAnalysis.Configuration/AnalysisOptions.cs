using System.Collections.Generic;
using System.Linq;
using Acolyte.Collections;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Configuration
{
    public sealed class AnalysisOptions : IOptions
    {
        public List<AlgorithmTypeValue> AvailableAlgorithms { get; set; } =
            new List<AlgorithmTypeValue>();

        public string CommonAnalysisFilenameSuffix { get; set; } = "series";

        public string OutputFileExtension { get; set; } = ".txt";


        public AnalysisOptions()
        {
        }

        public IReadOnlyList<AlgorithmType> GetAlgorithmTypes()
        {
            return AvailableAlgorithms
                .Select(AlgorithmType.Create)
                .ToReadOnlyList();
        }
    }
}

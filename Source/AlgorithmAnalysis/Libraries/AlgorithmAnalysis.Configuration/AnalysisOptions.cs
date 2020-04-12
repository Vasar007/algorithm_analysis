using System.Collections.Generic;
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
    }
}

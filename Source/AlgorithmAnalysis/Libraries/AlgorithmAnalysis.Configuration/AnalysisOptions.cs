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

        // Contract: the analysis program is located in the same directory as our app.
        public string AnalysisProgramName { get; set; } = "algorithm_analysis.exe";

        public string OutputFilenamePattern { get; set; } = "tests_average_";

        public string CommonAnalysisFilenameSuffix { get; set; } = "series";


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

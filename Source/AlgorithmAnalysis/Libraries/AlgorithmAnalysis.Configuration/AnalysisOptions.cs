using System.Collections.Generic;
using System.Linq;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Configuration
{
    public sealed class AnalysisOptions : IOptions
    {
        public Dictionary<string, string> AvailableAlgorithms { get; set; } =
            new Dictionary<string, string>();

        // Contract: the analysis program is located in the same directory as our app.
        public string DefaultAnalysisProgramName { get; } = "algorithm_analysis.exe";

        public string DefaultOutputFilenamePattern { get; } = "tests_average_";


        public AnalysisOptions()
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

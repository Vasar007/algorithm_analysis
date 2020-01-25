using System.Collections.Generic;

namespace AlgorithmAnalysis.DesktopApp.Domain
{
    internal static class DesktopOptions
    {
        public static string Title { get; } = "Algorithm analysis";

        public static string AnalysisProgramName { get; } = "algorithm_analysis.exe";

        public static string DefaultOutputFilename { get; } = "tests_average_";

        public static IReadOnlyList<string> AvailableAlgorithms { get; } = new[] { "A1", "A2" };
    }
}

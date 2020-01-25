using System.Collections.Generic;

namespace AlgorithmAnalysis.DesktopApp.Domain
{
    internal static class DesktopOptions
    {
        public static string Title { get; } = "Algorithm analysis";

        // Contract: the analysis program is located in the same directory as our app.
        public static string DefaultAnalysisProgramName { get; } = "algorithm_analysis.exe";

        public static string DefaultOutputFilenamePattern { get; } = "tests_average_";

        // TODO: refactor algorythm type related logic.
        public static IReadOnlyList<string> AvailableAlgorithms { get; } = new[]
        {
            "Pallottino's algorithm",
            "Insertion sort"
        };
    }
}

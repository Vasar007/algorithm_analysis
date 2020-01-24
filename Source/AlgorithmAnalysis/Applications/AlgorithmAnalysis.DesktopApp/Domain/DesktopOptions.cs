using System.Collections.Generic;

namespace AlgorithmAnalysis.DesktopApp.Domain
{
    internal static class DesktopOptions
    {
        public static string Title { get; } = "Algorithm analysis";

        public static IReadOnlyList<string> AvailableAlgorithms { get; } = new[] { "A1", "A2" };
    }
}

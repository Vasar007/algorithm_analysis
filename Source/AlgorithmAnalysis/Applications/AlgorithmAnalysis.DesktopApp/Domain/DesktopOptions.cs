using System.Collections.Generic;
using AlgorithmAnalysis.DomainLogic;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DesktopApp.Domain
{
    internal static class DesktopOptions
    {
        public static string Title { get; } = "Algorithm analysis";

        // Contract: the analysis program is located in the same directory as our app.
        public static string DefaultAnalysisProgramName { get; } = "algorithm_analysis.exe";

        public static string DefaultOutputFilenamePattern { get; } = "tests_average_";

        public static IReadOnlyList<string> AvailableAnalysisKindForPhaseOne { get; } =
            AnalysisHelper.GetAvailableAnalysisKindForPhaseOnePartOne();

        public static IReadOnlyList<string> AvailableAnalysisKindForPhaseTwo { get; } =
         AnalysisHelper.GetAvailableAnalysisKindForPhaseOnePartTwo();

        public static IReadOnlyList<AlgorithmType> AvailableAlgorithms { get; } =
            AnalysisHelper.GetAvailableAlgorithms();

        public static string FinalExcelFilename { get; } = "test.xlsx";
    }
}

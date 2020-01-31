﻿using System.Collections.Generic;
using AlgorithmAnalysis.DomainLogic;

namespace AlgorithmAnalysis.DesktopApp.Domain
{
    internal static class DesktopOptions
    {
        public static string Title { get; } = "Algorithm analysis";

        // Contract: the analysis program is located in the same directory as our app.
        public static string DefaultAnalysisProgramName { get; } = "algorithm_analysis.exe";

        public static string DefaultOutputFilenamePattern { get; } = "tests_average_";

        public static IReadOnlyList<string> AvailableAnalysisKindForPhaseOne { get; } =
            AnalysisHelper.GetAvailableAnalysisKindForPhaseOne();

        // TODO: refactor algorythm type related logic.
        public static IReadOnlyList<string> AvailableAlgorithms { get; } = new[]
        {
            "Pallottino's algorithm",
            "Insertion sort"
        };

        public static string FinalExcelFilename { get; } = @"test.xlsx";
    }
}

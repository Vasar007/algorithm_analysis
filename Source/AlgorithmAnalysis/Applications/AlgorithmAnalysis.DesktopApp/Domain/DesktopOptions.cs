using System.Collections.Generic;
using AlgorithmAnalysis.DomainLogic;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DesktopApp.Domain
{
    internal static class DesktopOptions
    {
        public static string Title { get; } = "Algorithm analysis";

        public static IReadOnlyList<string> AvailableAnalysisKindForPhaseOne { get; } =
            AnalysisHelper.GetAvailableAnalysisKindForPhaseOnePartOne();

        public static IReadOnlyList<string> AvailableAnalysisKindForPhaseTwo { get; } =
            AnalysisHelper.GetAvailableAnalysisKindForPhaseOnePartTwo();

        public static IReadOnlyList<AlgorithmType> AvailableAlgorithms { get; } =
            AnalysisHelper.GetAvailableAlgorithms();
    }
}

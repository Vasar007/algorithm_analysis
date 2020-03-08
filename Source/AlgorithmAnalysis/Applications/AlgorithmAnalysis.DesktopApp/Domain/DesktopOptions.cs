using System.Collections.Generic;
using AlgorithmAnalysis.DomainLogic;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DesktopApp.Domain
{
    internal static class DesktopOptions
    {
        public static string Title { get; } = "Algorithm analysis";

        public static IReadOnlyList<PhaseOnePartOneAnalysisKind> AvailableAnalysisKindForPhaseOnePartOne { get; } =
            AnalysisHelper.GetAvailableAnalysisKindForPhaseOnePartOne();

        public static IReadOnlyList<PhaseOnePartTwoAnalysisKind> AvailableAnalysisKindForPhaseOnePartTwo { get; } =
            AnalysisHelper.GetAvailableAnalysisKindForPhaseOnePartTwo();

        public static IReadOnlyList<PhaseTwoAnalysisKind> AvailableAnalysisKindForPhaseTwo { get; } =
            AnalysisHelper.GetAvailableAnalysisKindForPhaseTwo();

        public static IReadOnlyList<AlgorithmType> AvailableAlgorithms =>
            AnalysisHelper.GetAvailableAlgorithms();
    }
}

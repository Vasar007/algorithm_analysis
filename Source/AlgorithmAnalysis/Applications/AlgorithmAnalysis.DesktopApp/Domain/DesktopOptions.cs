using System;
using System.Collections.Generic;
using AlgorithmAnalysis.DomainLogic;
using AlgorithmAnalysis.Math;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DesktopApp.Domain
{
    internal static class DesktopOptions
    {
        public static string Title { get; } = "Algorithm analysis";

        public static int MinDegreeOfParallerism { get; } = 1;

        public static int MaxDegreeOfParallerism { get; } = Environment.ProcessorCount;

        public static IReadOnlyList<PhaseOnePartOneAnalysisKind> AvailableAnalysisKindForPhaseOnePartOne { get; } =
            AnalysisHelper.GetAvailableAnalysisKindForPhaseOnePartOne();

        public static IReadOnlyList<PhaseOnePartTwoAnalysisKind> AvailableAnalysisKindForPhaseOnePartTwo { get; } =
            AnalysisHelper.GetAvailableAnalysisKindForPhaseOnePartTwo();

        public static IReadOnlyList<PhaseTwoAnalysisKind> AvailableAnalysisKindForPhaseTwo { get; } =
            AnalysisHelper.GetAvailableAnalysisKindForPhaseTwo();

        public static IReadOnlyList<GoodnessOfFitKind> AvailableGoodnessOfFitKinds { get; } =
            AnalysisHelper.GetAvailableGoodnessOfFitKinds();

        public static IReadOnlyList<AlgorithmType> AvailableAlgorithms =>
            AnalysisHelper.GetAvailableAlgorithms();
    }
}

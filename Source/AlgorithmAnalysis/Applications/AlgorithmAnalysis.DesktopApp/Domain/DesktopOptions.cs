using System;
using System.Collections.Generic;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.DomainLogic;
using AlgorithmAnalysis.Math;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DesktopApp.Domain
{
    internal static class DesktopOptions
    {
        public static string Title { get; } = "Algorithm Analysis System";

        public static int MinDegreeOfParallelism { get; } = CommonConstants.MinimumProcessorCount;

        public static int MaxDegreeOfParallelism { get; } = Environment.ProcessorCount;

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

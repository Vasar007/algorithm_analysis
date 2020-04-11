using System;
using System.Collections.Generic;
using Acolyte.Common;
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

        public static IReadOnlyList<PhaseOnePartOneAnalysisKind> AvailableAnalysisKindForPhaseOnePartOne =>
            AnalysisHelper.GetAvailableAnalysisKindForPhaseOnePartOne();

        public static IReadOnlyList<PhaseOnePartTwoAnalysisKind> AvailableAnalysisKindForPhaseOnePartTwo =>
            AnalysisHelper.GetAvailableAnalysisKindForPhaseOnePartTwo();

        public static IReadOnlyList<PhaseTwoAnalysisKind> AvailableAnalysisKindForPhaseTwo =>
            AnalysisHelper.GetAvailableAnalysisKindForPhaseTwo();

        public static IReadOnlyList<GoodnessOfFitKind> AvailableGoodnessOfFitKinds =>
            AnalysisHelper.GetAvailableGoodnessOfFitKinds();

        public static IReadOnlyList<AlgorithmType> AvailableAlgorithms =>
            AnalysisHelper.GetAvailableAlgorithms();

        public static IReadOnlyList<ExcelCellCreationMode> AvailableCellCreationModes { get; } =
            EnumHelper.GetValues<ExcelCellCreationMode>();

        public static IReadOnlyList<ExcelLibraryProvider> AvailableLibraryProviders { get; } =
            EnumHelper.GetValues<ExcelLibraryProvider>();

        public static IReadOnlyList<ExcelVersion> AvailableExcelVersions { get; } =
            EnumHelper.GetValues<ExcelVersion>();
    }
}

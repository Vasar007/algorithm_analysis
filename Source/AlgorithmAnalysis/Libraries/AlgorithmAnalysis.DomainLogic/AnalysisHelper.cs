using System;
using System.Collections.Generic;
using System.Linq;
using Acolyte.Assertions;
using Acolyte.Common;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DomainLogic.Excel;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo;
using AlgorithmAnalysis.DomainLogic.Processes;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DomainLogic
{
    public static class AnalysisHelper
    {
        public static IReadOnlyList<string> GetAvailableAnalysisKindForPhaseOnePartOne()
        {
            return GetAllEnumDescriptionValues<PhaseOnePartOneAnalysisKind>();
        }

        public static IReadOnlyList<string> GetAvailableAnalysisKindForPhaseOnePartTwo()
        {
            return GetAllEnumDescriptionValues<PhaseOnePartTwoAnalysisKind>();
        }

        public static IReadOnlyList<AlgorithmType> GetAvailableAlgorithms()
        {
            return ConfigOptions.Algorithms.GetAlgorithmTypes();
        }

        public static TEnum GetEnumValueByDescription<TEnum>(string enumDescription)
            where TEnum : struct, Enum
        {
            enumDescription.ThrowIfNull(nameof(enumDescription));

            TEnum enumResult = EnumHelper.GetValues<TEnum>()
                .Select(enumValue => (enumValue: enumValue, description: enumValue.GetDescription()))
                .First(pair => StringComparer.OrdinalIgnoreCase.Equals(pair.description, enumDescription))
                .enumValue;

            return enumResult;
        }

        internal static void RunAnalysisProgram(string analysisProgramName, string args,
            bool showWindow)
        {
            analysisProgramName.ThrowIfNullOrWhiteSpace(nameof(analysisProgramName));
            args.ThrowIfNullOrWhiteSpace(nameof(args));

            using var processHolder = ProcessHolder.Start(
                analysisProgramName, args, showWindow
            );

            processHolder.CheckExecutionStatus();
            processHolder.WaitForExit();
        }

        internal static IAnalysisPhaseOnePartOne CreateAnalysisPhaseOnePartOne(
            PhaseOnePartOneAnalysisKind phaseOnePartOne, ParametersPack args)
        {
            return phaseOnePartOne switch
            {
                PhaseOnePartOneAnalysisKind.NormalDistribution =>
                    new NormalDistributionAnalysisPhaseOnePartOne(args),

                // TODO: add formulas for solution based on beta distribution.
                PhaseOnePartOneAnalysisKind.BetaDistribution =>
                    throw new NotImplementedException(
                        "Beta distribution analysis for phase one part one is not implemented."
                    ),

                _ => throw new ArgumentOutOfRangeException(
                         nameof(phaseOnePartOne),
                         phaseOnePartOne,
                         $"Unknown analysis kind for phase 1 part 1 value: '{phaseOnePartOne.ToString()}'."
                     )
            };
        }

        internal static IAnalysisPhaseOnePartTwo CreateAnalysisPhaseOnePartTwo(
            PhaseOnePartTwoAnalysisKind phaseOnePartTwo, ParametersPack args)
        {
            return phaseOnePartTwo switch
            {
                PhaseOnePartTwoAnalysisKind.BetaDistributionWithScott =>
                    new BetaDistributionAnalysisPhaseOnePartTwo(new ScottFrequencyHistogramBuilder(), args),

                // TODO: add formulas for solution based on Sturges's formula.
                PhaseOnePartTwoAnalysisKind.BetaDistributionWithSturges =>
                    throw new NotImplementedException(
                        "Sturges's analysis for phase one part two is not implemented."
                    ),

                _ => throw new ArgumentOutOfRangeException(
                         nameof(phaseOnePartTwo),
                         phaseOnePartTwo,
                         $"Unknown analysis kind for phase 1 part 2 value: '{phaseOnePartTwo.ToString()}'."
                     )
            };
        }

        // TODO: use interface instead of this method.
        internal static string GetMinFormula(ExcelColumnIndex columnIndex, int rowIndex)
        {
            columnIndex.ThrowIfEnumValueIsUndefined(nameof(columnIndex));
            rowIndex.ThrowIfValueIsOutOfRange(nameof(rowIndex), 1, int.MaxValue);

            string cell = $"${columnIndex.ToString()}${rowIndex.ToString()}";
            return cell;
        }

        // TODO: use interface instead of this method.
        internal static string GetAverageFormula(ExcelColumnIndex columnIndex, int rowIndex)
        {
            columnIndex.ThrowIfEnumValueIsUndefined(nameof(columnIndex));
            rowIndex.ThrowIfValueIsOutOfRange(nameof(rowIndex), 1, int.MaxValue);

            string cell = $"${columnIndex.ToString()}${rowIndex.ToString()}";
            return $"{cell} * {cell} * ({cell} - 1) / 2";
        }

        // TODO: use interface instead of this method.
        internal static string GetMaxFormula(ExcelColumnIndex columnIndex, int rowIndex)
        {
            columnIndex.ThrowIfEnumValueIsUndefined(nameof(columnIndex));
            rowIndex.ThrowIfValueIsOutOfRange(nameof(rowIndex), 1, int.MaxValue);

            string cell = $"${columnIndex.ToString()}${rowIndex.ToString()}";
            return $"{cell} * {cell} * {cell} * ({cell} - 1) / 2";
        }

        private static IReadOnlyList<string> GetAllEnumDescriptionValues<TEnum>()
            where TEnum : struct, Enum
        {
            return EnumHelper.GetValues<TEnum>()
                .Select(enumValue => enumValue.GetDescription())
                .ToList();
        }
    }
}

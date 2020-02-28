using System;
using System.Collections.Generic;
using Acolyte.Assertions;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo;
using AlgorithmAnalysis.DomainLogic.Processes;
using AlgorithmAnalysis.Excel;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DomainLogic
{
    public static class AnalysisHelper
    {
        public static IReadOnlyList<PhaseOnePartOneAnalysisKind>
            GetAvailableAnalysisKindForPhaseOnePartOne()
        {
            return new List<PhaseOnePartOneAnalysisKind>
            {
                PhaseOnePartOneAnalysisKind.NormalDistribution,
                PhaseOnePartOneAnalysisKind.BetaDistribution
            };
        }

        public static IReadOnlyList<PhaseOnePartTwoAnalysisKind>
            GetAvailableAnalysisKindForPhaseOnePartTwo()
        {
            return new List<PhaseOnePartTwoAnalysisKind>
            {
                PhaseOnePartTwoAnalysisKind.BetaDistributionWithScott,
                PhaseOnePartTwoAnalysisKind.BetaDistributionWithSturges
            };
        }

        public static IReadOnlyList<AlgorithmType> GetAvailableAlgorithms()
        {
            return ConfigOptions.Analysis.GetAlgorithmTypes();
        }

        internal static IAnalysisPhaseOnePartOne CreateAnalysisPhaseOnePartOne(
            PhaseOnePartOneAnalysisKind phaseOnePartOne, ParametersPack args)
        {
            phaseOnePartOne.ThrowIfNull(nameof(phaseOnePartOne));

            if (PhaseOnePartOneAnalysisKind.NormalDistribution.Equals(phaseOnePartOne))
            {
                return new NormalDistributionAnalysisPhaseOnePartOne(args);
            }
            if (PhaseOnePartOneAnalysisKind.BetaDistribution.Equals(phaseOnePartOne))
            {
                // TODO: add formulas for solution based on beta distribution.
                throw new NotImplementedException(
                    "Beta distribution analysis for phase one part one is not implemented."
                );
            }

            throw new ArgumentOutOfRangeException(
                nameof(phaseOnePartOne), phaseOnePartOne,
                $"Unknown analysis kind for phase 1 part 1 value: {phaseOnePartOne.ToLogString()}."
            );
        }

        internal static IAnalysisPhaseOnePartTwo CreateAnalysisPhaseOnePartTwo(
            PhaseOnePartTwoAnalysisKind phaseOnePartTwo, ParametersPack args)
        {
            phaseOnePartTwo.ThrowIfNull(nameof(phaseOnePartTwo));

            if (PhaseOnePartTwoAnalysisKind.BetaDistributionWithScott.Equals(phaseOnePartTwo))
            {
                return new BetaDistributionAnalysisPhaseOnePartTwo(
                    new ScottFrequencyHistogramBuilder(args), args
                );
            }
            if (PhaseOnePartTwoAnalysisKind.BetaDistributionWithSturges.Equals(phaseOnePartTwo))
            {
                // TODO: add formulas for solution based on Sturges's formula.
                throw new NotImplementedException(
                    "Sturges's analysis for phase one part two is not implemented."
                );
            }

            throw new ArgumentOutOfRangeException(
                nameof(phaseOnePartTwo), phaseOnePartTwo,
                $"Unknown analysis kind for phase 1 part 2 value: {phaseOnePartTwo.ToLogString()}."
            );
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
    }
}

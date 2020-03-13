using System;
using System.Collections.Generic;
using Acolyte.Assertions;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseTwo;
using AlgorithmAnalysis.Math;
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

        public static IReadOnlyList<PhaseTwoAnalysisKind>
            GetAvailableAnalysisKindForPhaseTwo()
        {
            return new List<PhaseTwoAnalysisKind>
            {
                PhaseTwoAnalysisKind.NormalDistribution,
                PhaseTwoAnalysisKind.BetaDistribution
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
                    "Beta distribution analysis for phase 1 part 1 is not implemented."
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
                    "Sturges's analysis for phase 1 part 2 is not implemented."
                );
            }

            throw new ArgumentOutOfRangeException(
                nameof(phaseOnePartTwo), phaseOnePartTwo,
                $"Unknown analysis kind for phase 1 part 2 value: {phaseOnePartTwo.ToLogString()}."
            );
        }

        internal static IAnalysisPhaseTwo CreateAnalysisPhaseTwo(
            PhaseTwoAnalysisKind phaseTwo, ParametersPack args)
        {
            phaseTwo.ThrowIfNull(nameof(phaseTwo));

            if (PhaseTwoAnalysisKind.NormalDistribution.Equals(phaseTwo))
            {
                // TODO: add formulas for solution based on Sturges's formula.
                throw new NotImplementedException(
                    "Normal distribution analysis for phase 2 is not implemented."
                );
            }
            if (PhaseTwoAnalysisKind.BetaDistribution.Equals(phaseTwo))
            {
                return new BetaDistributionAnalysisPhaseTwo(args, Regression.CreateDefault());
            }

            throw new ArgumentOutOfRangeException(
                nameof(phaseTwo), phaseTwo,
                $"Unknown analysis kind for phase 2 value: {phaseTwo.ToLogString()}."
            );
        }
    }
}

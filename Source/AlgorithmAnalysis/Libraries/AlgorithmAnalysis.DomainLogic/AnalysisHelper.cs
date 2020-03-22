using System;
using System.Collections.Generic;
using System.IO;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseTwo;
using AlgorithmAnalysis.Math;
using AlgorithmAnalysis.Math.Selectors;
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

        public static IReadOnlyList<GoodnessOfFitKind>
            GetAvailableGoodnessOfFitKinds()
        {
            return new List<GoodnessOfFitKind>
            {
                GoodnessOfFitKind.CoefficientOfDetermination,
                GoodnessOfFitKind.RSquaredValue
            };
        }

        public static IReadOnlyList<AlgorithmType> GetAvailableAlgorithms()
        {
            return ConfigOptions.Analysis.GetAlgorithmTypes();
        }

        internal static string GetOrCreateDataFolder(ParametersPack args)
        {
            args.ThrowIfNull(nameof(args));

            return Utils.GetOrCreateFolderUsingFilePath(args.OutputFilenamePattern);
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
                $"Unknown analysis kind for phase 1 part 1 value: {phaseOnePartOne.ToLogString()}"
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
                $"Unknown analysis kind for phase 1 part 2 value: {phaseOnePartTwo.ToLogString()}"
            );
        }

        internal static IAnalysisPhaseTwo CreateAnalysisPhaseTwo(
            PhaseTwoAnalysisKind phaseTwo, IFunctionSelector goodnessOfFit, ParametersPack args)
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
                var regression = Regression.Create(goodnessOfFit);
                return new BetaDistributionAnalysisPhaseTwo(args, regression);
            }

            throw new ArgumentOutOfRangeException(
                nameof(phaseTwo), phaseTwo,
                $"Unknown analysis kind for phase 2 value: {phaseTwo.ToLogString()}"
            );
        }

        internal static IFunctionSelector CreateGoodnessOfFit(GoodnessOfFitKind goodnessOfFit)
        {
            goodnessOfFit.ThrowIfNull(nameof(goodnessOfFit));

            if (GoodnessOfFitKind.CoefficientOfDetermination.Equals(goodnessOfFit))
            {
                return new FunctionSelectorBasedOnCoefficientOfDetermination();
            }
            if (GoodnessOfFitKind.RSquaredValue.Equals(goodnessOfFit))
            {
                return new FunctionSelectorBasedOnRSquaredValue();
            }

            throw new ArgumentOutOfRangeException(
                nameof(goodnessOfFit), goodnessOfFit,
                $"Unknown goodness of fit kind value: {goodnessOfFit.ToLogString()}"
            );
        }
    }
}

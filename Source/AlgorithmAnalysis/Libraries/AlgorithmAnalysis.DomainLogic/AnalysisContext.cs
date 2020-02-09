using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class AnalysisContext
    {
        public ParametersPack Args { get; }

        public PhaseOnePartOneAnalysisKind PhaseOnePartOne { get; }

        public PhaseOnePartTwoAnalysisKind PhaseOnePartTwo { get; }

        public bool ShowAnalysisWindow { get; }

        public AnalysisContext(
            ParametersPack args,
            bool showAnalysisWindow,
            PhaseOnePartOneAnalysisKind phaseOnePartOne,
            PhaseOnePartTwoAnalysisKind phaseOnePartTwo)
        {
            Args = args.ThrowIfNull(nameof(args));
            PhaseOnePartOne = phaseOnePartOne.ThrowIfEnumValueIsUndefined(nameof(phaseOnePartOne));
            PhaseOnePartTwo = phaseOnePartTwo.ThrowIfEnumValueIsUndefined(nameof(phaseOnePartTwo));
            ShowAnalysisWindow = showAnalysisWindow;
        }

        internal IAnalysisPhaseOnePartOne CreateAnalysisPhaseOnePartOne()
        {
            return PhaseOnePartOne switch
            {
                PhaseOnePartOneAnalysisKind.NormalDistribution =>
                    new NormalDistributionAnalysisPhaseOnePartOne(Args),

                // TODO: add formulas for solution based on beta distribution.
                PhaseOnePartOneAnalysisKind.BetaDistribution =>
                    throw new NotImplementedException(
                        "Beta distribution analysis for phase one part one is not implemented."
                    ),
                
                _ => throw new ArgumentOutOfRangeException(
                         nameof(PhaseOnePartOne),
                         PhaseOnePartOne,
                         $"Unknown analysis kind for phase 1 part 1 value: '{PhaseOnePartOne.ToString()}'."
                     )
            };
        }

        internal IAnalysisPhaseOnePartTwo CreateAnalysisPhaseOnePartTwo()
        {
            return PhaseOnePartTwo switch
            {
                PhaseOnePartTwoAnalysisKind.BetaDistributionWithScott =>
                    new BetaDistributionAnalysisPhaseOnePartTwo(Args),

                // TODO: add formulas for solution based on Sturges's formula.
                PhaseOnePartTwoAnalysisKind.BetaDistributionWithSturges =>
                    throw new NotImplementedException(
                        "Sturges's analysis for phase one part two is not implemented."
                    ),

                _ => throw new ArgumentOutOfRangeException(
                         nameof(PhaseOnePartOne),
                         PhaseOnePartTwo,
                         $"Unknown analysis kind for phase 1 part 2 value: '{PhaseOnePartTwo.ToString()}'."
                     )
            };
        }
    }
}

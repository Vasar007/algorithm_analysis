using Acolyte.Assertions;

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
    }
}

using System.Text;
using Acolyte.Assertions;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class AnalysisContext : ILoggable
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
            PhaseOnePartOne = phaseOnePartOne.ThrowIfNull(nameof(phaseOnePartOne));
            PhaseOnePartTwo = phaseOnePartTwo.ThrowIfNull(nameof(phaseOnePartTwo));
            ShowAnalysisWindow = showAnalysisWindow;
        }

        #region ILoggable Implementation

        public string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(AnalysisContext)}]")
                .AppendLine($"Args: {Args.ToLogString()}")
                .AppendLine($"PhaseOnePartOne: {PhaseOnePartOne.ToLogString()}")
                .AppendLine($"PhaseOnePartTwo: {PhaseOnePartTwo.ToLogString()}")
                .AppendLine($"ShowAnalysisWindow: '{ShowAnalysisWindow.ToString()}'");

            return sb.ToString();
        }

        #endregion
    }
}

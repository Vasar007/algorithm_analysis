using System.IO;
using System.Text;
using Acolyte.Assertions;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class AnalysisContext : ILoggable
    {
        public ParametersPack Args { get; }

        public bool ShowAnalysisWindow { get; }

        public FileInfo OutputExcelFile { get; }

        public PhaseOnePartOneAnalysisKind PhaseOnePartOne { get; }

        public PhaseOnePartTwoAnalysisKind PhaseOnePartTwo { get; }

        public PhaseTwoAnalysisKind PhaseTwo { get; }


        public AnalysisContext(
            ParametersPack args,
            bool showAnalysisWindow,
            FileInfo outputExcelFile,
            PhaseOnePartOneAnalysisKind phaseOnePartOne,
            PhaseOnePartTwoAnalysisKind phaseOnePartTwo,
            PhaseTwoAnalysisKind phaseTwo)
        {
            Args = args.ThrowIfNull(nameof(args));
            ShowAnalysisWindow = showAnalysisWindow;
            OutputExcelFile = outputExcelFile.ThrowIfNull(nameof(outputExcelFile));
            PhaseOnePartOne = phaseOnePartOne.ThrowIfNull(nameof(phaseOnePartOne));
            PhaseOnePartTwo = phaseOnePartTwo.ThrowIfNull(nameof(phaseOnePartTwo));
            PhaseTwo = phaseTwo.ThrowIfNull(nameof(phaseTwo));
        }

        #region ILoggable Implementation

        public string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(AnalysisContext)}]")
                .AppendLine($"Args: {Args.ToLogString()}")
                .AppendLine($"ShowAnalysisWindow: '{ShowAnalysisWindow.ToString()}'")
                .AppendLine($"OutputExcelFile: '{OutputExcelFile}'")
                .AppendLine($"PhaseOnePartOne: {PhaseOnePartOne.ToLogString()}")
                .AppendLine($"PhaseOnePartTwo: {PhaseOnePartTwo.ToLogString()}")
                .AppendLine($"PhaseOnePartTwo: {PhaseTwo.ToLogString()}");

            return sb.ToString();
        }

        #endregion
    }
}

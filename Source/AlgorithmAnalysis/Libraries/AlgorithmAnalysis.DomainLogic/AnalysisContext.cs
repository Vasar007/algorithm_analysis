using System.IO;
using System.Text;
using Acolyte.Assertions;
using AlgorithmAnalysis.Math;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class AnalysisContext : ILoggable
    {
        public ParametersPack Args { get; }

        public AnalysisLaunchContext LaunchContext { get; }

        public FileInfo OutputExcelFile { get; }

        public PhaseOnePartOneAnalysisKind PhaseOnePartOne { get; }

        public PhaseOnePartTwoAnalysisKind PhaseOnePartTwo { get; }

        public PhaseTwoAnalysisKind PhaseTwo { get; }

        public GoodnessOfFitKind GoodnessOfFit { get; }


        public AnalysisContext(
            ParametersPack args,
            AnalysisLaunchContext launchContext,
            FileInfo outputExcelFile,
            PhaseOnePartOneAnalysisKind phaseOnePartOne,
            PhaseOnePartTwoAnalysisKind phaseOnePartTwo,
            PhaseTwoAnalysisKind phaseTwo,
            GoodnessOfFitKind goodnessOfFit)
        {
            Args = args.ThrowIfNull(nameof(args));
            LaunchContext = launchContext.ThrowIfNull(nameof(launchContext));
            OutputExcelFile = outputExcelFile.ThrowIfNull(nameof(outputExcelFile));
            PhaseOnePartOne = phaseOnePartOne.ThrowIfNull(nameof(phaseOnePartOne));
            PhaseOnePartTwo = phaseOnePartTwo.ThrowIfNull(nameof(phaseOnePartTwo));
            PhaseTwo = phaseTwo.ThrowIfNull(nameof(phaseTwo));
            GoodnessOfFit = goodnessOfFit.ThrowIfNull(nameof(goodnessOfFit));
        }

        public AnalysisContext CreateWith(ParametersPack args)
        {
            return new AnalysisContext(
                args: args,
                launchContext: LaunchContext,
                outputExcelFile: OutputExcelFile,
                phaseOnePartOne: PhaseOnePartOne,
                phaseOnePartTwo: PhaseOnePartTwo,
                phaseTwo: PhaseTwo,
                goodnessOfFit: GoodnessOfFit
            );
        }

        #region ILoggable Implementation

        public string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(AnalysisContext)}]")
                .AppendLine($"Args: {Args.ToLogString()}")
                .AppendLine($"LaunchContext: {LaunchContext.ToLogString()}")
                .AppendLine($"OutputExcelFile: '{OutputExcelFile}'")
                .AppendLine($"PhaseOnePartOne: {PhaseOnePartOne.ToLogString()}")
                .AppendLine($"PhaseOnePartTwo: {PhaseOnePartTwo.ToLogString()}")
                .AppendLine($"PhaseOnePartTwo: {PhaseTwo.ToLogString()}")
                .AppendLine($"GoodnessOfFit: {GoodnessOfFit.ToLogString()}");

            return sb.ToString();
        }

        #endregion
    }
}

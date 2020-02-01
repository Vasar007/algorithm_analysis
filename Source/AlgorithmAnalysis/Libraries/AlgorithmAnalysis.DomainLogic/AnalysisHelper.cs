using System.Collections.Generic;
using System.Linq;
using Acolyte.Assertions;
using Acolyte.Common;
using AlgorithmAnalysis.DomainLogic.Processes;

namespace AlgorithmAnalysis.DomainLogic
{
    public static class AnalysisHelper
    {
        public static IReadOnlyList<string> GetAvailableAnalysisKindForPhaseOne()
        {
            return EnumHelper.GetValues<PhaseOnePartOneAnalysisKind>()
                .Select(enumValue => enumValue.GetDescription())
                .ToList();
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
    }
}

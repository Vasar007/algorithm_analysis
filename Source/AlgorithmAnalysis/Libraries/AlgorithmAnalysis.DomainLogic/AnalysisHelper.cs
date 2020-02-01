using System;
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
            return GetAllEnumDescriptionValues<PhaseOnePartOneAnalysisKind>();
        }

        public static IReadOnlyList<string> GetAvailableAlgorithms()
        {
            return GetAllEnumDescriptionValues<AlgorithmType>();
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

        private static IReadOnlyList<string> GetAllEnumDescriptionValues<TEnum>()
            where TEnum : struct, Enum
        {
            return EnumHelper.GetValues<TEnum>()
                .Select(enumValue => enumValue.GetDescription())
                .ToList();
        }
    }
}

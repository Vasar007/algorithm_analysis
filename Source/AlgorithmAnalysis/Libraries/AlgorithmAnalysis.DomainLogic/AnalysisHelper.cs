using System.Collections.Generic;
using System.Linq;
using Acolyte.Common;

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
    }
}

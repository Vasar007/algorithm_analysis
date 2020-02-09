using System.ComponentModel;

namespace AlgorithmAnalysis.DomainLogic
{
    public enum PhaseOnePartTwoAnalysisKind
    {
        [Description("Beta distribution with Scott's segments formula")]
        BetaDistributionWithScott = 1,

        [Description("Beta distribution with Sturges's segments formula")]
        BetaDistributionWithSturges = 2
    }
}

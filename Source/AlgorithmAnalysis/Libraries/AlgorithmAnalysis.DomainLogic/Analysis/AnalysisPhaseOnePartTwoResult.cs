using Acolyte.Assertions;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    internal sealed class AnalysisPhaseOnePartTwoResult
    {
        public AnalysisPhaseOnePartOneResult PartOneResult { get; }

        public bool IsH0HypothesisProved { get; }


        public AnalysisPhaseOnePartTwoResult(
            AnalysisPhaseOnePartOneResult partOneResult,
            bool isH0HypothesisProved)
        {
            PartOneResult = partOneResult.ThrowIfNull(nameof(partOneResult));
            IsH0HypothesisProved = isH0HypothesisProved;
        }
    }
}

using Acolyte.Assertions;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    internal sealed class AnalysisPhaseOneResult
    {
        public int CalculatedSampleSize { get; }

        public int TotalIterationNumber { get; }


        public AnalysisPhaseOneResult(
            int calculatedSampleSize,
            int totalIterationNumber)
        {
            CalculatedSampleSize = calculatedSampleSize.ThrowIfValueIsOutOfRange(nameof(calculatedSampleSize), 1, int.MaxValue);
            TotalIterationNumber = totalIterationNumber.ThrowIfValueIsOutOfRange(nameof(calculatedSampleSize), 1, int.MaxValue);
        }
    }
}

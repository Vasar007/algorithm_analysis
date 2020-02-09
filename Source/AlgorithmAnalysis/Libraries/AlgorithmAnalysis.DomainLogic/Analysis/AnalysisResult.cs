using Acolyte.Assertions;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    public sealed class AnalysisResult
    {
        public bool Success { get; }

        public string Message { get; }


        private AnalysisResult(
            bool success,
            string message)
        {
            Success = success;
            Message = message.ThrowIfNull(nameof(message));
        }

        internal static AnalysisResult CreateSuccess(string message)
        {
            return new AnalysisResult(success: true, message);
        }

        internal static AnalysisResult CreateFailure(string message)
        {
            return new AnalysisResult(success: false, message);
        }
    }
}

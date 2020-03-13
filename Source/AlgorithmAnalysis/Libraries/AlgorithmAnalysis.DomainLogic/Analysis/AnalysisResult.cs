using System.Text;
using Acolyte.Assertions;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    public sealed class AnalysisResult : ILoggable
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

        #region ILoggable Implementation

        public string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(AnalysisResult)}]")
                .AppendLine($"Success: '{Success.ToString()}'")
                .AppendLine($"Message: '{Message}'");

            return sb.ToString();
        }

        #endregion
    }
}

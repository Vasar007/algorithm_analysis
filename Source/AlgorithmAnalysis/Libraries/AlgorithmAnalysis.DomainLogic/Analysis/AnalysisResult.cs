using System.Text;
using Acolyte.Assertions;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    public sealed class AnalysisResult : ILoggable
    {
        public bool Success { get; }

        public string Message { get; }

        public AnalysisContext Context { get; }


        private AnalysisResult(
            bool success,
            string message,
            AnalysisContext context)
        {
            Success = success;
            Message = message.ThrowIfNull(nameof(message));
            Context = context.ThrowIfNull(nameof(context));
        }

        internal static AnalysisResult CreateSuccess(
            string message,
            AnalysisContext context)
        {
            return new AnalysisResult(
                success: true,
                message: message,
                context: context
            );
        }

        internal static AnalysisResult CreateFailure(
            string message,
            AnalysisContext context)
        {
            return new AnalysisResult(
                success: false,
                message: message,
                context: context
            );
        }

        internal static AnalysisResult CreateDefault(
            AnalysisContext context)
        {
            return new AnalysisResult(
                success: false,
                message: string.Empty,
                context: context
            );
        }

        #region ILoggable Implementation

        public string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(AnalysisResult)}]")
                .AppendLine($"Success: '{Success.ToString()}'")
                .AppendLine($"Message: '{Message}'")
                .AppendLine($"Context: {Context.ToLogString()}");

            return sb.ToString();
        }

        #endregion
    }
}

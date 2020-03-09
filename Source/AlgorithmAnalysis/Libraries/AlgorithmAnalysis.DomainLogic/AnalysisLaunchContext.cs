using System;
using System.Text;
using Acolyte.Assertions;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class AnalysisLaunchContext : ILoggable
    {
        public bool ShowAnalysisWindow { get; }

        public int MaxDegreeOfParallelism { get; }


        public AnalysisLaunchContext(
            bool showAnalysisWindow,
            int maxDegreeOfParallelism)
        {
            ShowAnalysisWindow = showAnalysisWindow;
            MaxDegreeOfParallelism = maxDegreeOfParallelism.ThrowIfValueIsOutOfRange(nameof(maxDegreeOfParallelism), 1, Environment.ProcessorCount);
        }

        #region ILoggable Implementation

        public string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(AnalysisLaunchContext)}]")
                .AppendLine($"ShowAnalysisWindow: '{ShowAnalysisWindow.ToString()}'")
                .AppendLine($"MaxDegreeOfParallelism: '{MaxDegreeOfParallelism.ToString()}'");

            return sb.ToString();
        }

        #endregion
    }
}

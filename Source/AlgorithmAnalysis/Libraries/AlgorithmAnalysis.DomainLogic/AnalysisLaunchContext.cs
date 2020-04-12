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

        public bool ShowResults { get; }


        public AnalysisLaunchContext(
            bool showAnalysisWindow,
            int maxDegreeOfParallelism,
            bool showResults)
        {
            ShowAnalysisWindow = showAnalysisWindow;
            MaxDegreeOfParallelism = maxDegreeOfParallelism.ThrowIfValueIsOutOfRange(
                nameof(maxDegreeOfParallelism), 1, Environment.ProcessorCount
            );
            ShowResults = showResults;
        }

        #region ILoggable Implementation

        public string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(AnalysisLaunchContext)}]")
                .AppendLine($"ShowAnalysisWindow: '{ShowAnalysisWindow.ToString()}'")
                .AppendLine($"MaxDegreeOfParallelism: '{MaxDegreeOfParallelism.ToString()}'")
                .AppendLine($"ShowResults: '{ShowResults.ToString()}'");

            return sb.ToString();
        }

        #endregion
    }
}

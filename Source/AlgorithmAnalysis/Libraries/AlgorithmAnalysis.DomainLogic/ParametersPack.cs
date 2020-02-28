using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acolyte.Assertions;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class ParametersPack : ILoggable
    {
        public string AnalysisProgramName { get; }

        public AlgorithmType AlgorithmType { get; }

        public int StartValue { get; }

        public int EndValue { get; }

        public int LaunchesNumber { get; }

        public int Step { get; }

        public string OutputFilenamePattern { get; }

        public string CommonAnalysisFilenameSuffix { get; }


        public ParametersPack(
            string analysisProgramName,
            AlgorithmType algorithmType,
            int startValue,
            int endValue,
            int launchesNumber,
            int step,
            string outputFilenamePattern,
            string commonAnalysisFilenameSuffix)
        {
            AnalysisProgramName = analysisProgramName.ThrowIfNullOrWhiteSpace(nameof(analysisProgramName));
            AlgorithmType = algorithmType.ThrowIfNull(nameof(algorithmType));
            StartValue = startValue.ThrowIfValueIsOutOfRange(nameof(startValue), 1, int.MaxValue);
            EndValue = endValue.ThrowIfValueIsOutOfRange(nameof(endValue), StartValue, int.MaxValue);
            LaunchesNumber = launchesNumber.ThrowIfValueIsOutOfRange(nameof(launchesNumber), 1, int.MaxValue);
            Step = step.ThrowIfValueIsOutOfRange(nameof(step), 1, int.MaxValue);
            OutputFilenamePattern = outputFilenamePattern.ThrowIfNullOrWhiteSpace(nameof(outputFilenamePattern));
            CommonAnalysisFilenameSuffix = commonAnalysisFilenameSuffix.ThrowIfNullOrWhiteSpace(nameof(commonAnalysisFilenameSuffix));
        }

        public static ParametersPack Create(
            AnalysisOptions analysisOptions,
            AlgorithmType algorithmType,
            int startValue,
            int endValue,
            int launchesNumber,
            int step)
        {
            analysisOptions.ThrowIfNull(nameof(analysisOptions));

            return new ParametersPack(
                analysisProgramName: analysisOptions.AnalysisProgramName,
                algorithmType: algorithmType,
                startValue: startValue,
                endValue: endValue,
                launchesNumber: launchesNumber,
                step: step,
                outputFilenamePattern: analysisOptions.OutputFilenamePattern,
                commonAnalysisFilenameSuffix: analysisOptions.CommonAnalysisFilenameSuffix
            );
        }

        internal ParametersPack CreateWith(int newLaunchesNumber)
        {
            return new ParametersPack(
                analysisProgramName: AnalysisProgramName,
                algorithmType: AlgorithmType,
                startValue: StartValue,
                endValue: EndValue,
                launchesNumber: newLaunchesNumber,
                step: Step,
                outputFilenamePattern: OutputFilenamePattern,
                commonAnalysisFilenameSuffix: CommonAnalysisFilenameSuffix
            );
        }

        #region ILoggable Implementation

        public string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(ParametersPack)}]")
                .AppendLine($"AnalysisProgramName: '{AnalysisProgramName.ToString()}'")
                .AppendLine($"AlgorithmType: {AlgorithmType.ToLogString()}")
                .AppendLine($"StartValue: '{StartValue.ToString()}'")
                .AppendLine($"EndValue: '{EndValue.ToString()}'")
                .AppendLine($"LaunchesNumber: '{LaunchesNumber.ToString()}'")
                .AppendLine($"Step: '{Step.ToString()}'")
                .AppendLine($"OutputFilenamePattern: '{OutputFilenamePattern.ToString()}'");

            return sb.ToString();
        }

        #endregion

        internal IReadOnlyList<string> GetOutputFilenames(int phaseNumber)
        {
            phaseNumber.ThrowIfValueIsOutOfRange(nameof(phaseNumber), 1, int.MaxValue);

            int iterations = phaseNumber switch
            {
                1 => 0,

                2 => (EndValue - StartValue) / Step,

                _ => throw new ArgumentOutOfRangeException(
                         nameof(phaseNumber), phaseNumber,
                         $"Unsupported phase number: '{phaseNumber.ToString()}'."
                     )
            };

            // Contract: final output filenames should be constructed as in the line
            // with "actualQuantity".
            return Enumerable.Range(0, iterations + 1)
                .Select(i => StartValue + i * Step)
                .Select(actualQuantity => $"{OutputFilenamePattern}{actualQuantity.ToString()}.txt")
                // Analysis module produces common analysis data file.
                .Append($"{OutputFilenamePattern}{CommonAnalysisFilenameSuffix}.txt")
                .ToList();
        }

        internal string PackAsInputArgumentsForPhaseOne()
        {
            return $"{AlgorithmType.Value.ToString()} " +
                   $"{StartValue.ToString()} " +
                   $"{StartValue.ToString()} " +
                   $"{LaunchesNumber.ToString()} " +
                   $"{Step.ToString()} " +
                   $"{OutputFilenamePattern}";
        }

        internal string PackAsInputArgumentsForPhaseTwo()
        {
            return $"{AlgorithmType.Value.ToString()} " +
                   $"{StartValue.ToString()} " +
                   $"{EndValue.ToString()} " +
                   $"{LaunchesNumber.ToString()} " +
                   $"{Step.ToString()} " +
                   $"{OutputFilenamePattern}";
        }
    }
}

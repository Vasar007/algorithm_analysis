using System;
using System.Collections.Generic;
using System.Linq;
using Acolyte.Assertions;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class ParametersPack
    {
        private const string CommonAnalysisFilenameSuffix = "series";

        public string AnalysisProgramName { get; }

        public AlgorithmType AlgorithmType { get; }

        public int StartValue { get; }

        public int EndValue { get; }

        public int LaunchesNumber { get; }

        public int Step { get; }

        public string OutputFilenamePattern { get; }


        public ParametersPack(
            string analysisProgramName,
            AlgorithmType algorithmType,
            int startValue,
            int endValue,
            int launchesNumber,
            int step,
            string outputFilenamePattern)
        {
            AnalysisProgramName = analysisProgramName.ThrowIfNullOrWhiteSpace(nameof(analysisProgramName));
            AlgorithmType = algorithmType.ThrowIfNull(nameof(algorithmType));
            StartValue = startValue.ThrowIfValueIsOutOfRange(nameof(startValue), 1, int.MaxValue);
            EndValue = endValue.ThrowIfValueIsOutOfRange(nameof(endValue), StartValue, int.MaxValue);
            LaunchesNumber = launchesNumber.ThrowIfValueIsOutOfRange(nameof(launchesNumber), 1, int.MaxValue);
            Step = step.ThrowIfValueIsOutOfRange(nameof(step), 1, int.MaxValue);
            OutputFilenamePattern = outputFilenamePattern.ThrowIfNullOrWhiteSpace(nameof(outputFilenamePattern));
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
                outputFilenamePattern: OutputFilenamePattern
            );
        }

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

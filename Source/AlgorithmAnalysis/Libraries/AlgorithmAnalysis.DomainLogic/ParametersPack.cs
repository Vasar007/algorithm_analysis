using System.Collections.Generic;
using System.Linq;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class ParametersPack
    {
        private const string CommonAnalysisFilenameSuffix = "series";

        public string AnalysisProgramName { get; }

        public int AlgorythmType { get; }

        public int StartValue { get; }

        public int EndValue { get; }

        public int LaunchesNumber { get; }

        public int Step { get; }

        public string OutputFilenamePattern { get; }


        public ParametersPack(
            string analysisProgramName,
            int algorythmType,
            int startValue,
            int endValue,
            int launchesNumber,
            int step,
            string outputFilenamePattern)
        {
            AnalysisProgramName = analysisProgramName.ThrowIfNullOrWhiteSpace(nameof(analysisProgramName));

            // TODO: refactor algorythm type related logic.
            AlgorythmType = algorythmType.ThrowIfValueIsOutOfRange(nameof(algorythmType), 0, 1);

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
                algorythmType: AlgorythmType,
                startValue: StartValue,
                endValue: EndValue,
                launchesNumber: newLaunchesNumber,
                step: Step,
                outputFilenamePattern: OutputFilenamePattern
            );
        }

        internal IReadOnlyList<string> GetOutputFilenames()
        {
            int iterations = (EndValue - StartValue) / LaunchesNumber;

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
            return $"{AlgorythmType.ToString()} " +
                   $"{StartValue.ToString()} " +
                   $"{StartValue.ToString()} " +
                   $"{LaunchesNumber.ToString()} " +
                   $"{Step.ToString()} " +
                   $"{OutputFilenamePattern}";
        }

        internal string PackAsInputArgumentsForPhaseTwo()
        {
            return $"{AlgorythmType.ToString()} " +
                   $"{StartValue.ToString()} " +
                   $"{EndValue.ToString()} " +
                   $"{LaunchesNumber.ToString()} " +
                   $"{Step.ToString()} " +
                   $"{OutputFilenamePattern}";
        }
    }
}

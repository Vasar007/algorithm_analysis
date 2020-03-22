using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Acolyte.Assertions;
using Acolyte.Collections;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class ParametersPack : ILoggable
    {
        public FileInfo AnalysisProgramName { get; }

        public AlgorithmType AlgorithmType { get; }

        public int StartValue { get; }

        public int EndValue { get; }

        public int LaunchesNumber { get; }

        public int ExtrapolationSegmentValue { get; }

        public int Step { get; }

        public string OutputFilenamePattern { get; }

        public string CommonAnalysisFilenameSuffix { get; }

        public string OutputFileExtension { get; }


        private ParametersPack(
            FileInfo analysisProgramName,
            AlgorithmType algorithmType,
            int startValue,
            int endValue,
            int extrapolationSegmentValue,
            int launchesNumber,
            int step,
            string outputFilenamePattern,
            string commonAnalysisFilenameSuffix,
            string outputFileExtension)
        {
            AnalysisProgramName = analysisProgramName.ThrowIfNull(nameof(analysisProgramName));
            AlgorithmType = algorithmType.ThrowIfNull(nameof(algorithmType));
            StartValue = startValue.ThrowIfValueIsOutOfRange(nameof(startValue), 1, int.MaxValue);
            EndValue = endValue.ThrowIfValueIsOutOfRange(nameof(endValue), startValue, int.MaxValue);
            ExtrapolationSegmentValue = extrapolationSegmentValue.ThrowIfValueIsOutOfRange(nameof(extrapolationSegmentValue), endValue, int.MaxValue);
            LaunchesNumber = launchesNumber.ThrowIfValueIsOutOfRange(nameof(launchesNumber), 1, int.MaxValue);
            Step = step.ThrowIfValueIsOutOfRange(nameof(step), 1, int.MaxValue);
            OutputFilenamePattern = outputFilenamePattern.ThrowIfNullOrWhiteSpace(nameof(outputFilenamePattern));
            CommonAnalysisFilenameSuffix = commonAnalysisFilenameSuffix.ThrowIfNullOrWhiteSpace(nameof(commonAnalysisFilenameSuffix));
            OutputFileExtension = outputFileExtension.ThrowIfNullOrWhiteSpace(nameof(outputFileExtension));
        }

        public static ParametersPack Create(
            AnalysisOptions analysisOptions,
            AlgorithmType algorithmType,
            int startValue,
            int endValue,
            int extrapolationSegmentValue,
            int launchesNumber,
            int step)
        {
            analysisOptions.ThrowIfNull(nameof(analysisOptions));
            algorithmType.ThrowIfNull(nameof(algorithmType));

            return new ParametersPack(
                analysisProgramName: new FileInfo(algorithmType.AnalysisProgramName),
                algorithmType: algorithmType,
                startValue: startValue,
                endValue: endValue,
                extrapolationSegmentValue: extrapolationSegmentValue,
                launchesNumber: launchesNumber,
                step: step,
                outputFilenamePattern: Utils.ResolvePath(algorithmType.OutputFilenamePattern),
                commonAnalysisFilenameSuffix: analysisOptions.CommonAnalysisFilenameSuffix,
                outputFileExtension: analysisOptions.OutputFileExtension
            );
        }

        internal ParametersPack CreateWith(int newLaunchesNumber)
        {
            return new ParametersPack(
                analysisProgramName: AnalysisProgramName,
                algorithmType: AlgorithmType,
                startValue: StartValue,
                endValue: EndValue,
                extrapolationSegmentValue: ExtrapolationSegmentValue,
                launchesNumber: newLaunchesNumber,
                step: Step,
                outputFilenamePattern: OutputFilenamePattern,
                commonAnalysisFilenameSuffix: CommonAnalysisFilenameSuffix,
                outputFileExtension: OutputFileExtension
            );
        }

        #region ILoggable Implementation

        public string ToLogString()
        {
            var sb = new StringBuilder()
                .AppendLine($"[{nameof(ParametersPack)}]")
                .AppendLine($"AnalysisProgramName: '{AnalysisProgramName}'")
                .AppendLine($"AlgorithmType: {AlgorithmType.ToLogString()}")
                .AppendLine($"StartValue: '{StartValue.ToString()}'")
                .AppendLine($"EndValue: '{EndValue.ToString()}'")
                .AppendLine($"ExtrapolationSegmentValue: '{ExtrapolationSegmentValue.ToString()}'")
                .AppendLine($"LaunchesNumber: '{LaunchesNumber.ToString()}'")
                .AppendLine($"Step: '{Step.ToString()}'")
                .AppendLine($"OutputFilenamePattern: '{OutputFilenamePattern}'")
                .AppendLine($"CommonAnalysisFilenameSuffix: '{CommonAnalysisFilenameSuffix}'")
                .AppendLine($"OutputFileExtension: '{OutputFileExtension}'");

            return sb.ToString();
        }

        #endregion

        internal int GetIterationsNumber(int phaseNumber)
        {
            return phaseNumber switch
            {
                1 => 1,

                2 => ((EndValue - StartValue) / Step) + 1,

                _ => throw new ArgumentOutOfRangeException(
                         nameof(phaseNumber), phaseNumber,
                         $"Unsupported phase number: '{phaseNumber.ToString()}'."
                     )
            };
        }

        internal IReadOnlyList<FileInfo> GetOutputFilenames(int phaseNumber)
        {
            int iterationsNumber = GetIterationsNumber(phaseNumber);

            // Contract: final output filenames should be constructed as in the line
            // with "actualQuantity".
            return Enumerable.Range(0, iterationsNumber)
                .Select(i => StartValue + i * Step)
                .Select(actualQuantity => $"{OutputFilenamePattern}{actualQuantity.ToString()}.txt")
                // Analysis module produces common analysis data file.
                .Append($"{OutputFilenamePattern}{CommonAnalysisFilenameSuffix}.txt")
                .Select(filenames => new FileInfo(filenames))
                .ToReadOnlyList();
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

        internal IReadOnlyList<string> CollectionPackAsInputArgumentsForPhaseTwo()
        {
            string formatArgs =
                $"{AlgorithmType.Value.ToString()} " +
                "{0} " +
                "{0} " +
                $"{LaunchesNumber.ToString()} " +
                $"{Step.ToString()} " +
                $"{OutputFilenamePattern}";

            int iterationsNumber = GetIterationsNumber(phaseNumber: 2);

            return Enumerable.Range(0, iterationsNumber)
                .Select(i => StartValue + i * Step)
                .Select(actualQuantity => string.Format(formatArgs, actualQuantity))
                .ToReadOnlyList();
        }

        internal int GetNumberOfIterationByFilename(string filename)
        {
            int iterationsNumber = GetIterationsNumber(phaseNumber: 2);

            // TODO: remove this dirty hack to retrieve number of iteration.
            char[] filenameIterationNumberArray = filename
                .Reverse()
                .Skip(OutputFileExtension.Length)
                .TakeWhile(ch => char.IsDigit(ch))
                .Reverse()
                .ToArray();

            int filenameIterationNumber = int.Parse(filenameIterationNumberArray);

            return Enumerable.Range(0, iterationsNumber)
                .First(i => (StartValue + i * Step) == filenameIterationNumber);
        }
    }
}

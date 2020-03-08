using System;
using System.Collections.Generic;
using System.Linq;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common.Files;
using AlgorithmAnalysis.Common.Processes;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    internal static class AnalysisRunner
    {
        public static FileObject PerformOneIterationOfPhaseOne(ParametersPack args,
            bool showAnalysisWindow, LocalFileWorker fileWorker)
        {
            args.ThrowIfNull(nameof(args));
            fileWorker.ThrowIfNull(nameof(fileWorker));

            // Contract: output files are located in the same directory as our app.
            IReadOnlyList<string> finalOutputFilenames = args.GetOutputFilenames(phaseNumber: 1);
            CheckExpectedFilenamesNumber(expectedFilenamessNumber: 2, finalOutputFilenames);

            var fileHolder = new FileHolder(finalOutputFilenames);

            using (var analysisRunner = ProgramRunner.RunProgram(
                       args.AnalysisProgramName,
                       args.PackAsInputArgumentsForPhaseOne(),
                       showAnalysisWindow
                   ))
            {
                analysisRunner.Wait();
            }

            // The first data file is iteration result, the last is common analysis data file.
            // We don't need to read/use the last one.
            string finalOutputFilename = finalOutputFilenames.First();

            DataObject<OutputFileData> data = fileWorker.ReadDataFile(finalOutputFilename);

            return new FileObject(fileHolder, data);
        }

        public static FileObject PerformFullAnalysisForPhaseTwo(ParametersPack args,
            bool showAnalysisWindow, LocalFileWorker fileWorker)
        {
            args.ThrowIfNull(nameof(args));
            fileWorker.ThrowIfNull(nameof(fileWorker));

            // Contract: output files are located in the same directory as our app.
            IReadOnlyList<string> finalOutputFilenames = args.GetOutputFilenames(phaseNumber: 2);

            var fileHolder = new FileHolder(finalOutputFilenames);

            using (var analysisRunner = ProgramRunner.RunProgram(
                       args.AnalysisProgramName,
                       args.PackAsInputArgumentsForPhaseTwo(),
                       showAnalysisWindow
                   ))
            {
                analysisRunner.Wait();

                // TODO: process text files as soon as analysis module produces result of each
                // iteration.
                string finalOutputFilename = finalOutputFilenames.First();

                DataObject<OutputFileData> data = fileWorker.ReadDataFile(finalOutputFilename);

                return new FileObject(fileHolder, data);
            }
        }

        private static void CheckExpectedFilenamesNumber(int expectedFilenamessNumber,
            IReadOnlyList<string> actualOutputFilenames)
        {
            expectedFilenamessNumber.ThrowIfValueIsOutOfRange(nameof(expectedFilenamessNumber), 1, int.MaxValue);
            actualOutputFilenames.ThrowIfNullOrEmpty(nameof(actualOutputFilenames));

            if (actualOutputFilenames.Count != expectedFilenamessNumber)
            {
                string message =
                    "Failed to perform analysis. Should be only " +
                    $"{expectedFilenamessNumber.ToString()} output filenames but was " +
                    $"{actualOutputFilenames.Count.ToString()}.";

                throw new InvalidOperationException(message);
            }
        }
    }
}

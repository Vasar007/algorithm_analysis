using System;
using System.Collections.Generic;
using System.IO;
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
            IReadOnlyList<FileInfo> finalOutputFiles = args.GetOutputFilenames(phaseNumber: 1);
            CheckExpectedFilenamesNumber(expectedFilesNumber: 2, finalOutputFiles);

            var fileDeleter = new FileDeleter(finalOutputFiles);

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
            FileInfo finalOutputFile = finalOutputFiles.First();

            DataObject<OutputFileData> data = fileWorker.ReadDataFile(finalOutputFile);

            return new FileObject(fileDeleter, data);
        }

        public static FileObject PerformFullAnalysisForPhaseTwo(ParametersPack args,
            bool showAnalysisWindow, LocalFileWorker fileWorker)
        {
            args.ThrowIfNull(nameof(args));
            fileWorker.ThrowIfNull(nameof(fileWorker));

            // Contract: output files are located in the same directory as our app.
            IReadOnlyList<FileInfo> finalOutputFiles = args.GetOutputFilenames(phaseNumber: 2);

            var fileDeleter = new FileDeleter(finalOutputFiles);

            using (var analysisRunner = ProgramRunner.RunProgram(
                       args.AnalysisProgramName,
                       args.PackAsInputArgumentsForPhaseTwo(),
                       showAnalysisWindow
                   ))
            {
                analysisRunner.Wait();

                // TODO: process text files as soon as analysis module produces result of each
                // iteration.
                FileInfo finalOutputFile = finalOutputFiles.First();

                DataObject<OutputFileData> data = fileWorker.ReadDataFile(finalOutputFile);

                return new FileObject(fileDeleter, data);
            }
        }

        private static void CheckExpectedFilenamesNumber(int expectedFilesNumber,
            IReadOnlyList<FileInfo> actualOutputFiles)
        {
            expectedFilesNumber.ThrowIfValueIsOutOfRange(nameof(expectedFilesNumber), 1, int.MaxValue);
            actualOutputFiles.ThrowIfNullOrEmpty(nameof(actualOutputFiles));

            if (actualOutputFiles.Count != expectedFilesNumber)
            {
                string message =
                    "Failed to perform analysis. Should be only " +
                    $"{expectedFilesNumber.ToString()} output files but was " +
                    $"{actualOutputFiles.Count.ToString()}.";

                throw new InvalidOperationException(message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common.Files;
using AlgorithmAnalysis.Common.Processes;
using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal static class ExcelHelper
    {
        public const string SheetNamePrefix = "Sheet";

        public static IExcelWorkbook GetOrCreateWorkbook(string outputExcelFilename)
        {
            outputExcelFilename.ThrowIfNullOrWhiteSpace(nameof(outputExcelFilename));

            if (File.Exists(outputExcelFilename))
            {
                return ExcelWrapperFactory.CreateWorkbook(outputExcelFilename);
            }

            return ExcelWrapperFactory.CreateWorkbook();
        }

        public static string CreateSheetName(string sheetNamePrefix, string phaseNumber,
            string? iterationNumber)
        {
            sheetNamePrefix.ThrowIfNull(nameof(sheetNamePrefix));
            phaseNumber.ThrowIfNullOrEmpty(nameof(phaseNumber));

            return iterationNumber is null
                    ? $"{sheetNamePrefix}{phaseNumber.ToString()}"
                    : $"{sheetNamePrefix}{phaseNumber.ToString()}-{iterationNumber.ToString()}";
        }

        public static string CreateSheetName(string sheetNamePrefix, int phaseNumber,
            int iterationNumber)
        {
            return CreateSheetName(
                sheetNamePrefix, phaseNumber.ToString(), iterationNumber.ToString()
            );
        }

        public static string CreateSheetName(int phaseNumber)
        {
            return CreateSheetName(SheetNamePrefix, phaseNumber);
        }

        public static string CreateSheetName(string sheetNamePrefix, int phaseNumber)
        {
            return CreateSheetName(
                sheetNamePrefix, phaseNumber.ToString(), iterationNumber: null
            );
        }

        public static string CreateSheetName(int phaseNumber, int iterationNumber)
        {
            return CreateSheetName(SheetNamePrefix, phaseNumber, iterationNumber);
        }

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

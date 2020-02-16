using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Files;
using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal static class ExcelHelper
    {
        public static IExcelWorkbook GetWorkbook(string outputExcelFilename)
        {
            outputExcelFilename.ThrowIfNullOrWhiteSpace(nameof(outputExcelFilename));

            if (File.Exists(outputExcelFilename))
            {
                return ExcelWrapperFactory.CreateWorkbook(outputExcelFilename);
            }

            return ExcelWrapperFactory.CreateWorkbook();
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

            AnalysisHelper.RunAnalysisProgram(
                args.AnalysisProgramName,
                args.PackAsInputArgumentsForPhaseOne(),
                showAnalysisWindow
            );

            string finalOutputFilename = finalOutputFilenames.First();

            DataObject<OutputFileData> data = fileWorker.ReadDataFile(
                finalOutputFilename, args
            );

            return new FileObject(fileHolder, data);
        }

        private static void CheckExpectedFilenamesNumber(int expectedFilenamessNumber,
            IReadOnlyList<string> actualOutputFilenames)
        {
            expectedFilenamessNumber.ThrowIfValueIsOutOfRange(nameof(expectedFilenamessNumber), 1, int.MaxValue);
            actualOutputFilenames.ThrowIfNullOrEmpty(nameof(actualOutputFilenames));

            if (actualOutputFilenames.Count != expectedFilenamessNumber)
            {
                string message =
                    "Phase 1 of analysis failed. Should be only " +
                    $"{expectedFilenamessNumber.ToString()} output filenames but was " +
                    $"{actualOutputFilenames.Count.ToString()}.";

                throw new InvalidOperationException(message);
            }
        }
    }
}

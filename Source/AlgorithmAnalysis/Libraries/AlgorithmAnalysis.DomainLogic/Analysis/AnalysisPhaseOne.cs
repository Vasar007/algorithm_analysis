using System;
using System.Collections.Generic;
using System.Linq;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel;
using AlgorithmAnalysis.DomainLogic.Files;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    internal sealed class AnalysisPhaseOne : IAnalysis
    {
        private const int PhaseNumber = 1;

        private readonly LocalFileWorker _fileWorker;

        private readonly ExcelWrapperForPhaseOne _excelWrapper;


        public AnalysisPhaseOne(string outputExcelFilename)
        {
            _fileWorker = new LocalFileWorker();
            _excelWrapper = new ExcelWrapperForPhaseOne(outputExcelFilename);
        }

        #region Implementation of IAnalysis

        public void Analyze(AnalysisContext context)
        {
            // Find appropriate launches number iteratively (part 1 of phase 1).
            PerformPartOne(context);

            // TODO: check H0 hypothesis on calculated launches number (part 2 of phase 1).
        }

        #endregion

        private void PerformPartOne(AnalysisContext context)
        {
            int iterationNumber = 1;
            int calculatedSampleSize = context.Args.LaunchesNumber;
            int previousCalculatedSampleSize = 0;

            while (calculatedSampleSize > previousCalculatedSampleSize)
            {
                previousCalculatedSampleSize = calculatedSampleSize;

                var excelContext = new ExcelContextForPhaseOne(
                    args: context.Args.CreateWith(calculatedSampleSize),
                    showAnalysisWindow: context.ShowAnalysisWindow,
                    analysisFactory: context.CreateAnalysisPhaseOnePartOne,
                    sheetName: $"Sheet{PhaseNumber.ToString()}-{iterationNumber.ToString()}"
                );
                calculatedSampleSize = PerformOneIterationOfPartOne(excelContext);

                if (iterationNumber == 2) break;
                calculatedSampleSize = 3000;

                ++iterationNumber;
            }
        }

        private int PerformOneIterationOfPartOne(ExcelContextForPhaseOne excelContext)
        {
            // Contract: output files are located in the same directory as our app.
            IReadOnlyList<string> finalOutputFilenames = excelContext.Args.GetOutputFilenames();
            CheckExpectedFilenamesNumber(expectedFilenamessNumber: 2, finalOutputFilenames);

            using var fileHolder = new FileHolder(finalOutputFilenames);

            AnalysisHelper.RunAnalysisProgram(
                excelContext.Args.AnalysisProgramName,
                excelContext.Args.PackAsInputArgumentsForPhaseOne(),
                excelContext.ShowAnalysisWindow
            );

            string finalOutputFilename = finalOutputFilenames.First();

            using DataObject<OutputFileData> data = _fileWorker.ReadDataFile(
                finalOutputFilename, excelContext.Args
            );
            
            IEnumerable<int> operationNumbers = data.GetData(item => item.operationNumber);

            return _excelWrapper.ApplyAnalysisAndSaveData(
                operationNumbers, excelContext
            );
        }

        private static void CheckExpectedFilenamesNumber(int expectedFilenamessNumber,
            IReadOnlyList<string> actualOutputFilenames)
        {
            expectedFilenamessNumber.ThrowIfValueIsOutOfRange(nameof(expectedFilenamessNumber), 1, int.MaxValue);

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

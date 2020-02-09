using System;
using System.Collections.Generic;
using System.Linq;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne;
using AlgorithmAnalysis.DomainLogic.Files;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    internal sealed class AnalysisPhaseOne : IAnalysis
    {
        private const int PhaseNumber = 1;

        private readonly LocalFileWorker _fileWorker;

        private readonly ExcelWrapperForPhaseOnePartOne _excelWrapperPartOne;

        private readonly ExcelWrapperForPhaseOnePartTwo _excelWrapperPartTwo;


        public AnalysisPhaseOne(string outputExcelFilename)
        {
            _fileWorker = new LocalFileWorker();
            _excelWrapperPartOne = new ExcelWrapperForPhaseOnePartOne(outputExcelFilename);
            _excelWrapperPartTwo = new ExcelWrapperForPhaseOnePartTwo(outputExcelFilename);
        }

        #region IAnalysis Implementation

        public AnalysisResult Analyze(AnalysisContext context)
        {
            // Find appropriate launches number iteratively (part 1 of phase 1).
            AnalysisPhaseOneResult partOneResult = PerformPartOne(context);

            // TODO: check H0 hypothesis on calculated launches number (part 2 of phase 1).
            bool isH0HypothesisProved = PerfromPartTwo(context, partOneResult);

            return isH0HypothesisProved
                ? AnalysisResult.CreateSuccess("H0 hypothesis for the algorithm was proved.")
                : AnalysisResult.CreateFailure("H0 hypothesis for the algorithm was not proved.");
        }

        #endregion

        private AnalysisPhaseOneResult PerformPartOne(AnalysisContext context)
        {
            int iterationNumber = 1;
            int calculatedSampleSize = context.Args.LaunchesNumber;
            int previousCalculatedSampleSize = 0;

            while (calculatedSampleSize > previousCalculatedSampleSize)
            {
                previousCalculatedSampleSize = calculatedSampleSize;

                var excelContext = ExcelContextForPhaseOne.CreateForPartOne(
                    args: context.Args.CreateWith(calculatedSampleSize),
                    showAnalysisWindow: context.ShowAnalysisWindow,
                    sheetName: $"Sheet{PhaseNumber.ToString()}-{iterationNumber.ToString()}",
                    partOneFactory: context.CreateAnalysisPhaseOnePartOne
                );
                calculatedSampleSize = PerformOneIterationOfPartOne(excelContext);

                ++iterationNumber;
            }

            // TODO: set bold on text with final calculated sample size.
            return new AnalysisPhaseOneResult(calculatedSampleSize, iterationNumber);
        }

        private bool PerfromPartTwo(AnalysisContext context, AnalysisPhaseOneResult partOneResult)
        {
            // Perform the final iteration to get actual data using calculated sample size.
            var excelContext = ExcelContextForPhaseOne.CreateForPartTwo(
                args: context.Args.CreateWith(partOneResult.CalculatedSampleSize),
                showAnalysisWindow: context.ShowAnalysisWindow,
                sheetName: $"Sheet{PhaseNumber.ToString()}-{partOneResult.TotalIterationNumber.ToString()}",
                partTwoFactory: context.CreateAnalysisPhaseOnePartTwo
            );

            using FileObject fileObject = ExcelHelper.PerformOneIterationOfPhaseOne(
                 excelContext.Args, excelContext.ShowAnalysisWindow, _fileWorker
             );

            return _excelWrapperPartTwo.ApplyAnalysisAndSaveData(
                fileObject.Data.GetData(item => item.operationNumber), excelContext
            );
        }

        private int PerformOneIterationOfPartOne(ExcelContextForPhaseOne excelContext)
        {
            using FileObject fileObject = ExcelHelper.PerformOneIterationOfPhaseOne(
                excelContext.Args, excelContext.ShowAnalysisWindow, _fileWorker
            );

            return _excelWrapperPartOne.ApplyAnalysisAndSaveData(
                fileObject.Data.GetData(item => item.operationNumber), excelContext
            );
        }
    }
}

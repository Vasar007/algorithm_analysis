using System.IO;
using AlgorithmAnalysis.Common.Files;
using AlgorithmAnalysis.DomainLogic.Excel;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    internal sealed class AnalysisPhaseOne : IAnalysis
    {
        private const int PhaseNumber = 1;

        private readonly LocalFileWorker _fileWorker;

        private readonly ExcelWrapperForPhaseOnePartOne _excelWrapperPartOne;

        private readonly ExcelWrapperForPhaseOnePartTwo _excelWrapperPartTwo;


        public AnalysisPhaseOne()
        {
            _fileWorker = new LocalFileWorker();
            _excelWrapperPartOne = new ExcelWrapperForPhaseOnePartOne();
            _excelWrapperPartTwo = new ExcelWrapperForPhaseOnePartTwo();
        }

        #region IAnalysis Implementation

        public AnalysisResult Analyze(AnalysisContext context)
        {
            // Find appropriate launches number iteratively (part 1 of phase 1).
            AnalysisPhaseOneResult partOneResult = PerformPartOne(context);

            // Check H0 hypothesis on calculated launches number (part 2 of phase 1).
            bool isH0HypothesisProved = PerfromPartTwo(context, partOneResult);

            return isH0HypothesisProved
                ? AnalysisResult.CreateSuccess("H0 hypothesis for the algorithm was proved.")
                : AnalysisResult.CreateSuccess("H0 hypothesis for the algorithm was not proved."); // CreateFailure // TODO: remove this statement when finish debugging.
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

                var excelContext = ExcelContextForPhaseOne<IAnalysisPhaseOnePartOne>.CreateFor(
                    args: context.Args.CreateWith(calculatedSampleSize),
                    showAnalysisWindow: context.ShowAnalysisWindow,
                    outputExcelFile: context.OutputExcelFile,
                    sheetName: ExcelHelper.CreateSheetName(PhaseNumber, iterationNumber),
                    analysisFactory: args => AnalysisHelper.CreateAnalysisPhaseOnePartOne(context.PhaseOnePartOne, args)
                );
                calculatedSampleSize = PerformOneIterationOfPartOne(excelContext);

                ++iterationNumber;
                break; // TODO: remove this statement when finish debugging.
            }

            // TODO: set bold on text with final calculated sample size.
            return new AnalysisPhaseOneResult(calculatedSampleSize, iterationNumber);
        }

        private int PerformOneIterationOfPartOne(
            ExcelContextForPhaseOne<IAnalysisPhaseOnePartOne> excelContext)
        {
            using FileObject fileObject = AnalysisRunner.PerformOneIterationOfPhaseOne(
                excelContext.Args, excelContext.ShowAnalysisWindow, _fileWorker
            );

            return _excelWrapperPartOne.ApplyAnalysisAndSaveData(
                fileObject.Data.GetData(item => item.operationNumber), excelContext
            );
        }

        private bool PerfromPartTwo(AnalysisContext context, AnalysisPhaseOneResult partOneResult)
        {
            // Perform the final iteration to get actual data using calculated sample size.
            var excelContext = ExcelContextForPhaseOne<IAnalysisPhaseOnePartTwo>.CreateFor(
                args: context.Args.CreateWith(1000), // partOneResult.CalculatedSampleSize // TODO: remove this statement when finish debugging.
                showAnalysisWindow: context.ShowAnalysisWindow,
                outputExcelFile: context.OutputExcelFile,
                sheetName: ExcelHelper.CreateSheetName(PhaseNumber, partOneResult.TotalIterationNumber),
                analysisFactory: args => AnalysisHelper.CreateAnalysisPhaseOnePartTwo(context.PhaseOnePartTwo, args)
            );

            using FileObject fileObject = AnalysisRunner.PerformOneIterationOfPhaseOne(
                 excelContext.Args, excelContext.ShowAnalysisWindow, _fileWorker
             );

            return _excelWrapperPartTwo.ApplyAnalysisAndSaveData(
                fileObject.Data.GetData(item => item.operationNumber), excelContext
            );
        }
    }
}

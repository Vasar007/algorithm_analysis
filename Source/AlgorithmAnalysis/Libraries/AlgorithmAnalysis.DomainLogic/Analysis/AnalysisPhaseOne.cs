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
            AnalysisPhaseOnePartOneResult partOneResult = PerformPartOne(context);

            // Check H0 hypothesis on calculated launches number (part 2 of phase 1).
            AnalysisPhaseOnePartTwoResult partTwoResult = PerfromPartTwo(context, partOneResult);

            return partTwoResult.IsH0HypothesisProved
                ? AnalysisResult.CreateSuccess("H0 hypothesis for the algorithm was proved.")
                : AnalysisResult.CreateFailure("H0 hypothesis for the algorithm was not proved.");
        }

        #endregion

        private AnalysisPhaseOnePartOneResult PerformPartOne(AnalysisContext context)
        {
            int iterationNumber = 1;
            int calculatedSampleSize = context.Args.LaunchesNumber;
            int previousCalculatedSampleSize = 0;

            while (calculatedSampleSize > previousCalculatedSampleSize)
            {
                previousCalculatedSampleSize = calculatedSampleSize;

                var excelContext = ExcelContextForPhaseOne<IAnalysisPhaseOnePartOne>.CreateFor(
                    args: context.Args.CreateWith(calculatedSampleSize),
                    launchContext: context.LaunchContext,
                    outputExcelFile: context.OutputExcelFile,
                    sheetName: ExcelHelper.CreateSheetName(PhaseNumber, iterationNumber),
                    analysisFactory: args => AnalysisHelper.CreateAnalysisPhaseOnePartOne(context.PhaseOnePartOne, args)
                );
                calculatedSampleSize = PerformOneIterationOfPartOne(excelContext);

                ++iterationNumber;
            }

            // TODO: set bold on text with final calculated sample size.
            return new AnalysisPhaseOnePartOneResult(calculatedSampleSize, iterationNumber);
        }

        private int PerformOneIterationOfPartOne(
            ExcelContextForPhaseOne<IAnalysisPhaseOnePartOne> excelContext)
        {
            using FileObject fileObject = AnalysisRunner.PerformOneIterationOfPhaseOne(
                excelContext.Args, excelContext.LaunchContext, _fileWorker
            );

            return _excelWrapperPartOne.ApplyAnalysisAndSaveData(
                fileObject.Data.GetData(item => item.operationNumber), excelContext
            );
        }

        private AnalysisPhaseOnePartTwoResult PerfromPartTwo(AnalysisContext context,
            AnalysisPhaseOnePartOneResult partOneResult)
        {
            // Perform the final iteration to get actual data using calculated sample size.
            var excelContext = ExcelContextForPhaseOne<IAnalysisPhaseOnePartTwo>.CreateFor(
                args: context.Args.CreateWith(partOneResult.CalculatedSampleSize),
                launchContext: context.LaunchContext,
                outputExcelFile: context.OutputExcelFile,
                sheetName: ExcelHelper.CreateSheetName(PhaseNumber, partOneResult.TotalIterationNumber),
                analysisFactory: args => AnalysisHelper.CreateAnalysisPhaseOnePartTwo(context.PhaseOnePartTwo, args)
            );

            using FileObject fileObject = AnalysisRunner.PerformOneIterationOfPhaseOne(
                 excelContext.Args, excelContext.LaunchContext, _fileWorker
             );

            bool isH0HypothesisProved = _excelWrapperPartTwo.ApplyAnalysisAndSaveData(
                fileObject.Data.GetData(item => item.operationNumber), excelContext
            );

            return new AnalysisPhaseOnePartTwoResult(partOneResult, isH0HypothesisProved);
        }
    }
}

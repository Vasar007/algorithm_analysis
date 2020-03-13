using System.Threading.Tasks;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common.Files;
using AlgorithmAnalysis.DomainLogic.Excel;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo;
using AlgorithmAnalysis.Logging;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    internal sealed class AnalysisPhaseOne : IAnalysis
    {
        private const int PhaseNumber = 1;

        private static readonly ILogger _logger = LoggerFactory.CreateLoggerFor<AnalysisPhaseOne>();

        private readonly LocalFileWorker _fileWorker;

        private readonly ExcelWrapperForPhaseOnePartOne _excelWrapperPartOne;

        private readonly ExcelWrapperForPhaseOnePartTwo _excelWrapperPartTwo;


        public AnalysisPhaseOne(LocalFileWorker fileWorker)
        {
            _fileWorker = fileWorker.ThrowIfNull(nameof(fileWorker));

            _excelWrapperPartOne = new ExcelWrapperForPhaseOnePartOne();
            _excelWrapperPartTwo = new ExcelWrapperForPhaseOnePartTwo();
        }

        #region IAnalysis Implementation

        public async Task<AnalysisResult> AnalyzeAsync(AnalysisContext context)
        {
            _logger.Info("Starting analysis phase one.");

            // Find appropriate launches number iteratively (part 1 of phase 1).
            AnalysisPhaseOnePartOneResult partOneResult =
                await PerformPartOneAsync(context);

            // Check H0 hypothesis on calculated launches number (part 2 of phase 1).
            AnalysisPhaseOnePartTwoResult partTwoResult =
                await PerfromPartTwoAsync(context, partOneResult);

            return partTwoResult.IsH0HypothesisProved
                ? AnalysisResult.CreateSuccess("H0 hypothesis for the algorithm was proved.")
                : AnalysisResult.CreateFailure("H0 hypothesis for the algorithm was not proved.");
        }

        #endregion

        private async Task<AnalysisPhaseOnePartOneResult> PerformPartOneAsync(AnalysisContext context)
        {
            _logger.Info("Starting analysis phase one part one.");

            int iterationNumber = 1;
            int calculatedSampleSize = context.Args.LaunchesNumber;
            int previousCalculatedSampleSize = 0;

            while (calculatedSampleSize > previousCalculatedSampleSize)
            {
                _logger.Info(
                    $"Iteration: '{iterationNumber.ToString()}', " +
                    $"calculated sample size: '{calculatedSampleSize.ToString()}'."
                );

                previousCalculatedSampleSize = calculatedSampleSize;

                var excelContext = ExcelContextForPhaseOne<IAnalysisPhaseOnePartOne>.CreateFor(
                    args: context.Args.CreateWith(calculatedSampleSize),
                    launchContext: context.LaunchContext,
                    outputExcelFile: context.OutputExcelFile,
                    sheetName: ExcelHelper.CreateSheetName(PhaseNumber, iterationNumber),
                    analysisFactory: args => AnalysisHelper.CreateAnalysisPhaseOnePartOne(context.PhaseOnePartOne, args)
                );
                _logger.Info($"Iteration parameters pack: {excelContext.Args.ToLogString()}");

                calculatedSampleSize = await PerformOneIterationOfPartOneAsync(excelContext);

                ++iterationNumber;
            }

            // TODO: set bold on text with final calculated sample size.

            _logger.Info("Finished analysis phase one part one.");
            _logger.Info(
                    $"Total iteration number: '{iterationNumber.ToString()}', " +
                    $"final calculated sample size: '{calculatedSampleSize.ToString()}'."
                );
            return new AnalysisPhaseOnePartOneResult(calculatedSampleSize, iterationNumber);
        }

        private async Task<int> PerformOneIterationOfPartOneAsync(
            ExcelContextForPhaseOne<IAnalysisPhaseOnePartOne> excelContext)
        {
            using FileObject fileObject = await AnalysisRunner.PerformOneIterationOfPhaseOneAsync(
                excelContext.Args, excelContext.LaunchContext, _fileWorker
            );

            return _excelWrapperPartOne.ApplyAnalysisAndSaveData(
                fileObject.Data.GetData(item => item.operationNumber), excelContext
            );
        }

        private async Task<AnalysisPhaseOnePartTwoResult> PerfromPartTwoAsync(AnalysisContext context,
            AnalysisPhaseOnePartOneResult partOneResult)
        {
            _logger.Info("Starting analysis phase one part two.");

            // Perform the final iteration to get actual data using calculated sample size.
            var excelContext = ExcelContextForPhaseOne<IAnalysisPhaseOnePartTwo>.CreateFor(
                args: context.Args.CreateWith(partOneResult.CalculatedSampleSize),
                launchContext: context.LaunchContext,
                outputExcelFile: context.OutputExcelFile,
                sheetName: ExcelHelper.CreateSheetName(PhaseNumber, partOneResult.TotalIterationNumber),
                analysisFactory: args => AnalysisHelper.CreateAnalysisPhaseOnePartTwo(context.PhaseOnePartTwo, args)
            );
            _logger.Info($"Final parameters pack: {excelContext.Args.ToLogString()}");

            using FileObject fileObject = await AnalysisRunner.PerformOneIterationOfPhaseOneAsync(
                 excelContext.Args, excelContext.LaunchContext, _fileWorker
             );

            bool isH0HypothesisProved = _excelWrapperPartTwo.ApplyAnalysisAndSaveData(
                fileObject.Data.GetData(item => item.operationNumber), excelContext
            );

            _logger.Info("Finished analysis phase one part two.");
            return new AnalysisPhaseOnePartTwoResult(partOneResult, isH0HypothesisProved);
        }
    }
}

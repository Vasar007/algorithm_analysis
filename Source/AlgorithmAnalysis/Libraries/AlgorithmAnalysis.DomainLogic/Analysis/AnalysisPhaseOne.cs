using System;
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
            _logger.Info($"Context: {context.ToLogString()}");

            // Find appropriate launches number iteratively (part 1 of phase 1).
            AnalysisPhaseOnePartOneResult partOneResult =
                await PerformPartOneAsync(context);

            AnalysisContext updatedContext = context.CreateWith(
                args: context.Args.CreateWith(partOneResult.CalculatedSampleSize)
            );

            // Check H0 hypothesis on calculated launches number (part 2 of phase 1).
            AnalysisPhaseOnePartTwoResult partTwoResult =
                await PerfromPartTwoAsync(updatedContext, partOneResult);

            if (partTwoResult.IsH0HypothesisProved)
            {
                return AnalysisResult.CreateSuccess(
                    "H0 hypothesis for the algorithm was proved.", updatedContext
                );
            }
            
            return AnalysisResult.CreateFailure(
                "H0 hypothesis for the algorithm was not proved.", updatedContext
            );
        }

        #endregion

        private static ExcelContextForPhaseOne<IAnalysisPhaseOnePartOne> CreateExcelContextPartOne(
            AnalysisContext context, int iterationNumber, int calculatedSampleSize)
        {
            Func<ParametersPack, IAnalysisPhaseOnePartOne> analysisFactory =
                args => AnalysisHelper.CreateAnalysisPhaseOnePartOne(
                            context.PhaseOnePartOne, args
                        );

            return ExcelContextForPhaseOne<IAnalysisPhaseOnePartOne>.CreateFor(
                args: context.Args.CreateWith(calculatedSampleSize),
                launchContext: context.LaunchContext,
                outputExcelFile: context.OutputExcelFile,
                sheetName: ExcelHelper.CreateSheetName(PhaseNumber, iterationNumber),
                analysisFactory: analysisFactory
            );
        }

        private async Task<AnalysisPhaseOnePartOneResult> PerformPartOneAsync(
            AnalysisContext context)
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

                var excelContext = CreateExcelContextPartOne(
                    context, iterationNumber, calculatedSampleSize
                );
                _logger.Info($"Iteration parameters pack: {excelContext.Args.ToLogString()}");

                calculatedSampleSize = await PerformOneIterationOfPartOneAsync(excelContext);

                ++iterationNumber;
            }

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

        private static ExcelContextForPhaseOne<IAnalysisPhaseOnePartTwo> CreateExcelContextPartTwo(
            AnalysisContext context, AnalysisPhaseOnePartOneResult partOneResult)
        {
            string sheetName =
                ExcelHelper.CreateSheetName(PhaseNumber, partOneResult.TotalIterationNumber);

            Func<ParametersPack, IAnalysisPhaseOnePartTwo> analysisFactory =
                args => AnalysisHelper.CreateAnalysisPhaseOnePartTwo(context.PhaseOnePartTwo, args);

            return ExcelContextForPhaseOne<IAnalysisPhaseOnePartTwo>.CreateFor(
                args: context.Args,
                launchContext: context.LaunchContext,
                outputExcelFile: context.OutputExcelFile,
                sheetName: sheetName,
                analysisFactory: analysisFactory
            );
        }

        private async Task<AnalysisPhaseOnePartTwoResult> PerfromPartTwoAsync(
            AnalysisContext context, AnalysisPhaseOnePartOneResult partOneResult)
        {
            _logger.Info("Starting analysis phase one part two.");

            var excelContext = CreateExcelContextPartTwo(context, partOneResult);

            _logger.Info($"Final parameters pack: {excelContext.Args.ToLogString()}");

            // Perform the final iteration to get actual data using calculated sample size.
            using FileObject fileObject = await AnalysisRunner.PerformOneIterationOfPhaseOneAsync(
                 excelContext.Args, excelContext.LaunchContext, _fileWorker
             );

            bool isH0HypothesisProved = _excelWrapperPartTwo.ApplyAnalysisAndSaveData(
                fileObject.Data.GetData(item => item.operationNumber), excelContext
            );

            _logger.Info("Finished analysis phase one part two.");
            _logger.Info(
                $"H0 hypothesis for the algorithm was proved: '{isH0HypothesisProved.ToString()}'."
            );
            return new AnalysisPhaseOnePartTwoResult(partOneResult, isH0HypothesisProved);
        }
    }
}

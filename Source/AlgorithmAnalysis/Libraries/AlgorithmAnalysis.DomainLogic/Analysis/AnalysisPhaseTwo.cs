using System;
using System.Threading.Tasks;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common.Files;
using AlgorithmAnalysis.Common.Threading;
using AlgorithmAnalysis.DomainLogic.Excel;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseTwo;
using AlgorithmAnalysis.Logging;
using AlgorithmAnalysis.Math.Selectors;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    internal sealed class AnalysisPhaseTwo : IAnalysis
    {
        private const int PhaseNumber = 2;

        private static readonly ILogger _logger = LoggerFactory.CreateLoggerFor<AnalysisPhaseTwo>();

        private readonly LocalFileWorker _fileWorker;

        private readonly ExcelWrapperForPhaseTwo _excelWrapperForPhaseTwo;

        private readonly AsyncLock _asyncLock;


        public AnalysisPhaseTwo(LocalFileWorker fileWorker)
        {
            _fileWorker = fileWorker.ThrowIfNull(nameof(fileWorker));

            _excelWrapperForPhaseTwo = new ExcelWrapperForPhaseTwo();
            _asyncLock = new AsyncLock();
        }

        #region IAnalysis Implementation

        public async Task<AnalysisResult> AnalyzeAsync(AnalysisContext context)
        {
            _logger.Info("Starting analysis phase two.");
            _logger.Info($"Context: {context.ToLogString()}");

            // Perform full analysis and calculate confidence complexity function.
            AnalysisPhaseTwoResult _ = await PerformPhaseTwoAsync(context);

            _logger.Info("Finished analysis phase two.");
            return AnalysisResult.CreateSuccess("Analysis finished successfully.", context);
        }

        #endregion

        private static ExcelContextForPhaseTwo<IAnalysisPhaseTwo> CreateExcelContext(
            AnalysisContext context)
        {
            IFunctionSelector goonessOfFit = AnalysisHelper.CreateGoodnessOfFit(
                context.GoodnessOfFit
            );

            Func<ParametersPack, IAnalysisPhaseTwo> analysisFactory =
                args => AnalysisHelper.CreateAnalysisPhaseTwo(context.PhaseTwo, goonessOfFit, args);

            return ExcelContextForPhaseTwo<IAnalysisPhaseTwo>.CreateFor(
                analysisContext: context,
                sheetName: ExcelHelper.CreateSheetName(PhaseNumber),
                analysisFactory: analysisFactory
            );
        }

        private async Task<AnalysisPhaseTwoResult> PerformPhaseTwoAsync(AnalysisContext context)
        {
            var excelContext = CreateExcelContext(context);

            await AnalysisRunner.PerformFullAnalysisForPhaseTwoAsync(
                args: context.Args,
                launchContext: context.LaunchContext,
                fileWorker: _fileWorker,
                asyncCallback: fileObject => PerformOneIterationASync(excelContext, fileObject)
            );

            _excelWrapperForPhaseTwo.ApplyAnalysisAndSaveData(excelContext);

            return new AnalysisPhaseTwoResult();
        }

        private async Task PerformOneIterationASync(
            ExcelContextForPhaseTwo<IAnalysisPhaseTwo> excelContext, FileObject fileObject)
        {
            _logger.Info("Awaiting an opportunity to start processing of algorithm results.");

            // Lock data processing to avoid corrupted results in the final Excel file.
            using (await _asyncLock.EnterAsync())
            {
                _logger.Info(
                    "Processing began for algorithm results for one iteration of phase two."
                );

                _excelWrapperForPhaseTwo.ApplyAnalysisAndSaveDataOneIteration(
                    data: fileObject.Data.GetData(item => item.operationNumber),
                    excelContext: excelContext,
                    dataFilename: fileObject.Data.Name
                );

                _logger.Info(
                    "Processing finished for algorithm results for one iteration of phase two."
                );
            }
        }
    }
}

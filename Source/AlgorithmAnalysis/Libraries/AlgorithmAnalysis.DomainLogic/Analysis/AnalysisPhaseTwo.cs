using System.Threading.Tasks;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common.Files;
using AlgorithmAnalysis.DomainLogic.Excel;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseTwo;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    internal sealed class AnalysisPhaseTwo : IAnalysis
    {
        private const int PhaseNumber = 2;

        private readonly LocalFileWorker _fileWorker;

        private readonly ExcelWrapperForPhaseTwo _excelWrapperForPhaseTwo;


        public AnalysisPhaseTwo(LocalFileWorker fileWorker)
        {
            _fileWorker = fileWorker.ThrowIfNull(nameof(fileWorker));

            _excelWrapperForPhaseTwo = new ExcelWrapperForPhaseTwo();
        }

        #region IAnalysis Implementation

        public async Task<AnalysisResult> AnalyzeAsync(AnalysisContext context)
        {
            AnalysisPhaseTwoResult _ = await PerformPartTwoAsync(context);

            // TODO: process data and perform statisitical analysis.
            return AnalysisResult.CreateFailure("Phase 2 is not fully implemented.");
        }

        #endregion

        private async Task<AnalysisPhaseTwoResult> PerformPartTwoAsync(AnalysisContext context)
        {
            var excelContext = ExcelContextForPhaseTwo<IAnalysisPhaseTwo>.CreateFor(
                analysisContext: context,
                sheetName: ExcelHelper.CreateSheetName(PhaseNumber),
                analysisFactory: args => AnalysisHelper.CreateAnalysisPhaseTwo(context.PhaseTwo, args)

            );

            await AnalysisRunner.PerformFullAnalysisForPhaseTwoAsync(
                context.Args,
                context.LaunchContext,
                _fileWorker,
                fileObject => PerformOneIteration(excelContext, fileObject)
            );

            _excelWrapperForPhaseTwo.ApplyAnalysisAndSaveData(excelContext);

            return new AnalysisPhaseTwoResult();
        }

        private void PerformOneIteration(ExcelContextForPhaseTwo<IAnalysisPhaseTwo> excelContext,
            FileObject fileObject)
        {
            _excelWrapperForPhaseTwo.ApplyAnalysisAndSaveDataOneIteration(
                fileObject.Data.GetData(item => item.operationNumber),
                excelContext,
                fileObject.Data.Name
            );
        }
    }
}

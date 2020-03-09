using AlgorithmAnalysis.DomainLogic.Excel;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseTwo;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    internal sealed class AnalysisPhaseTwo : IAnalysis
    {
        private const int PhaseNumber = 2;

        private readonly ExcelWrapperForPhaseTwo _excelWrapperForPhaseTwo;


        public AnalysisPhaseTwo()
        {
            _excelWrapperForPhaseTwo = new ExcelWrapperForPhaseTwo();
        }

        #region IAnalysis Implementation

        public AnalysisResult Analyze(AnalysisContext context)
        {
            PerformPartTwo(context);

            // TODO: launch analysis several times in segment [StartValue, EndValue] with step=Step.
            // TODO: find output files with data and parse them.
            // TODO: save output data to the Excel tables and apply formulas.
            // TODO: delete output files with data.
            return AnalysisResult.CreateFailure("Phase 2 is not fully implemented.");
        }

        #endregion

        private AnalysisPhaseTwoResult PerformPartTwo(AnalysisContext context)
        {
            var excelContext = ExcelContextForPhaseTwo<IAnalysisPhaseTwo>.CreateFor(
                analysisContext: context,
                sheetName: ExcelHelper.CreateSheetName(PhaseNumber),
                analysisFactory: args => AnalysisHelper.CreateAnalysisPhaseTwo(context.PhaseTwo, args)

            );
            _excelWrapperForPhaseTwo.ApplyAnalysisAndSaveData(excelContext);

            return new AnalysisPhaseTwoResult();
        }
    }
}

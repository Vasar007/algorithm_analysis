using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseTwo
{
    internal interface IAnalysisPhaseTwo
    {
        void ApplyAnalysisToSingleLaunch(IExcelSheet sheet, ExcelColumnIndex currentColumn,
            int currentRow, int operationNumber);

        void ApplyAnalysisToDataset(IExcelSheet sheet, int currentColumnIndex);
    }
}

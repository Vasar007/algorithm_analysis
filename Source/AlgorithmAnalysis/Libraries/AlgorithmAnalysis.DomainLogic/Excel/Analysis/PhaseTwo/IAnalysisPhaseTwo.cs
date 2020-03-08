using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseTwo
{
    internal interface IAnalysisPhaseTwo
    {
        void ApplyAnalysisToDataset(IExcelSheet sheet, int currentColumnIndex);
    }
}

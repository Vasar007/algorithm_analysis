using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo
{
    internal interface IAnalysisPhaseOnePartTwo
    {
        // Contract: operationNumber and currentRow are greater than or equal to zero.
        void ApplyAnalysisToSingleLaunch(IExcelSheet sheet, int operationNumber, int currentRow);
        void ApplyAnalysisToDataset(IExcelSheet sheet);
        bool CheckH0Hypothesis(IExcelSheet sheet);
    }
}

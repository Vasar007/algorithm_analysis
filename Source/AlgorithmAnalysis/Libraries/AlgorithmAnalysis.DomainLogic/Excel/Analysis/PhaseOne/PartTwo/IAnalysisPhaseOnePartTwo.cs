using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo
{
    internal interface IAnalysisPhaseOnePartTwo : IAnalysisPhaseOne
    {
        // Contract: operationNumber and currentRow are greater than or equal to zero.
        void ApplyAnalysisToSingleLaunch(IExcelSheet sheet, int currentRow, int operationNumber);

        void ApplyAnalysisToDataset(IExcelSheet sheet);

        bool CheckH0Hypothesis(IExcelSheet sheet);
    }
}

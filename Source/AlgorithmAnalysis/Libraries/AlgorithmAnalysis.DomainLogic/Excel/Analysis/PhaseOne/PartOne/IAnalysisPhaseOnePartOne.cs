using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne
{
    internal interface IAnalysisPhaseOnePartOne : IAnalysisPhaseOne
    {
        // Contract: operationNumber and currentRow are greater than or equal to zero.
        void ApplyAnalysisToSingleLaunch(IExcelSheet sheet, int operationNumber, int currentRow);
        void ApplyAnalysisToDataset(IExcelSheet sheet);
        int GetCalculatedSampleSize(IExcelSheet sheet);
    }
}

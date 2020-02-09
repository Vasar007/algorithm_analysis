namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne
{
    internal interface IAnalysisPhaseOnePartOne
    {
        // Contract: operationNumber and currentRow are greater than or equal to zero.
        void ApplyAnalysisToSingleLaunch(ExcelSheet sheet, int operationNumber, int currentRow);
        void ApplyAnalysisToDataset(ExcelSheet sheet);
        int GetCalculatedSampleSize(ExcelSheet sheet);
    }
}

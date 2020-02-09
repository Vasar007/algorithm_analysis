namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo
{
    internal interface IAnalysisPhaseOnePartTwo
    {
        // Contract: operationNumber and currentRow are greater than or equal to zero.
        void ApplyAnalysisToSingleLaunch(ExcelSheet sheet, int operationNumber, int currentRow);
        void ApplyAnalysisToDataset(ExcelSheet sheet);
        bool CheckH0Hypothesis(ExcelSheet sheet);
    }
}

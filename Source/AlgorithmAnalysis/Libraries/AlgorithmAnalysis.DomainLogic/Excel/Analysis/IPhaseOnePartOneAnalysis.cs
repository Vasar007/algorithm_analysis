namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis
{
    internal interface IPhaseOnePartOneAnalysis
    {
        void ApplyAnalysisToSingleLaunch(int operationNumber, int currentRow);
        void ApplyAnalysisToDataset();
    }
}

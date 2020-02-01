namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis
{
    internal interface IAnalysisPhaseOnePartOne
    {
        void ApplyAnalysisToSingleLaunch(int operationNumber, int currentRow);
        void ApplyAnalysisToDataset();
        int GetCalculatedSampleSize();
    }
}

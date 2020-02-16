using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo
{
    internal sealed class DummyAnalysisPhaseOnePartTwo : IAnalysisPhaseOnePartTwo
    {
        public DummyAnalysisPhaseOnePartTwo()
        {
        }

        public static DummyAnalysisPhaseOnePartTwo Create()
        {
            return new DummyAnalysisPhaseOnePartTwo();
        }

        #region IAnalysisPhaseOnePartTwo Implementation

        public void ApplyAnalysisToSingleLaunch(IExcelSheet sheet, int operationNumber,
            int currentRow)
        {
            // Do nothing.
        }

        public void ApplyAnalysisToDataset(IExcelSheet sheet)
        {
            // Do nothing.
        }

        public bool CheckH0Hypothesis(IExcelSheet sheet)
        {
            // Do nothing.
            return default;
        }

        #endregion
    }
}

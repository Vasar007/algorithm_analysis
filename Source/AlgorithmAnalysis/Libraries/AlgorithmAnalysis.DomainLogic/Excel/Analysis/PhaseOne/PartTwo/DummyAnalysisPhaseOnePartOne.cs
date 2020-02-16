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

        public void ApplyAnalysisToSingleLaunch(ExcelSheet sheet, int operationNumber,
            int currentRow)
        {
            // Do nothing.
        }

        public void ApplyAnalysisToDataset(ExcelSheet sheet)
        {
            // Do nothing.
        }

        public bool CheckH0Hypothesis(ExcelSheet sheet)
        {
            // Do nothing.
            return default;
        }

        #endregion
    }
}

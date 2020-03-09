using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne
{
    internal sealed class DummyAnalysisPhaseOnePartOne : IAnalysisPhaseOnePartOne
    {
        public DummyAnalysisPhaseOnePartOne()
        {
        }

        public static DummyAnalysisPhaseOnePartOne Create()
        {
            return new DummyAnalysisPhaseOnePartOne();
        }

        #region IAnalysisPhaseOnePartOne Implementation

        public void ApplyAnalysisToSingleLaunch(IExcelSheet sheet, int currentRow,
            int operationNumber)
        {
            // Do nothing.
        }

        public void ApplyAnalysisToDataset(IExcelSheet sheet)
        {
            // Do nothing.
        }

        public int GetCalculatedSampleSize(IExcelSheet sheet)
        {
            // Do nothing.
            return default;
        }

        #endregion
    }
}

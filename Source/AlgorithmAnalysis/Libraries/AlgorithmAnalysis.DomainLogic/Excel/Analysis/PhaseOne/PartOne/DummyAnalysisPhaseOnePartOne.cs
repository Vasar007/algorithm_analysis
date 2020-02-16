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

        public void ApplyAnalysisToSingleLaunch(ExcelSheet sheet, int operationNumber,
            int currentRow)
        {
            // Do nothing.
        }

        public void ApplyAnalysisToDataset(ExcelSheet sheet)
        {
            // Do nothing.
        }

        public int GetCalculatedSampleSize(ExcelSheet sheet)
        {
            // Do nothing.
            return default;
        }

        #endregion
    }
}

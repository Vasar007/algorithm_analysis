using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo
{
    internal interface IFrequencyHistogramBuilder
    {
        void CreateHistogramData(IExcelSheet sheet);
        bool CheckH0HypothesisByHistogramData(IExcelSheet sheet);
    }
}

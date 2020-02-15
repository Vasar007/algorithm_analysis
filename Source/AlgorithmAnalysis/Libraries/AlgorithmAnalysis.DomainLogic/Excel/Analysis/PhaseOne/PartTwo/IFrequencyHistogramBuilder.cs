namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo
{
    internal interface IFrequencyHistogramBuilder
    {
        void CreateHistogramData(ExcelSheet sheet);
        bool CheckH0HypothesisByHistogramData(ExcelSheet sheet);
    }
}

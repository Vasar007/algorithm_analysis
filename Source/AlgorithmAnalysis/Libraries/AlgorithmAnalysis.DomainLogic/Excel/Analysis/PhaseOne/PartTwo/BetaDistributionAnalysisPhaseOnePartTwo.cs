using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Properties;
using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo
{
    internal sealed class BetaDistributionAnalysisPhaseOnePartTwo : IAnalysisPhaseOnePartTwo
    {
        private readonly IFrequencyHistogramBuilder _histogramBuilder;

        private readonly ParametersPack _args;


        public BetaDistributionAnalysisPhaseOnePartTwo(
            IFrequencyHistogramBuilder histogramBuilder,
            ParametersPack args)
        {
            _histogramBuilder = histogramBuilder.ThrowIfNull(nameof(histogramBuilder));
            _args = args.ThrowIfNull(nameof(args));
        }

        #region IPhaseOnePartOneAnalysis Implementation

        public void ApplyAnalysisToSingleLaunch(IExcelSheet sheet, int operationNumber,
            int currentRow)
        {
            string formula = $"($A{currentRow.ToString()} - $M$6) / ($M$7 - $M$6)";
            sheet[ExcelColumnIndex.B, currentRow].SetFormula(formula);
        }

        public void ApplyAnalysisToDataset(IExcelSheet sheet)
        {
            sheet[ExcelColumnIndex.B, 1].SetValue(ExcelStringsPhaseOnePartTwo.NormalizedColumnName);
            sheet[ExcelColumnIndex.L, 1].SetValue(ExcelStringsPhaseOnePartTwo.NormalDistributionSolutionColumnName);
            sheet.GetOrCreateCenterizedCell(ExcelColumnIndex.M, 1);

            sheet.AddMergedRegion(ExcelColumnIndex.L, 1, ExcelColumnIndex.M, 1);

            sheet[ExcelColumnIndex.L, 2].SetValue(ExcelStringsPhaseOnePartTwo.SampleMean);
            sheet[ExcelColumnIndex.L, 3].SetValue(ExcelStringsPhaseOnePartTwo.SampleVariance);
            sheet[ExcelColumnIndex.L, 4].SetValue(ExcelStringsPhaseOnePartTwo.SampleDeviation);
            sheet[ExcelColumnIndex.L, 5].SetValue(ExcelStringsPhaseOnePartTwo.VariationCoefficient);
            sheet[ExcelColumnIndex.L, 6].SetValue(ExcelStringsPhaseOnePartTwo.TheoreticalMin);
            sheet[ExcelColumnIndex.L, 7].SetValue(ExcelStringsPhaseOnePartTwo.TheoreticalMax);
            sheet[ExcelColumnIndex.L, 8].SetValue(ExcelStringsPhaseOnePartTwo.Span);
            sheet[ExcelColumnIndex.L, 9].SetValue(ExcelStringsPhaseOnePartTwo.NormalizedMean);
            sheet[ExcelColumnIndex.L, 10].SetValue(ExcelStringsPhaseOnePartTwo.NormalizedVarience);
            sheet[ExcelColumnIndex.L, 11].SetValue(ExcelStringsPhaseOnePartTwo.Alpha);
            sheet[ExcelColumnIndex.L, 12].SetValue(ExcelStringsPhaseOnePartTwo.Beta);

            string lastValueRowIndex = _args.LaunchesNumber.SkipHeader().ToString();
            sheet[ExcelColumnIndex.M, 2].SetFormula(sheet.FormulaProvider.Average($"$A$2:$A${lastValueRowIndex}"));
            sheet[ExcelColumnIndex.M, 3].SetFormula(sheet.FormulaProvider.Var($"$A$2:$A${lastValueRowIndex}"));
            sheet[ExcelColumnIndex.M, 4].SetFormula(sheet.FormulaProvider.StdDev($"$A$2:$A${lastValueRowIndex}"));
            sheet[ExcelColumnIndex.M, 5].SetFormula("$M$4 / $M$2");
            sheet[ExcelColumnIndex.M, 6].SetFormula("$J$3");
            sheet[ExcelColumnIndex.M, 7].SetFormula("$J$5");
            sheet[ExcelColumnIndex.M, 8].SetFormula("$M$7 - $M$6");
            sheet[ExcelColumnIndex.M, 9].SetFormula(sheet.FormulaProvider.Average($"$B$2:$B${lastValueRowIndex}"));
            sheet[ExcelColumnIndex.M, 10].SetFormula(sheet.FormulaProvider.Var($"$B$2:$B${lastValueRowIndex}"));
            sheet[ExcelColumnIndex.M, 11].SetFormula("$M$9 * (($M$9 * (1 - $M$9) / $M$10) - 1)");
            sheet[ExcelColumnIndex.M, 12].SetFormula("(1 - $M$9) * (($M$9 * (1 - $M$9) / $M$10) - 1)");

            sheet.AutoSizeColumn(ExcelColumnIndex.B);
            sheet.AutoSizeColumn(ExcelColumnIndex.L);
            sheet.AutoSizeColumn(ExcelColumnIndex.M, useMergedCells: true);

            _histogramBuilder.CreateHistogramData(sheet);
        }

        public bool CheckH0Hypothesis(IExcelSheet sheet)
        {
            return _histogramBuilder.CheckH0HypothesisByHistogramData(sheet);
        }

        #endregion
    }
}

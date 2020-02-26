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
            sheet[ExcelColumnIndex.B, 1].SetValue(ExcelStrings.NormalizedColumnName);
            sheet[ExcelColumnIndex.L, 1].SetValue(ExcelStrings.NormalDistributionSolutionColumnName);
            sheet.GetOrCreateCenterizedCell(ExcelColumnIndex.M, 1);

            sheet.AddMergedRegion(ExcelColumnIndex.L, 1, ExcelColumnIndex.M, 1);

            sheet[ExcelColumnIndex.L, 2].SetValue(ExcelStrings.SampleMean);
            sheet[ExcelColumnIndex.L, 3].SetValue(ExcelStrings.SampleVariance);
            sheet[ExcelColumnIndex.L, 4].SetValue(ExcelStrings.SampleDeviation);
            sheet[ExcelColumnIndex.L, 5].SetValue(ExcelStrings.VariationCoefficient);
            sheet[ExcelColumnIndex.L, 6].SetValue(ExcelStrings.TheoreticalMin);
            sheet[ExcelColumnIndex.L, 7].SetValue(ExcelStrings.TheoreticalMax);
            sheet[ExcelColumnIndex.L, 8].SetValue(ExcelStrings.Span);
            sheet[ExcelColumnIndex.L, 9].SetValue(ExcelStrings.NormalizedMean);
            sheet[ExcelColumnIndex.L, 10].SetValue(ExcelStrings.NormalizedVarience);
            sheet[ExcelColumnIndex.L, 11].SetValue(ExcelStrings.Alpha);
            sheet[ExcelColumnIndex.L, 12].SetValue(ExcelStrings.Beta);

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

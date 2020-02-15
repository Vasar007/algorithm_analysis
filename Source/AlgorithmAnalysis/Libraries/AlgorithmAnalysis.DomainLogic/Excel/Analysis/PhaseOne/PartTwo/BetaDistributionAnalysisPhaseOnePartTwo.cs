using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Properties;

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

        public void ApplyAnalysisToSingleLaunch(ExcelSheet sheet, int operationNumber,
            int currentRow)
        {
            string formula = $"($A{currentRow.ToString()} - $M$6) / ($M$7 - $M$6)";
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.B, currentRow, formula);
        }

        public void ApplyAnalysisToDataset(ExcelSheet sheet)
        {
            sheet.SetCenterizedCellValue(ExcelColumnIndex.B, 1, ExcelStrings.NormalizedColumnName);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.L, 1, ExcelStrings.NormalDistributionSolutionColumnName);
            sheet.GetOrCreateCenterizedCell(ExcelColumnIndex.M, 1);

            sheet.AddMergedRegion(ExcelColumnIndex.L, 1, ExcelColumnIndex.M, 1);

            sheet.SetCenterizedCellValue(ExcelColumnIndex.L, 2, ExcelStrings.SampleMean);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.L, 3, ExcelStrings.SampleVariance);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.L, 4, ExcelStrings.SampleDeviation);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.L, 5, ExcelStrings.VariationCoefficient);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.L, 6, ExcelStrings.TheoreticalMin);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.L, 7, ExcelStrings.TheoreticalMax);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.L, 8, ExcelStrings.Span);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.L, 9, ExcelStrings.NormalizedMean);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.L, 10, ExcelStrings.NormalizedVarience);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.L, 11, ExcelStrings.Alpha);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.L, 12, ExcelStrings.Beta);

            string lastValueRowIndex = _args.LaunchesNumber.SkipHeader().ToString();
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.M, 2, $"AVERAGE($A$2:$A${lastValueRowIndex})");
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.M, 3, $"VAR($A$2:$A${lastValueRowIndex})"); // VAR == VAR.S
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.M, 4, $"STDEV($A$2:$A${lastValueRowIndex})"); // STDEV == STDEV.S
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.M, 5, "$M$4 / $M$2");
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.M, 6, "$J$3");
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.M, 7, "$J$5");
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.M, 8, "$M$7 - $M$6");
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.M, 9, $"AVERAGE($B$2:$B${lastValueRowIndex})");
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.M, 10, $"VAR($B$2:$B${lastValueRowIndex})"); // VAR == VAR.S
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.M, 11, "$M$9 * (($M$9 * (1 - $M$9) / $M$10) - 1)");
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.M, 12, "(1 - $M$9) * (($M$9 * (1 - $M$9) / $M$10) - 1)");

            sheet.AutoSizeColumn(ExcelColumnIndex.B);
            sheet.AutoSizeColumn(ExcelColumnIndex.L);
            sheet.AutoSizeColumn(ExcelColumnIndex.M, useMergedCells: true);

            _histogramBuilder.CreateHistogramData(sheet);
        }

        public bool CheckH0Hypothesis(ExcelSheet sheet)
        {
            return _histogramBuilder.CheckH0HypothesisByHistogramData(sheet);
        }

        #endregion
    }
}

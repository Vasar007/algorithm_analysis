using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Properties;
using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne
{
    internal sealed class NormalDistributionAnalysisPhaseOnePartOne : IAnalysisPhaseOnePartOne
    {
        private readonly ParametersPack _args;


        public NormalDistributionAnalysisPhaseOnePartOne(ParametersPack args)
        {
            _args = args.ThrowIfNull(nameof(args));
        }

        #region IPhaseOnePartOneAnalysis Implementation

        public void ApplyAnalysisToSingleLaunch(IExcelSheet sheet, int currentRow,
            int operationNumber)
        {
            // Do nothing.
        }

        public void ApplyAnalysisToDataset(IExcelSheet sheet)
        {
            sheet[ExcelColumnIndex.J, 1].SetValue(ExcelStringsPhaseOnePartOne.NormalDistributionSolutionColumnName);
            sheet.GetOrCreateCenterizedCell(ExcelColumnIndex.K, 1);

            sheet.AddMergedRegion(ExcelColumnIndex.J, 1, ExcelColumnIndex.K, 1);

            sheet[ExcelColumnIndex.J, 2].SetValue(ExcelStringsPhaseOnePartOne.PreliminarySampleSize);
            sheet[ExcelColumnIndex.J, 3].SetValue(ExcelStringsPhaseOnePartOne.SampleMean);
            sheet[ExcelColumnIndex.J, 4].SetValue(ExcelStringsPhaseOnePartOne.SampleVariance);
            sheet[ExcelColumnIndex.J, 5].SetValue(ExcelStringsPhaseOnePartOne.SampleDeviation);
            sheet[ExcelColumnIndex.J, 6].SetValue(ExcelStringsPhaseOnePartOne.VariationCoefficient);
            sheet[ExcelColumnIndex.J, 7].SetValue(ExcelStringsPhaseOnePartOne.CalculatedSampleSize);

            string lastValueRowIndex = _args.LaunchesNumber.SkipHeader().ToString();
            sheet[ExcelColumnIndex.K, 2].SetFormula("$F$6");
            sheet[ExcelColumnIndex.K, 3].SetFormula(sheet.FormulaProvider.Average($"$A$2:$A${lastValueRowIndex}"));
            sheet[ExcelColumnIndex.K, 4].SetFormula(sheet.FormulaProvider.Var($"$A$2:$A${lastValueRowIndex}"));
            sheet[ExcelColumnIndex.K, 5].SetFormula(sheet.FormulaProvider.StdDev($"$A$2:$A${lastValueRowIndex}"));
            sheet[ExcelColumnIndex.K, 6].SetFormula(ManualFormulaProvider.VariationCoefficient("$K$3", "$K$5"));
            sheet[ExcelColumnIndex.K, 7].SetFormula(sheet.FormulaProvider.RoundUp("3.8416 * $K$6^2 / $F$9^2", "0"));

            sheet.AutoSizeColumn(ExcelColumnIndex.J);
            sheet.AutoSizeColumn(ExcelColumnIndex.K, useMergedCells: true);
        }

        public int GetCalculatedSampleSize(IExcelSheet sheet)
        {
            IExcelCellValueHolder calculatedSampleSize = sheet.EvaluateCell(ExcelColumnIndex.K, 7);

            return Convert.ToInt32(calculatedSampleSize.NumericValue);
        }

        #endregion
    }
}

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

        public void ApplyAnalysisToSingleLaunch(ExcelSheet sheet, int operationNumber,
            int currentRow)
        {
            // Do nothing.
        }

        public void ApplyAnalysisToDataset(ExcelSheet sheet)
        {
            sheet.SetCenterizedCellValue(ExcelColumnIndex.J, 1, ExcelStrings.NormalDistributionSolutionColumnName);
            sheet.GetOrCreateCenterizedCell(ExcelColumnIndex.K, 1);

            sheet.AddMergedRegion(ExcelColumnIndex.J, 1, ExcelColumnIndex.K, 1);

            sheet.SetCenterizedCellValue(ExcelColumnIndex.J, 2, ExcelStrings.PreliminarySampleSize);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.J, 3, ExcelStrings.SampleMean);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.J, 4, ExcelStrings.SampleVariance);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.J, 5, ExcelStrings.SampleDeviation);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.J, 6, ExcelStrings.VariationCoefficient);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.J, 7, ExcelStrings.CalculatedSampleSize);

            string lastValueRowIndex = _args.LaunchesNumber.SkipHeader().ToString();
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.K, 2, "$F$6");
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.K, 3, $"AVERAGE($A$2:$A${lastValueRowIndex})");
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.K, 4, $"VAR($A$2:$A${lastValueRowIndex})"); // VAR == VAR.S
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.K, 5, $"STDEV($A$2:$A${lastValueRowIndex})"); // STDEV == STDEV.S
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.K, 6, "$K$5 / $K$3");
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.K, 7, "ROUNDUP(3.8416 * $K$6^2 / $F$9^2, 0)");

            sheet.AutoSizeColumn(ExcelColumnIndex.J);
            sheet.AutoSizeColumn(ExcelColumnIndex.K, useMergedCells: true);
        }

        public int GetCalculatedSampleSize(ExcelSheet sheet)
        {
            CellValueHolder calculatedSampleSize = sheet.EvaluateCell(ExcelColumnIndex.K, 7);

            return Convert.ToInt32(calculatedSampleSize.NumericValue);
        }

        #endregion
    }
}

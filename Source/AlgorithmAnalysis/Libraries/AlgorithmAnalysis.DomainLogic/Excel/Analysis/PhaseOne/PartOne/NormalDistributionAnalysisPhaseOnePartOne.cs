using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Properties;
using NPOI.SS.UserModel;

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

            // TODO: move this code to the beta distribution analysis.
            //sheet
            //    .GetOrCreateCenterizedCell(ExcelColumnIndex.B, currentRow)
            //    // TODO: use formulas for min and max and fix this formula with $I$5 and $I$6 (min and max).
            //    .SetCellFormula($"($A{currentRow.ToString()} - $F$2) / ($F$6 - $F$2)");
        }

        public void ApplyAnalysisToDataset(ExcelSheet sheet)
        {
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 1)
                .SetCellValue(ExcelStrings.NormalDistributionSolutionColumnName);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.K, 1);

            sheet.AddMergedRegion(ExcelColumnIndex.J, 1, ExcelColumnIndex.K, 1);

            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 2)
                .SetCellValue(ExcelStrings.PreliminarySampleSize);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 3)
                .SetCellValue(ExcelStrings.SampleMean);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 4)
                .SetCellValue(ExcelStrings.SampleVariance);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 5)
                .SetCellValue(ExcelStrings.SampleDeviation);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 6)
                .SetCellValue(ExcelStrings.VariationCoefficient);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 7)
                .SetCellValue(ExcelStrings.CalculatedSampleSize);

            string lastRowIndex = (_args.LaunchesNumber + 1).ToString();
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.K, 2)
                .SetCellFormula("$F$6");
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.K, 3)
                .SetCellFormula($"AVERAGE($A$2:$A${lastRowIndex})");
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.K, 4)
                .SetCellFormula($"VAR($A$2:$A${lastRowIndex})"); // VAR == VAR.S
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.K, 5)
                .SetCellFormula($"STDEV($A$2:$A${lastRowIndex})"); // STDEV == STDEV.S
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.K, 6)
                .SetCellFormula("$K$5 / $K$3");
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.K, 7)
                .SetCellFormula("ROUNDUP(3.8416 * $K$6^2 / $F$9^2, 0)");

            sheet.AutoSizeColumn(ExcelColumnIndex.J);
            sheet.AutoSizeColumn(ExcelColumnIndex.K, useMergedCells: true);
        }

        public int GetCalculatedSampleSize(ExcelSheet sheet)
        {
            ICell cellWithResult = sheet.GetOrCreateCenterizedCell(ExcelColumnIndex.K, 7);
            IWorkbook workbook = cellWithResult.Sheet.Workbook;

            IFormulaEvaluator evaluator = WorkbookFactory.CreateFormulaEvaluator(workbook);
            CellValue cellValue = evaluator.Evaluate(cellWithResult);

            return Convert.ToInt32(cellValue.NumberValue);
        }

        #endregion
    }
}

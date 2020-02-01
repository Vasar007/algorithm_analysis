using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Properties;
using NPOI.SS.UserModel;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis
{
    internal sealed class NormalDistributionAnalysis : IAnalysisPhaseOnePartOne
    {
        private readonly ExcelSheet _sheet;

        private readonly ParametersPack _args;


        public NormalDistributionAnalysis(ExcelSheet sheet, ParametersPack args)
        {
            _sheet = sheet.ThrowIfNull(nameof(sheet));
            _args = args.ThrowIfNull(nameof(args));
        }

        #region Implementation of IPhaseOnePartOneAnalysis

        public void ApplyAnalysisToSingleLaunch(int operationNumber, int currentRow)
        {
            // DO nothing.

            // TODO: move this code to the beta distribution analysis.
            //_sheet
            //    .GetOrCreateCenterizedCell(ExcelColumnIndex.B, currentRow)
            //    // TODO: use formulas for min and max and fix this formula with $I$5 and $I$6 (min and max).
            //    .SetCellFormula($"($A{currentRow.ToString()} - $F$2) / ($F$6 - $F$2)");
        }

        public void ApplyAnalysisToDataset()
        {
            _sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 1)
                .SetCellValue(ExcelStrings.NormalDistributionSolutionColumnName);
            _sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.K, 1)
;
            _sheet.AddMergedRegion(ExcelColumnIndex.J, 1, ExcelColumnIndex.K, 1);

            _sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 2)
                .SetCellValue(ExcelStrings.PreliminarySampleSize);
            _sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 3)
                .SetCellValue(ExcelStrings.SampleMean);
            _sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 4)
                .SetCellValue(ExcelStrings.SampleVariance);
            _sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 5)
                .SetCellValue(ExcelStrings.SampleDeviation);
            _sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 6)
                .SetCellValue(ExcelStrings.VariationCoefficient);
            _sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 7)
                .SetCellValue(ExcelStrings.CalculatedSampleSize);

            string lastRowIndex = (_args.LaunchesNumber + 1).ToString();
            _sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.K, 2)
                .SetCellFormula("$F$6");
            _sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.K, 3)
                .SetCellFormula($"AVERAGE($A$2:$A${lastRowIndex})");
            _sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.K, 4)
                .SetCellFormula($"VAR($A$2:$A${lastRowIndex})"); // VAR == VAR.S
            _sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.K, 5)
                .SetCellFormula($"STDEV($A$2:$A${lastRowIndex})"); // STDEV == STDEV.S
            _sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.K, 6)
                .SetCellFormula("$K$5 / $K$3");
            _sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.K, 7)
                .SetCellFormula("ROUNDUP(3.8416 * $K$6^2 / $F$9^2, 0)");

            _sheet.AutoSizeColumn(ExcelColumnIndex.J);
            _sheet.AutoSizeColumn(ExcelColumnIndex.K, useMergedCells: true);
        }

        public int GetCalculatedSampleSize()
        {
            ICell cellWithResult = _sheet.GetOrCreateCenterizedCell(ExcelColumnIndex.K, 7);
            IWorkbook workbook = cellWithResult.Sheet.Workbook;

            IFormulaEvaluator evaluator = WorkbookFactory.CreateFormulaEvaluator(workbook);
            CellValue cellValue = evaluator.Evaluate(cellWithResult);

            return Convert.ToInt32(cellValue.NumberValue);
        }

        #endregion
    }
}

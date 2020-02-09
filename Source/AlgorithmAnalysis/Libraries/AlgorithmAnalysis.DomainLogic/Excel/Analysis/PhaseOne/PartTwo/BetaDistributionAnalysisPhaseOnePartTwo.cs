using Acolyte.Assertions;
using NPOI.SS.UserModel;
using AlgorithmAnalysis.DomainLogic.Properties;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo
{
    internal sealed class BetaDistributionAnalysisPhaseOnePartTwo : IAnalysisPhaseOnePartTwo
    {
        private readonly ParametersPack _args;


        public BetaDistributionAnalysisPhaseOnePartTwo(ParametersPack args)
        {
            _args = args.ThrowIfNull(nameof(args));
        }

        #region IPhaseOnePartOneAnalysis Implementation

        public void ApplyAnalysisToSingleLaunch(ExcelSheet sheet, int operationNumber,
            int currentRow)
        {
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.B, currentRow)
                // TODO: use formulas for min and max and fix this formula with $I$5 and $I$6 (min and max).
                .SetCellFormula($"($A{currentRow.ToString()} - $J$2) / ($J$6 - $J$2)");
        }

        public void ApplyAnalysisToDataset(ExcelSheet sheet)
        {
            // TODO: change this function to apply appropriate formulas.
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.L, 1)
                .SetCellValue(ExcelStrings.NormalDistributionSolutionColumnName);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.M, 1);

            sheet.AddMergedRegion(ExcelColumnIndex.L, 1, ExcelColumnIndex.M, 1);

            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.L, 2)
                .SetCellValue(ExcelStrings.PreliminarySampleSize);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.L, 3)
                .SetCellValue(ExcelStrings.SampleMean);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.L, 4)
                .SetCellValue(ExcelStrings.SampleVariance);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.L, 5)
                .SetCellValue(ExcelStrings.SampleDeviation);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.L, 6)
                .SetCellValue(ExcelStrings.VariationCoefficient);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.L, 7)
                .SetCellValue(ExcelStrings.CalculatedSampleSize);

            string lastRowIndex = (_args.LaunchesNumber + 1).ToString();
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.M, 2)
                .SetCellFormula("$F$6");
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.M, 3)
                .SetCellFormula($"AVERAGE($A$2:$A${lastRowIndex})");
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.M, 4)
                .SetCellFormula($"VAR($A$2:$A${lastRowIndex})"); // VAR == VAR.S
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.M, 5)
                .SetCellFormula($"STDEV($A$2:$A${lastRowIndex})"); // STDEV == STDEV.S
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.M, 6)
                .SetCellFormula("$M$5 / $M$3");
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.M, 7)
                .SetCellFormula("ROUNDUP(3.8416 * $M$6^2 / $J$9^2, 0)");

            sheet.AutoSizeColumn(ExcelColumnIndex.L);
            sheet.AutoSizeColumn(ExcelColumnIndex.M, useMergedCells: true);
        }

        public bool CheckH0Hypothesis(ExcelSheet sheet)
        {
            // TODO: change this function to read critical chi^2 value of the result Excel sheet.
            ICell cellWithResult = sheet.GetOrCreateCenterizedCell(ExcelColumnIndex.M, 7);
            IWorkbook workbook = cellWithResult.Sheet.Workbook;

            IFormulaEvaluator evaluator = WorkbookFactory.CreateFormulaEvaluator(workbook);
            CellValue cellValue = evaluator.Evaluate(cellWithResult);

            return cellValue.NumberValue > 0.0;
        }

        #endregion
    }
}

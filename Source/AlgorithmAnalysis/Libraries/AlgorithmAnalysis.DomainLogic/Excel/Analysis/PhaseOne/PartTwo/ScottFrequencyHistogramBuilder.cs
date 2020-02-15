using NPOI.SS.UserModel;
using AlgorithmAnalysis.DomainLogic.Properties;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo
{
    internal sealed class ScottFrequencyHistogramBuilder : IFrequencyHistogramBuilder
    {
        public ScottFrequencyHistogramBuilder()
        {
        }

        #region IFrequencyHistogramBuilder Implementation

        
        public void CreateHistogramData(ExcelSheet sheet)
        {
            sheet.SetCenterizedCellValue(ExcelColumnIndex.D, 1, ExcelStrings.PocketColumnName);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.E, 1, ExcelStrings.FrequencyColumnName);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.F, 1, ExcelStrings.EmpiricalFrequencyColumnName);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.G, 1, ExcelStrings.TheoreticalFrequencyColumnName);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.H, 1, ExcelStrings.Chi2ValueColumnName);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 10, ExcelStrings.HistogramSemisegmentsNumber);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 11, ExcelStrings.Chi2);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 12, ExcelStrings.FreedomDegreesNumber);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 13, ExcelStrings.Chi2Critical);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 14, ExcelStrings.CheckTestFucntion);

            // TODO: apply appropriate formulas.

            // J10 = (3.5 * STDEV.S($B$2:$B${LAUNCHES_NUMBER + 1})) / (J6^(1/3))
            // J11 = SEGMENTS_NUMBER_FORMULA
            // J12 = SUM($H$2:$H${J11 + 1}) * $J$6
            // J13 = $J$11 - 1 - 2
            // J14 = CHIINV($J$8, $J$13)
            // J15 = CHITEST($F$2:$F${J11 + 1}, $G$2:$G${J11 + 1})

            //sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 10, ExcelStrings.IntervalLengthFormula);
            //sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 11, ExcelStrings.SegmentsNumberFormula);
            //sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 12, ExcelStrings.Chi2Formula);
            //sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 13, ExcelStrings.FreedomDegreesNumberFormula);
            //sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 14, ExcelStrings.Chi2CriticalFormula);
            //sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 15, ExcelStrings.CheckTestFucntionFormula);

            sheet.AutoSizeColumn(ExcelColumnIndex.D);
            sheet.AutoSizeColumn(ExcelColumnIndex.E);
            sheet.AutoSizeColumn(ExcelColumnIndex.F);
            sheet.AutoSizeColumn(ExcelColumnIndex.G);
            sheet.AutoSizeColumn(ExcelColumnIndex.H);
            sheet.AutoSizeColumn(ExcelColumnIndex.I);
            sheet.AutoSizeColumn(ExcelColumnIndex.J);
        }

        public bool CheckH0HypothesisByHistogramData(ExcelSheet sheet)
        {
            // TODO: change this function to read critical chi^2 value of the result Excel sheet.
            ICell cellWithResult = sheet.GetOrCreateCenterizedCell(ExcelColumnIndex.M, 12);
            IWorkbook workbook = cellWithResult.Sheet.Workbook;

            IFormulaEvaluator evaluator = WorkbookFactory.CreateFormulaEvaluator(workbook);
            CellValue cellValue = evaluator.Evaluate(cellWithResult);

            return cellValue.NumberValue > 0.0;
        }

        #endregion
    }
}

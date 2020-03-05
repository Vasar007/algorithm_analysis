using Acolyte.Assertions;
using OfficeOpenXml;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Excel.Formulas;

namespace AlgorithmAnalysis.Excel.EPPlus
{
    internal sealed class EpplusExcelSheet : BaseExcelSheet, IExcelSheet
    {
        // EPPlus library uses one-based indexing and support almost all required features.

        private readonly ExcelWorksheet _sheet;


        internal EpplusExcelSheet(
            ExcelWorksheet sheet,
            ExcelOptions excelOptions,
            IExcelFormulaProvider formulaProvider)
            : base(excelOptions, formulaProvider)
        {
            _sheet = sheet.ThrowIfNull(nameof(sheet));
        }

        public override IExcelCellHolder GetOrCreateCell(ExcelColumnIndex columnIndex, int rowIndex,
            bool centrized)
        {
            int columnIndexInt = columnIndex.AsInt32().UseOneBasedIndexing();
            columnIndexInt.ThrowIfValueIsOutOfRange(nameof(columnIndex), 1, int.MaxValue);
            rowIndex.ThrowIfValueIsOutOfRange(nameof(rowIndex), 1, int.MaxValue);

            ExcelRange excelRange = _sheet.Cells[rowIndex, columnIndexInt];

            excelRange = centrized
                ? excelRange.Center()
                : excelRange;

            return new EpplusExcelCellHolder(excelRange);
        }

        public override void AddMergedRegion(
            ExcelColumnIndex firstColumnIndex,
            int firstRowIndex,
            ExcelColumnIndex lastColumnIndex,
            int lastRowIndex)
        {
            int firstColumnIndexInt = firstColumnIndex.AsInt32().UseOneBasedIndexing();
            firstColumnIndexInt.ThrowIfValueIsOutOfRange(nameof(firstRowIndex), 1, int.MaxValue);
            firstRowIndex.ThrowIfValueIsOutOfRange(nameof(firstRowIndex), 1, int.MaxValue);

            int lastColumnIndexInt = lastColumnIndex.AsInt32().UseOneBasedIndexing();
            lastColumnIndexInt.ThrowIfValueIsOutOfRange(nameof(lastRowIndex), 1, int.MaxValue);
            lastRowIndex.ThrowIfValueIsOutOfRange(nameof(lastRowIndex), 1, int.MaxValue);

            _sheet.Cells[
                firstRowIndex, firstColumnIndexInt,
                lastRowIndex, lastColumnIndexInt
            ].Merge = true;
        }

        public override void AutoSizeColumn(ExcelColumnIndex columnIndex)
        {
            int columnIndexInt = columnIndex.AsInt32().UseOneBasedIndexing();
            columnIndexInt.ThrowIfValueIsOutOfRange(nameof(columnIndex), 1, int.MaxValue);

            _sheet.Column(columnIndexInt).AutoFit();
        }

        public override void AutoSizeColumn(ExcelColumnIndex columnIndex, bool useMergedCells)
        {
            int columnIndexInt = columnIndex.AsInt32().UseOneBasedIndexing();
            columnIndexInt.ThrowIfValueIsOutOfRange(nameof(columnIndex), 1, int.MaxValue);

            // TODO: find a way to enable auto size for merged cells.
            _sheet.Column(columnIndexInt).AutoFit();
        }

        public override void EvaluateAll()
        {
            _sheet.Calculate();
        }

        public override void SetArrayFormula(
            string arrayFormula,
            ExcelColumnIndex firstColumnIndex,
            int firstRowIndex,
            ExcelColumnIndex lastColumnIndex,
            int lastRowIndex)
        {
            arrayFormula.ThrowIfNullOrWhiteSpace(nameof(arrayFormula));

            int firstColumnIndexInt = firstColumnIndex.AsInt32().UseOneBasedIndexing();
            firstColumnIndexInt.ThrowIfValueIsOutOfRange(nameof(firstRowIndex), 1, int.MaxValue);
            firstRowIndex.ThrowIfValueIsOutOfRange(nameof(firstRowIndex), 1, int.MaxValue);

            int lastColumnIndexInt = lastColumnIndex.AsInt32().UseOneBasedIndexing();
            lastColumnIndexInt.ThrowIfValueIsOutOfRange(nameof(lastRowIndex), 1, int.MaxValue);
            lastRowIndex.ThrowIfValueIsOutOfRange(nameof(lastRowIndex), 1, int.MaxValue);

            _sheet.Cells[
                 firstRowIndex, firstColumnIndexInt,
                lastRowIndex, lastColumnIndexInt
            ].CreateArrayFormula(arrayFormula);
        }
    }
}

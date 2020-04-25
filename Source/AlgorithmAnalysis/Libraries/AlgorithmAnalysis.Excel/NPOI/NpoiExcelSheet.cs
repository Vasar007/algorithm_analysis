using Acolyte.Assertions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Excel.Formulas;

namespace AlgorithmAnalysis.Excel.NPOI
{
    internal sealed class NpoiExcelSheet : BaseExcelSheet, IExcelSheet
    {
        // Suitable for Excel 2007. If you try to use the latest Excel 2019 functions, NPOI can
        // throw NotImplementedException.

        // ATTENTION! This class serves as suitable interface to work with Excel sheet.
        // NPOI library uses zero-based indexing which is counterintuitive.
        // That's why we use one-based indexing for row indexing. We use enum to work with
        // column indexing. However, enum values are zero-based to simplify internal conversion to 
        // NPOI library calls. You should never usually use AsInt32 method in outside logic for
        // ExcelColumnIndex enum.

        private readonly ISheet _sheet;


        internal NpoiExcelSheet(
            ISheet sheet,
            ReportOptions excelOptions,
            IExcelFormulaProvider formulaProvider)
            : base(excelOptions, formulaProvider)
        {
            _sheet = sheet.ThrowIfNull(nameof(sheet));
        }

        public override IExcelCellHolder GetOrCreateCell(ExcelColumnIndex columnIndex, int rowIndex,
            bool centrized)
        {
            rowIndex.ThrowIfValueIsOutOfRange(nameof(rowIndex), 1, int.MaxValue);
            int columIndexInt = columnIndex.AsInt32();
            columIndexInt.ThrowIfValueIsOutOfRange(nameof(columnIndex), 0, int.MaxValue);

            // Because of using one-based indexing.
            int fixedRowIndex = rowIndex - 1;
            IRow row = GetOrCreateRow(fixedRowIndex, centrized);

            ICell cell = row.GetCell(columIndexInt);

            ICell result = cell is null
                ? row.CreateCell(columIndexInt)
                : cell;

            result = centrized
                ? result.Center()
                : result;

            return new NpoiExcelCellHolder(result);
        }

        public override void AddMergedRegion(
            ExcelColumnIndex firstColumnIndex,
            int firstRowIndex,
            ExcelColumnIndex lastColumnIndex,
            int lastRowIndex)
        {
            int firstColumIndexInt = firstColumnIndex.AsInt32();
            firstColumIndexInt.ThrowIfValueIsOutOfRange(nameof(firstColumnIndex), 0, int.MaxValue);
            firstRowIndex.ThrowIfValueIsOutOfRange(nameof(firstRowIndex), 1, int.MaxValue);

            int lastColumIndexInt = lastColumnIndex.AsInt32();
            lastColumIndexInt.ThrowIfValueIsOutOfRange(nameof(lastColumnIndex), 0, int.MaxValue);
            lastRowIndex.ThrowIfValueIsOutOfRange(nameof(lastRowIndex), 1, int.MaxValue);

            var cra = new CellRangeAddress(
                firstRow: firstRowIndex - 1, // Because of using one-based indexing.
                lastRow: lastRowIndex - 1, // Because of using one-based indexing.
                firstCol: firstColumIndexInt,
                lastCol: lastColumIndexInt
            );
            _sheet.AddMergedRegion(cra);
        }

        public override void AutoSizeColumn(ExcelColumnIndex columnIndex)
        {
            int columnIndexInt = columnIndex.AsInt32();
            columnIndexInt.ThrowIfValueIsOutOfRange(nameof(columnIndex), 1, int.MaxValue);

            _sheet.AutoSizeColumn(columnIndexInt);
        }

        public override void AutoSizeColumn(ExcelColumnIndex columnIndex, bool useMergedCells)
        {
            int columnIndexInt = columnIndex.AsInt32();
            columnIndexInt.ThrowIfValueIsOutOfRange(nameof(columnIndex), 1, int.MaxValue);

            _sheet.AutoSizeColumn(columnIndexInt, useMergedCells);
        }

        public override void EvaluateAll()
        {
            XSSFFormulaEvaluator.EvaluateAllFormulaCells(_sheet.Workbook);
        }

        public override void SetArrayFormula(
            string arrayFormula,
            ExcelColumnIndex firstColumnIndex,
            int firstRowIndex,
            ExcelColumnIndex lastColumnIndex,
            int lastRowIndex)
        {
            int firstColumIndexInt = firstColumnIndex.AsInt32();
            firstColumIndexInt.ThrowIfValueIsOutOfRange(nameof(firstColumnIndex), 0, int.MaxValue);
            firstRowIndex.ThrowIfValueIsOutOfRange(nameof(firstRowIndex), 1, int.MaxValue);

            int lastColumIndexInt = lastColumnIndex.AsInt32();
            lastColumIndexInt.ThrowIfValueIsOutOfRange(nameof(lastColumnIndex), 0, int.MaxValue);
            lastRowIndex.ThrowIfValueIsOutOfRange(nameof(lastRowIndex), 1, int.MaxValue);

            var cra = new CellRangeAddress(
                firstRow: firstRowIndex - 1, // Because of using one-based indexing.
                lastRow: lastRowIndex - 1, // Because of using one-based indexing.
                firstCol: firstColumIndexInt,
                lastCol: lastColumIndexInt
            );
            _sheet.SetArrayFormula(arrayFormula, cra);
        }

        private IRow GetOrCreateRow(int rowIndex, bool centrized)
        {
            rowIndex.ThrowIfValueIsOutOfRange(nameof(rowIndex), 0, int.MaxValue);

            IRow row = _sheet.GetRow(rowIndex);

            row = row is null
                ? _sheet.CreateRow(rowIndex)
                : row;

            return centrized
                ? row.Center()
                : row;
        }
    }
}

using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.Common;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

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
            ExcelOptions excelOptions)
            : base(excelOptions)
        {
            _sheet = sheet.ThrowIfNull(nameof(sheet));
        }

        public override IExcelCellHolder GetOrCreateCell(ExcelColumnIndex columnIndex, int rowIndex,
            bool centrized)
        {
            columnIndex.ThrowIfEnumValueIsUndefined(nameof(columnIndex));
            rowIndex.ThrowIfValueIsOutOfRange(nameof(rowIndex), 1, int.MaxValue);

            // Because of using one-based indexing.
            int fixedRowIndex = rowIndex - 1;
            IRow row = GetOrCreateRow(fixedRowIndex);

            if (centrized)
            {
                row.Center();
            }

            int columIndexInt = columnIndex.AsInt32();
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
            firstColumnIndex.ThrowIfEnumValueIsUndefined(nameof(firstColumnIndex));
            firstRowIndex.ThrowIfValueIsOutOfRange(nameof(firstRowIndex), 1, int.MaxValue);
            lastColumnIndex.ThrowIfEnumValueIsUndefined(nameof(lastColumnIndex));
            lastRowIndex.ThrowIfValueIsOutOfRange(nameof(lastRowIndex), 1, int.MaxValue);

            var cra = new CellRangeAddress(
                firstRow: firstRowIndex - 1, // Because of using one-based indexing.
                lastRow: lastRowIndex - 1, // Because of using one-based indexing.
                firstCol: firstColumnIndex.AsInt32(),
                lastCol: lastColumnIndex.AsInt32()
            );
            _sheet.AddMergedRegion(cra);
        }

        public override void AutoSizeColumn(ExcelColumnIndex columnIndex)
        {
            columnIndex.ThrowIfEnumValueIsUndefined(nameof(columnIndex));

            _sheet.AutoSizeColumn(columnIndex.AsInt32());
        }

        public override void AutoSizeColumn(ExcelColumnIndex columnIndex, bool useMergedCells)
        {
            columnIndex.ThrowIfEnumValueIsUndefined(nameof(columnIndex));

            _sheet.AutoSizeColumn(columnIndex.AsInt32(), useMergedCells);
        }

        public override void EvaluateAll()
        {
            XSSFFormulaEvaluator.EvaluateAllFormulaCells(_sheet.Workbook);
        }

        public override IExcelCellValueHolder EvaluateCell(ExcelColumnIndex columnIndex,
            int rowIndex)
        {
            IExcelCellHolder cellWithResult = GetOrCreateCenterizedCell(columnIndex, rowIndex);
            if (!(cellWithResult is NpoiExcelCellHolder cellHolder))
            {
                throw new InvalidOperationException(
                    "Failed to evaluate cell: sheet class uses inappropriate cell type."
                );
            }

            IWorkbook workbook = cellHolder.Sheet.Workbook;

            IFormulaEvaluator evaluator = WorkbookFactory.CreateFormulaEvaluator(workbook);
            return NpoiExcelCellValueHolder.CreateFrom(evaluator.Evaluate(cellHolder.Cell));
        }

        public override void SetArrayFormula(
            string arrayFormula,
            ExcelColumnIndex firstColumnIndex,
            int firstRowIndex,
            ExcelColumnIndex lastColumnIndex,
            int lastRowIndex)
        {
            arrayFormula.ThrowIfNullOrWhiteSpace(nameof(arrayFormula));
            firstColumnIndex.ThrowIfEnumValueIsUndefined(nameof(firstColumnIndex));
            firstRowIndex.ThrowIfValueIsOutOfRange(nameof(firstRowIndex), 1, int.MaxValue);
            lastColumnIndex.ThrowIfEnumValueIsUndefined(nameof(lastColumnIndex));
            lastRowIndex.ThrowIfValueIsOutOfRange(nameof(lastRowIndex), 1, int.MaxValue);

            var cra = new CellRangeAddress(
                firstRow: firstRowIndex - 1, // Because of using one-based indexing.
                lastRow: lastRowIndex - 1, // Because of using one-based indexing.
                firstCol: firstColumnIndex.AsInt32(),
                lastCol: lastColumnIndex.AsInt32()
            );
            _sheet.SetArrayFormula(arrayFormula, cra);
        }

        private IRow GetOrCreateRow(int rowIndex)
        {
            rowIndex.ThrowIfValueIsOutOfRange(nameof(rowIndex), 0, int.MaxValue);

            IRow row = _sheet.GetRow(rowIndex);

            return row is null
                ? _sheet.CreateRow(rowIndex)
                : row;
        }
    }
}

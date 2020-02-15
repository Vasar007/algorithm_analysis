using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.Models;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelSheet
    {
        private readonly ISheet _sheet;


        public ExcelSheet(ISheet sheet)
        {
            _sheet = sheet.ThrowIfNull(nameof(sheet));
        }

        public IRow GetOrCreateRow(int rowIndex)
        {
            rowIndex.ThrowIfValueIsOutOfRange(nameof(rowIndex), 0, int.MaxValue);

            IRow row = _sheet.GetRow(rowIndex);

            return row is null
                ? _sheet.CreateRow(rowIndex)
                : row;
        }

        public IRow GetOrCreateCenterizedRow(int rowIndex)
        {
            return GetOrCreateRow(rowIndex).Center();
        }

        public ICell GetOrCreateCell(ExcelColumnIndex columnIndex, int rowIndex, bool centrized)
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

            return centrized
                ? result.Center()
                : result;
        }

        public ICell GetOrCreateCell(ExcelColumnIndex columnIndex, int rowIndex)
        {
            return GetOrCreateCell(columnIndex, rowIndex, centrized: false);
        }

        public ICell GetOrCreateCellInCentrizedRow(ExcelColumnIndex columnIndex, int rowIndex)
        {
            return GetOrCreateCell(columnIndex, rowIndex, centrized: true);
        }

        public ICell GetOrCreateCenterizedCell(ExcelColumnIndex columnIndex, int rowIndex)
        {
            return GetOrCreateCell(columnIndex, rowIndex).Center();
        }

        public void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, string value)
        {
            GetOrCreateCell(columnIndex, rowIndex).SetCellValue(value);
        }

        public void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, bool value)
        {
            GetOrCreateCell(columnIndex, rowIndex).SetCellValue(value);
        }

        public void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, double value)
        {
            GetOrCreateCell(columnIndex, rowIndex).SetCellValue(value);
        }

        public void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, DateTime value)
        {
            GetOrCreateCell(columnIndex, rowIndex).SetCellValue(value);
        }

        public void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex,
            IRichTextString value)
        {
            GetOrCreateCell(columnIndex, rowIndex).SetCellValue(value);
        }

        public void SetCellFormula(ExcelColumnIndex columnIndex, int rowIndex, string formula)
        {
            GetOrCreateCell(columnIndex, rowIndex).SetCellFormula(formula);
        }

        public void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex, string value)
        {
            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetCellValue(value);
        }

        public void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex, bool value)
        {
            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetCellValue(value);
        }

        public void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex, double value)
        {
            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetCellValue(value);
        }

        public void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex, DateTime value)
        {
            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetCellValue(value);
        }

        public void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex,
            IRichTextString value)
        {
            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetCellValue(value);
        }

        public void SetCenterizedCellFormula(ExcelColumnIndex columnIndex, int rowIndex, string formula)
        {
            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetCellFormula(formula);
        }

        public int AddMergedRegion(
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
            return _sheet.AddMergedRegion(cra);
        }

        public void AutoSizeColumn(ExcelColumnIndex columnIndex)
        {
            columnIndex.ThrowIfEnumValueIsUndefined(nameof(columnIndex));

            _sheet.AutoSizeColumn(columnIndex.AsInt32());
        }

        public void AutoSizeColumn(ExcelColumnIndex columnIndex, bool useMergedCells)
        {
            columnIndex.ThrowIfEnumValueIsUndefined(nameof(columnIndex));

            _sheet.AutoSizeColumn(columnIndex.AsInt32(), useMergedCells);
        }
    }
}

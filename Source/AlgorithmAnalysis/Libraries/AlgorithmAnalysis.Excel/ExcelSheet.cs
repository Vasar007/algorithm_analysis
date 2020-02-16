using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.Models;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace AlgorithmAnalysis.Excel
{
    public sealed class ExcelSheet
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

        private readonly ExcelOptions _excelOptions;

        public CellHolder this[ExcelColumnIndex columnIndex, int rowIndex]
        {
            get
            {
                return _excelOptions.CellCreationMode switch
                {
                    ExcelCellCreationMode.Default =>
                        GetOrCreateCell(columnIndex, rowIndex),

                    ExcelCellCreationMode.Centerized =>
                        GetOrCreateCenterizedCell(columnIndex, rowIndex),

                    _ => throw new ArgumentOutOfRangeException(
                             nameof(_excelOptions), _excelOptions.CellCreationMode,
                             $"Unknown cell creation mode: '{_excelOptions.CellCreationMode.ToString()}'."
                         )
                };
            }
        }


        public ExcelSheet(ISheet sheet, ExcelOptions excelOptions)
        {
            _sheet = sheet.ThrowIfNull(nameof(sheet));
            _excelOptions = excelOptions.ThrowIfNull(nameof(excelOptions));
        }

        public ExcelSheet(ISheet sheet)
            : this(sheet, ConfigOptions.Excel)
        {
        }

        public CellHolder GetOrCreateCell(ExcelColumnIndex columnIndex, int rowIndex,
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

            return new CellHolder(result);
        }

        public CellHolder GetOrCreateCell(ExcelColumnIndex columnIndex, int rowIndex)
        {
            return GetOrCreateCell(columnIndex, rowIndex, centrized: false);
        }

        public CellHolder GetOrCreateCenterizedCell(ExcelColumnIndex columnIndex, int rowIndex)
        {
            return GetOrCreateCell(columnIndex, rowIndex, centrized: true);
        }

        public void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, string value)
        {
            GetOrCreateCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, bool value)
        {
            GetOrCreateCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, double value)
        {
            GetOrCreateCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, DateTime value)
        {
            GetOrCreateCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCellFormula(ExcelColumnIndex columnIndex, int rowIndex, string formula)
        {
            formula.ThrowIfNullOrWhiteSpace(nameof(formula));

            GetOrCreateCell(columnIndex, rowIndex).SetFormula(formula);
        }

        public void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex, string value)
        {
            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex, bool value)
        {
            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex, double value)
        {
            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex, DateTime value)
        {
            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCenterizedCellFormula(ExcelColumnIndex columnIndex, int rowIndex, string formula)
        {
            formula.ThrowIfNullOrWhiteSpace(nameof(formula));

            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetFormula(formula);
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

        public CellValueHolder EvaluateCell(ExcelColumnIndex columnIndex, int rowIndex)
        {
            CellHolder cellWithResult = GetOrCreateCenterizedCell(columnIndex, rowIndex);
            IWorkbook workbook = cellWithResult.Sheet.Workbook;

            IFormulaEvaluator evaluator = WorkbookFactory.CreateFormulaEvaluator(workbook);
            return CellValueHolder.CreateFrom(evaluator.Evaluate(cellWithResult.Cell));
        }

        public void SetArrayFormula(
            string formula,
            ExcelColumnIndex firstColumnIndex,
            int firstRowIndex,
            ExcelColumnIndex lastColumnIndex,
            int lastRowIndex)
        {
            formula.ThrowIfNullOrWhiteSpace(nameof(formula));
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
            _sheet.SetArrayFormula(formula, cra);
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

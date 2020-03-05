using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common;
using NPOI.SS.UserModel;

namespace AlgorithmAnalysis.Excel.NPOI
{
    internal sealed class NpoiExcelCellHolder : IExcelCellHolder
    {
        private readonly ICell _cell;

        public ActiveExcelCellType ActiveType { get; private set; }

        public string StringValue
        {
            get => _cell.StringCellValue;

            private set
            {
                ResetActiveType();
                SetValue(value);
            }
        }

        public double NumericValue
        {
            get => _cell.NumericCellValue;

            private set
            {
                ResetActiveType();
                SetValue(value);
            }
        }

        public bool BooleanValue
        {
            get => _cell.BooleanCellValue;

            private set
            {
                ResetActiveType();
                SetValue(value);
            }
        }

        public DateTime DateTimeValue
        {
            get => _cell.DateCellValue;

            private set
            {
                ResetActiveType();
                SetValue(value);
            }
        }

        public byte ErrorValue
        {
            get => _cell.ErrorCellValue;

            private set
            {
                ResetActiveType();
                SetValue(value);
            }
        }

        public string Formula
        {
            get => _cell.CellFormula;

            private set
            {
                ResetActiveType();
                SetValue(value);
            }
        }

        // Can return invalid addresses if ExcelColumnIndex does not contain value for
        // _cell.ColumnIndex.
        public string FullAddress =>
            $"{_cell.ColumnIndex.AsEnum<ExcelColumnIndex>().ToString()}" +
            $"{(_cell.RowIndex + 1).ToString()}";


        public NpoiExcelCellHolder(ICell cell)
        {
            _cell = cell.ThrowIfNull(nameof(cell));
            ActiveType = cell.CellType.TransformCellType();
        }

        public void SetValue(string value)
        {
            value.ThrowIfNull(nameof(value));

            _cell.SetCellValue(value);
            ActiveType = ActiveExcelCellType.String;
        }

        public void SetValue(double value)
        {
            _cell.SetCellValue(value);
            ActiveType = ActiveExcelCellType.Numeric;
        }

        public void SetValue(bool value)
        {
            _cell.SetCellValue(value);
            ActiveType = ActiveExcelCellType.Boolean;
        }

        public void SetValue(DateTime value)
        {
            _cell.SetCellValue(value);
            ActiveType = ActiveExcelCellType.DateTime;
        }

        public void SetErrorValue(byte value)
        {
            _cell.SetCellErrorValue(value);
            ActiveType = ActiveExcelCellType.Error;
        }

        public void SetFormula(string formula)
        {
            formula.ThrowIfNull(nameof(formula));

            _cell.SetCellFormula(formula);
            ActiveType = ActiveExcelCellType.Formula;
        }

        public IExcelCellValueHolder Evaluate()
        {
            IWorkbook workbook = _cell.Sheet.Workbook;

            IFormulaEvaluator evaluator = WorkbookFactory.CreateFormulaEvaluator(workbook);
            return NpoiExcelCellValueHolder.CreateFrom(evaluator.Evaluate(_cell));
        }

        private void ResetActiveType()
        {
            ActiveType = ActiveExcelCellType.Unknown;
        }
    }
}

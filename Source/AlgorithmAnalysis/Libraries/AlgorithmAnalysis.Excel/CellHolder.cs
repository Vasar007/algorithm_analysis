using System;
using Acolyte.Assertions;
using NPOI.SS.UserModel;

namespace AlgorithmAnalysis.Excel
{
    public sealed class CellHolder
    {
        private readonly ICell _cell;

        public ActiveCellType ActiveType { get; private set; }

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

        internal ISheet Sheet => _cell.Sheet;

        internal ICell Cell => _cell;


        public CellHolder(ICell cell)
        {
            ResetActiveType();
            _cell = cell.ThrowIfNull(nameof(cell));
        }

        public void SetValue(string value)
        {
            _cell.SetCellValue(value.ThrowIfNull(nameof(value)));
            ActiveType = ActiveCellType.String;
        }

        public void SetValue(double value)
        {
            _cell.SetCellValue(value);
            ActiveType = ActiveCellType.Numeric;
        }

        public void SetValue(bool value)
        {
            _cell.SetCellValue(value);
            ActiveType = ActiveCellType.Boolean;
        }

        public void SetValue(DateTime value)
        {
            _cell.SetCellValue(value);
            ActiveType = ActiveCellType.DateTime;
        }

        public void SetErrorValue(byte value)
        {
            _cell.SetCellErrorValue(value);
            ActiveType = ActiveCellType.Error;
        }

        public void SetFormula(string formula)
        {
            _cell.SetCellFormula(formula.ThrowIfNull(nameof(formula)));
            ActiveType = ActiveCellType.Formula;
        }

        private void ResetActiveType()
        {
            ActiveType = ActiveCellType.Unknown;
        }
    }
}

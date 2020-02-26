using System;
using Acolyte.Assertions;
using OfficeOpenXml;

namespace AlgorithmAnalysis.Excel.EPPlus
{
    internal sealed class EpplusExcelCellHolder : IExcelCellHolder
    {
        private readonly ExcelRange _cell;

        public ActiveExcelCellType ActiveType { get; private set; }

        public string StringValue
        {
            get => _cell.GetValue<string>();

            private set
            {
                ResetActiveType();
                SetValue(value);
            }
        }

        public double NumericValue
        {
            get => _cell.GetValue<double>();

            private set
            {
                ResetActiveType();
                SetValue(value);
            }
        }

        public bool BooleanValue
        {
            get => _cell.GetValue<bool>();

            private set
            {
                ResetActiveType();
                SetValue(value);
            }
        }

        public DateTime DateTimeValue
        {
            get => _cell.GetValue<DateTime>();

            private set
            {
                ResetActiveType();
                SetValue(value);
            }
        }

        public byte ErrorValue
        {
            get => _cell.GetValue<byte>();

            private set
            {
                ResetActiveType();
                SetValue(value);
            }
        }

        public string Formula
        {
            get => _cell.Formula;

            private set
            {
                ResetActiveType();
                SetValue(value);
            }
        }


        public EpplusExcelCellHolder(ExcelRange cell)
        {
            _cell = cell.ThrowIfNull(nameof(cell));
            ActiveType = cell.TransformCellType();
        }

        public void SetValue(string value)
        {
            value.ThrowIfNull(nameof(value));

            _cell.Value = value;
            ActiveType = ActiveExcelCellType.String;
        }

        public void SetValue(double value)
        {
            _cell.Value = value;
            ActiveType = ActiveExcelCellType.Numeric;
        }

        public void SetValue(bool value)
        {
            _cell.Value = value;
            ActiveType = ActiveExcelCellType.Boolean;
        }

        public void SetValue(DateTime value)
        {
            _cell.Value = value;
            ActiveType = ActiveExcelCellType.DateTime;
        }

        public void SetErrorValue(byte value)
        {
            _cell.Value = value;
            ActiveType = ActiveExcelCellType.Error;
        }

        public void SetFormula(string formula)
        {
            formula.ThrowIfNull(nameof(formula));

            _cell.Formula = formula;
            ActiveType = ActiveExcelCellType.Formula;
        }

        public IExcelCellValueHolder Evaluate()
        {
            _cell.Calculate();
            return EpplusExcelCellValueHolder.CreateFrom(_cell);
        }

        private void ResetActiveType()
        {
            ActiveType = ActiveExcelCellType.Unknown;
        }
    }
}

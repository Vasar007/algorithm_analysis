using System;
using Acolyte.Assertions;
using NPOI.SS.UserModel;

namespace AlgorithmAnalysis.Excel
{
    public sealed class CellValueHolder
    {
        public ActiveCellType ActiveType { get; private set; }

        private string? _stringValue;
        public string StringValue
        {
            get => _stringValue.ThrowIfNull(nameof(_stringValue));

            private set
            {
                ResetAllFields();
                SetValue(value);
            }
        }

        private double? _numericValue;
        public double NumericValue
        {
            get => _numericValue.ThrowIfNull(nameof(_numericValue));

            private set
            {
                ResetAllFields();
                SetValue(value);
            }
        }

        private bool? _booleanValue;
        public bool BooleanValue
        {
            get => _booleanValue.ThrowIfNull(nameof(_booleanValue));

            private set
            {
                ResetAllFields();
                SetValue(value);
            }
        }

        private DateTime? _dateTimeValue;
        public DateTime DateTimeValue
        {
            get => _dateTimeValue.ThrowIfNull(nameof(_dateTimeValue));

            private set
            {
                ResetAllFields();
                SetValue(value);
            }
        }

        private byte? _errorValue;
        public byte ErrorValue
        {
            get => _errorValue.ThrowIfNull(nameof(_errorValue));

            private set
            {
                ResetAllFields();
                SetValue(value);
            }
        }

        private string? _formula;
        public string Formula
        {
            get => _formula.ThrowIfNull(nameof(_formula));

            private set
            {
                ResetAllFields();
                SetValue(value);
            }
        }


        public CellValueHolder()
        {
            ResetAllFields();
        }

        public CellValueHolder(double value)
            : this()
        {
            _numericValue = value;
            ActiveType = ActiveCellType.Numeric;
        }

        public CellValueHolder(bool value)
            : this()
        {
            _booleanValue = value;
            ActiveType = ActiveCellType.Boolean;
        }

        public CellValueHolder(DateTime value)
            : this()
        {
            _dateTimeValue = value;
            ActiveType = ActiveCellType.DateTime;
        }

        public CellValueHolder(byte error)
            : this()
        {
            _errorValue = error;
            ActiveType = ActiveCellType.Error;
        }

        public CellValueHolder(string stringValue, bool isFormula)
            : this()
        {
            stringValue.ThrowIfNull(nameof(stringValue));

            if (isFormula)
            {
                _formula = stringValue;
                ActiveType = ActiveCellType.Formula;
            }
            else
            {
                _stringValue = stringValue;
                ActiveType = ActiveCellType.String;
            }
            
        }

        internal static CellValueHolder CreateFrom(CellValue cellValue)
        {
            cellValue.ThrowIfNull(nameof(cellValue));

            return cellValue.CellType switch
            {
                CellType.String => new CellValueHolder(cellValue.StringValue, isFormula: false),

                CellType.Numeric => new CellValueHolder(cellValue.NumberValue),

                CellType.Boolean => new CellValueHolder(cellValue.BooleanValue),

                CellType.Error => new CellValueHolder(cellValue.ErrorValue),

                CellType.Formula => new CellValueHolder(cellValue.StringValue, isFormula: true),

                _ => throw new ArgumentOutOfRangeException(
                        nameof(cellValue), cellValue.CellType,
                        $"Unknown cell type: '{cellValue.CellType.ToString()}'."
                     )
            };
        }

        public void SetValue(string value)
        {
            _stringValue = value.ThrowIfNull(nameof(value));
            ActiveType = ActiveCellType.String;
        }

        public void SetValue(double value)
        {
            _numericValue = value;
            ActiveType = ActiveCellType.Numeric;
        }

        public void SetValue(bool value)
        {
            _booleanValue = value;
            ActiveType = ActiveCellType.Boolean;
        }

        public void SetValue(DateTime value)
        {
            _dateTimeValue = value;
            ActiveType = ActiveCellType.DateTime;
        }

        public void SetErrorValue(byte value)
        {
            _errorValue = value;
            ActiveType = ActiveCellType.Error;
        }

        public void SetFormula(string formula)
        {
            _formula = formula.ThrowIfNull(nameof(formula));
            ActiveType = ActiveCellType.Formula;
        }

        private void ResetAllFields()
        {
            ActiveType = ActiveCellType.Unknown;

            _stringValue = null;
            _numericValue = null;
            _booleanValue = null;
            _dateTimeValue = null;
            _errorValue = null;
            _formula = null;
        }
    }
}

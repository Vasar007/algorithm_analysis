using System;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Excel
{
    internal abstract class BaseExcelCellValueHolder : IExcelCellValueHolder
    {
        #region IExcelCellValueHolder Implementation

        public ActiveExcelCellType ActiveType { get; }

        private readonly string? _stringValue;
        public string StringValue => _stringValue.ThrowIfNull(nameof(_stringValue));

        private readonly double? _numericValue;
        public double NumericValue => _numericValue.ThrowIfNull(nameof(_numericValue));

        private readonly bool? _booleanValue;
        public bool BooleanValue => _booleanValue.ThrowIfNull(nameof(_booleanValue));

        private readonly DateTime? _dateTimeValue;
        public DateTime DateTimeValue => _dateTimeValue.ThrowIfNull(nameof(_dateTimeValue));

        private readonly byte? _errorValue;
        public byte ErrorValue => _errorValue.ThrowIfNull(nameof(_errorValue));

        private readonly string? _formula;
        public string Formula => _formula.ThrowIfNull(nameof(_formula));

        #endregion


        protected BaseExcelCellValueHolder()
        {
            ActiveType = ActiveExcelCellType.Unknown;
        }

        protected BaseExcelCellValueHolder(double value)
            : this()
        {
            _numericValue = value;
            ActiveType = ActiveExcelCellType.Numeric;
        }

        protected BaseExcelCellValueHolder(bool value)
            : this()
        {
            _booleanValue = value;
            ActiveType = ActiveExcelCellType.Boolean;
        }

        protected BaseExcelCellValueHolder(DateTime value)
            : this()
        {
            _dateTimeValue = value;
            ActiveType = ActiveExcelCellType.DateTime;
        }

        protected BaseExcelCellValueHolder(byte error)
            : this()
        {
            _errorValue = error;
            ActiveType = ActiveExcelCellType.Error;
        }

        protected BaseExcelCellValueHolder(string stringValue, bool isFormula)
            : this()
        {
            stringValue.ThrowIfNull(nameof(stringValue));

            if (isFormula)
            {
                _formula = stringValue;
                ActiveType = ActiveExcelCellType.Formula;
            }
            else
            {
                _stringValue = stringValue;
                ActiveType = ActiveExcelCellType.String;
            }
        }
    }
}

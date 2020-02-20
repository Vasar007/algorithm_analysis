using System;
using Acolyte.Assertions;
using NPOI.SS.UserModel;

namespace AlgorithmAnalysis.Excel.NPOI
{
    internal sealed class NpoiExcelCellValueHolder : IExcelCellValueHolder
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


        private NpoiExcelCellValueHolder()
        {
            ActiveType = ActiveExcelCellType.Unknown;
        }

        private NpoiExcelCellValueHolder(double value)
        {
            _numericValue = value;
            ActiveType = ActiveExcelCellType.Numeric;
        }

        private NpoiExcelCellValueHolder(bool value)
        {
            _booleanValue = value;
            ActiveType = ActiveExcelCellType.Boolean;
        }

        private NpoiExcelCellValueHolder(DateTime value)
        {
            _dateTimeValue = value;
            ActiveType = ActiveExcelCellType.DateTime;
        }

        private NpoiExcelCellValueHolder(byte error)
        {
            _errorValue = error;
            ActiveType = ActiveExcelCellType.Error;
        }

        private NpoiExcelCellValueHolder(string stringValue, bool isFormula)
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

        internal static NpoiExcelCellValueHolder CreateFrom(CellValue cellValue)
        {
            cellValue.ThrowIfNull(nameof(cellValue));

            return cellValue.CellType switch
            {
                CellType.String => new NpoiExcelCellValueHolder(cellValue.StringValue, isFormula: false),

                CellType.Numeric => new NpoiExcelCellValueHolder(cellValue.NumberValue),

                CellType.Boolean => new NpoiExcelCellValueHolder(cellValue.BooleanValue),

                CellType.Error => new NpoiExcelCellValueHolder(cellValue.ErrorValue),

                CellType.Formula => new NpoiExcelCellValueHolder(cellValue.StringValue, isFormula: true),

                _ => throw new ArgumentOutOfRangeException(
                        nameof(cellValue), cellValue.CellType,
                        $"Unknown cell type: '{cellValue.CellType.ToString()}'."
                     )
            };
        }
    }
}

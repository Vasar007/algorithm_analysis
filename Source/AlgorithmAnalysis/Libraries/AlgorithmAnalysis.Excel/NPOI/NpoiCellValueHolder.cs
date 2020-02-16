using System;
using Acolyte.Assertions;
using NPOI.SS.UserModel;

namespace AlgorithmAnalysis.Excel.NPOI
{
    internal sealed class NpoiCellValueHolder : ICellValueHolder
    {
        public ActiveCellType ActiveType { get; }

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


        public NpoiCellValueHolder()
        {
            ActiveType = ActiveCellType.Unknown;
        }

        public NpoiCellValueHolder(double value)
            : this()
        {
            _numericValue = value;
            ActiveType = ActiveCellType.Numeric;
        }

        public NpoiCellValueHolder(bool value)
            : this()
        {
            _booleanValue = value;
            ActiveType = ActiveCellType.Boolean;
        }

        public NpoiCellValueHolder(DateTime value)
            : this()
        {
            _dateTimeValue = value;
            ActiveType = ActiveCellType.DateTime;
        }

        public NpoiCellValueHolder(byte error)
            : this()
        {
            _errorValue = error;
            ActiveType = ActiveCellType.Error;
        }

        public NpoiCellValueHolder(string stringValue, bool isFormula)
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

        internal static NpoiCellValueHolder CreateFrom(CellValue cellValue)
        {
            cellValue.ThrowIfNull(nameof(cellValue));

            return cellValue.CellType switch
            {
                CellType.String => new NpoiCellValueHolder(cellValue.StringValue, isFormula: false),

                CellType.Numeric => new NpoiCellValueHolder(cellValue.NumberValue),

                CellType.Boolean => new NpoiCellValueHolder(cellValue.BooleanValue),

                CellType.Error => new NpoiCellValueHolder(cellValue.ErrorValue),

                CellType.Formula => new NpoiCellValueHolder(cellValue.StringValue, isFormula: true),

                _ => throw new ArgumentOutOfRangeException(
                        nameof(cellValue), cellValue.CellType,
                        $"Unknown cell type: '{cellValue.CellType.ToString()}'."
                     )
            };
        }
    }
}

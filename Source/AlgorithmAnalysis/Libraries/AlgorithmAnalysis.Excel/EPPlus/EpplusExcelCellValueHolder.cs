using System;
using Acolyte.Assertions;
using OfficeOpenXml;

namespace AlgorithmAnalysis.Excel.EPPlus
{
    internal sealed class EpplusExcelCellValueHolder : IExcelCellValueHolder
    {
        private readonly ExcelRange _cell;

        public ActiveExcelCellType ActiveType { get; }

        public string StringValue => _cell.GetValue<string>();

        public double NumericValue => _cell.GetValue<double>();

        public bool BooleanValue => _cell.GetValue<bool>();

        public DateTime DateTimeValue => _cell.GetValue<DateTime>();

        public byte ErrorValue => _cell.GetValue<byte>();

        public string Formula => _cell.Formula;


        private EpplusExcelCellValueHolder(ExcelRange cell)
        {
            _cell = cell.ThrowIfNull(nameof(cell));
            ActiveType = cell.TransformCellType();
        }

        internal static EpplusExcelCellValueHolder CreateFrom(ExcelRange cell)
        {
            return new EpplusExcelCellValueHolder(cell);
        }
    }
}

using System;
using Acolyte.Assertions;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace AlgorithmAnalysis.Excel
{
    public static class ExcelExtensions
    {
        #region NPOI extensions

        internal static ICell Center(this ICell cell)
        {
            cell.ThrowIfNull(nameof(cell));

            ICellStyle cellStyle = cell.Row.Sheet.Workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            cell.CellStyle = cellStyle;
            return cell;
        }

        internal static IRow Center(this IRow row)
        {
            row.ThrowIfNull(nameof(row));

            ICellStyle cellStyle = row.Sheet.Workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            row.RowStyle = cellStyle;
            return row;
        }

        internal static ActiveExcelCellType TransformCellType(this CellType cellType)
        {
            cellType.ThrowIfEnumValueIsUndefined(nameof(cellType));

            // TODO: determine NPOI DateTime value in cell.
            return cellType switch
            {
                CellType.Unknown => ActiveExcelCellType.Unknown,

                CellType.Blank => ActiveExcelCellType.Unknown,

                CellType.String => ActiveExcelCellType.String,

                CellType.Numeric => ActiveExcelCellType.Numeric,

                CellType.Boolean => ActiveExcelCellType.Boolean,

                CellType.Error => ActiveExcelCellType.Error,

                CellType.Formula => ActiveExcelCellType.Formula,

                _ => throw new ArgumentOutOfRangeException(
                         nameof(cellType), cellType,
                         $"Unknown NPOI cell type: '{cellType.ToString()}'."
                     )
            };
        }

        #endregion

        #region EPPlus extensions

        internal static ExcelRange Center(this ExcelRange cell)
        {
            cell.ThrowIfNull(nameof(cell));

            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            return cell;
        }

        internal static ActiveExcelCellType TransformCellType(this ExcelRange cell)
        {
            cell.ThrowIfNull(nameof(cell));

            switch (cell.Value)
            {
                case string _: return ActiveExcelCellType.String;

                case double _: return ActiveExcelCellType.Numeric;

                case float _: return ActiveExcelCellType.Numeric;

                case int _: return ActiveExcelCellType.Numeric;

                case short _: return ActiveExcelCellType.Numeric;

                case bool _: return ActiveExcelCellType.Boolean;

                case byte _: return ActiveExcelCellType.Error;

                default:
                    if (ExcelErrorValue.Values.IsErrorValue(cell.Value)) return ActiveExcelCellType.Error;
                    if (!string.IsNullOrEmpty(cell.Formula)) return ActiveExcelCellType.Formula;
                    return ActiveExcelCellType.Unknown;
            };
        }

        #endregion

        public static int UseOneBasedIndexing(this int zeroBasedIndex)
        {
            zeroBasedIndex.ThrowIfValueIsOutOfRange(nameof(zeroBasedIndex), 0, int.MaxValue);

            return zeroBasedIndex + 1;
        }

        public static int SkipHeader(this int oneBasedIndex)
        {
            oneBasedIndex.ThrowIfValueIsOutOfRange(nameof(oneBasedIndex), 1, int.MaxValue);

            return oneBasedIndex + 1;
        }
    }
}

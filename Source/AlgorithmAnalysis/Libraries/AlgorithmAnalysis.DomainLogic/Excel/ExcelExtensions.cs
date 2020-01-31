using Acolyte.Assertions;
using NPOI.SS.UserModel;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal static class ExcelExtensions
    {
        public static ICell Center(this ICell cell)
        {
            cell.ThrowIfNull(nameof(cell));

            ICellStyle cellStyle = cell.Row.Sheet.Workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            cell.CellStyle = cellStyle;
            return cell;
        }

        public static IRow Center(this IRow row)
        {
            row.ThrowIfNull(nameof(row));

            ICellStyle cellStyle = row.Sheet.Workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            row.RowStyle = cellStyle;

            return row;
        }
    }
}

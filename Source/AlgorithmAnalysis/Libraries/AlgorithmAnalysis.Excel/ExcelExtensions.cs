using Acolyte.Assertions;
using NPOI.SS.UserModel;

namespace AlgorithmAnalysis.Excel
{
    public static class ExcelExtensions
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

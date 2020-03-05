using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Excel
{
    internal static class ExcelWrapperHelper
    {
        public static IExcelCellHolder GetCellHolder(IExcelSheet sheet,
            ExcelColumnIndex columnIndex, int rowIndex, ExcelOptions excelOptions)
        {
            sheet.ThrowIfNull(nameof(sheet));
            excelOptions.ThrowIfNull(nameof(excelOptions));

            return excelOptions.CellCreationMode switch
            {
                ExcelCellCreationMode.Default =>
                    sheet.GetOrCreateCell(columnIndex, rowIndex),

                ExcelCellCreationMode.Centerized =>
                    sheet.GetOrCreateCenterizedCell(columnIndex, rowIndex),

                _ => throw new ArgumentOutOfRangeException(
                         nameof(excelOptions), excelOptions.CellCreationMode,
                         $"Unknown cell creation mode: '{excelOptions.CellCreationMode.ToString()}'."
                     )
            };
        }

        public static string GetCellAddressFrom(ExcelColumnIndex columnIndex, int rowIndex)
        {
            columnIndex.ThrowIfEnumValueIsUndefined(nameof(columnIndex));
            rowIndex.ThrowIfValueIsOutOfRange(nameof(rowIndex), 1, int.MaxValue);

            return $"{columnIndex.ToString()}{rowIndex.ToString()}";
        }

        public static string GetRangeCellAddressFrom(
            ExcelColumnIndex firstColumnIndex, int firstRowIndex,
            ExcelColumnIndex lastColumnIndex, int lastRowIndex)
        {
            string firstCellAddress = GetCellAddressFrom(firstColumnIndex, firstRowIndex);
            string lastCellAddress = GetCellAddressFrom(lastColumnIndex, lastRowIndex);

            return $"{firstCellAddress}:{lastCellAddress}";
        }
    }
}


using Acolyte.Assertions;
using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis
{
    internal static class ManualFormulaProvider
    {
        public static string Span(string min, string max)
        {
            min.ThrowIfNullOrWhiteSpace(nameof(min));
            max.ThrowIfNullOrWhiteSpace(nameof(max));

            return $"{max} - {min}";
        }

        public static string VariationCoefficient(string mean, string deviation)
        {
            mean.ThrowIfNullOrWhiteSpace(nameof(mean));
            deviation.ThrowIfNullOrWhiteSpace(nameof(deviation));

            return $"{deviation} / {mean}";
        }

        public static string Alpha(string normalizedMean, string normalizedVarience)
        {
            normalizedMean.ThrowIfNullOrWhiteSpace(nameof(normalizedMean));
            normalizedVarience.ThrowIfNullOrWhiteSpace(nameof(normalizedVarience));

            return $"{normalizedMean} * " +
                   $"(({normalizedMean} * (1 - {normalizedMean}) / {normalizedVarience}) - 1)";
        }

        public static string Beta(string normalizedMean, string normalizedVarience)
        {
            normalizedMean.ThrowIfNullOrWhiteSpace(nameof(normalizedMean));
            normalizedVarience.ThrowIfNullOrWhiteSpace(nameof(normalizedVarience));

            return $"(1 - {normalizedMean}) * " +
                   $"(({normalizedMean} * (1 - {normalizedMean}) / {normalizedVarience}) - 1)";
        }

        public static string Normalize(string data, string min, string max)
        {
            data.ThrowIfNullOrWhiteSpace(nameof(data));
            min.ThrowIfNullOrWhiteSpace(nameof(min));
            max.ThrowIfNullOrWhiteSpace(nameof(max));

            return $"({data} - {min}) / ({max} - {min})";
        }

        // TODO: use interface instead of this method.
        internal static string Min(IExcelSheet sheet, ExcelColumnIndex columnIndex, int rowIndex)
        {
            sheet.ThrowIfNull(nameof(sheet));
            rowIndex.ThrowIfValueIsOutOfRange(nameof(rowIndex), 1, int.MaxValue);

            string cell = sheet[columnIndex, rowIndex].Address;
            return cell;
        }

        // TODO: use interface instead of this method.
        internal static string Average(IExcelSheet sheet, ExcelColumnIndex columnIndex,
            int rowIndex)
        {
            sheet.ThrowIfNull(nameof(sheet));
            rowIndex.ThrowIfValueIsOutOfRange(nameof(rowIndex), 1, int.MaxValue);

            string cell = sheet[columnIndex, rowIndex].Address;
            return $"{cell} * {cell} * ({cell} - 1) / 2";
        }

        // TODO: use interface instead of this method.
        internal static string Max(IExcelSheet sheet, ExcelColumnIndex columnIndex, int rowIndex)
        {
            sheet.ThrowIfNull(nameof(sheet));
            rowIndex.ThrowIfValueIsOutOfRange(nameof(rowIndex), 1, int.MaxValue);

            string cell = sheet[columnIndex, rowIndex].Address;
            return $"{cell} * {cell} * {cell} * ({cell} - 1) / 2";
        }
    }
}

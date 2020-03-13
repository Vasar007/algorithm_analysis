
using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.Excel;
using AlgorithmAnalysis.Excel.Formulas;
using AlgorithmAnalysis.Math.Functions;

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

        public static string ConfidenceComplexity(string leftYQuantile, string min, string max)
        {
            leftYQuantile.ThrowIfNullOrWhiteSpace(nameof(leftYQuantile));
            min.ThrowIfNullOrWhiteSpace(nameof(min));
            max.ThrowIfNullOrWhiteSpace(nameof(max));

            return $"{min} + {leftYQuantile} * ({max} - {min})";
        }

        public static string GetFormulaForFunction(IExcelFormulaProvider formulaProvider,
            FunctionType functionType)
        {
            formulaProvider.ThrowIfNull(nameof(formulaProvider));

            return functionType switch
            {
                FunctionType.Exponential => formulaProvider.GetFormulaByName(nameof(IExcelFormulaProvider.Exp)),

                FunctionType.Line => string.Empty,

                FunctionType.Logarithm => formulaProvider.GetFormulaByName(nameof(IExcelFormulaProvider.Ln)),

                FunctionType.Polynomial => string.Empty,

                FunctionType.Power => string.Empty,

                _ => throw new ArgumentOutOfRangeException(
                         nameof(functionType), functionType,
                         $"Unknown function type: '{functionType.ToString()}'."
                     )
            };
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

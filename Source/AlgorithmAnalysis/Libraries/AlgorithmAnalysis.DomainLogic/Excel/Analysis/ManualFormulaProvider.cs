using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.Excel;
using AlgorithmAnalysis.Excel.Formulas;
using AlgorithmAnalysis.Math.Functions;
using AlgorithmAnalysis.Models;

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

        public static string Chi2Single(string relativeFrequency, string theoreticalFrequency)
        {
            relativeFrequency.ThrowIfNullOrWhiteSpace(nameof(relativeFrequency));
            theoreticalFrequency.ThrowIfNullOrWhiteSpace(nameof(theoreticalFrequency));

            return $"({relativeFrequency} - {theoreticalFrequency})^2 / {theoreticalFrequency}";
        }

        public static string Chi2(IExcelFormulaProvider formulaProvider, string chi2Range,
            string experimentsNumber)
        {
            formulaProvider.ThrowIfNull(nameof(formulaProvider));
            chi2Range.ThrowIfNullOrWhiteSpace(nameof(chi2Range));
            experimentsNumber.ThrowIfNullOrWhiteSpace(nameof(experimentsNumber));

            return $"{formulaProvider.Sum(chi2Range)} * {experimentsNumber}";
        }

        public static string ScottFormula(IExcelFormulaProvider formulaProvider,
            string normalizedValueRange, string experimentsNumber)
        {
            formulaProvider.ThrowIfNull(nameof(formulaProvider));
            normalizedValueRange.ThrowIfNullOrWhiteSpace(nameof(normalizedValueRange));
            experimentsNumber.ThrowIfNullOrWhiteSpace(nameof(experimentsNumber));

            return $"(3.5 * {formulaProvider.StdDev(normalizedValueRange)}) / ({experimentsNumber}^(1/3))";
        }

        public static string GetFormulaForFunction(IExcelFormulaProvider formulaProvider,
            FunctionType functionType)
        {
            formulaProvider.ThrowIfNull(nameof(formulaProvider));

            return functionType switch
            {
                FunctionType.Exponential =>
                    formulaProvider.GetFormulaByName(nameof(IExcelFormulaProvider.Exp)),

                FunctionType.Line => string.Empty,

                FunctionType.Logarithm =>
                    formulaProvider.GetFormulaByName(nameof(IExcelFormulaProvider.Ln)),

                FunctionType.Polynomial => string.Empty,

                FunctionType.Power => string.Empty,

                _ => throw new ArgumentOutOfRangeException(
                         nameof(functionType), functionType,
                         $"Unknown function type: '{functionType.ToString()}'."
                     )
            };
        }

        public static string Min(IExcelSheet sheet, ExcelColumnIndex columnIndex, int rowIndex,
            AlgorithmType algorithmType)
        {
            algorithmType.ThrowIfNull(nameof(algorithmType));

            return FormatFormula(sheet, columnIndex, rowIndex, algorithmType.MinFormulaFormat);
        }

        public static string Average(IExcelSheet sheet, ExcelColumnIndex columnIndex,
            int rowIndex, AlgorithmType algorithmType)
        {
            algorithmType.ThrowIfNull(nameof(algorithmType));

            return FormatFormula(sheet, columnIndex, rowIndex, algorithmType.AverageFormulaFormat);
        }

        public static string Max(IExcelSheet sheet, ExcelColumnIndex columnIndex, int rowIndex,
            AlgorithmType algorithmType)
        {
            algorithmType.ThrowIfNull(nameof(algorithmType));

            return FormatFormula(sheet, columnIndex, rowIndex, algorithmType.MaxFormulaFormat);
        }

        private static string FormatFormula(IExcelSheet sheet, ExcelColumnIndex columnIndex,
            int rowIndex, string formatString)
        {
            sheet.ThrowIfNull(nameof(sheet));
            rowIndex.ThrowIfValueIsOutOfRange(nameof(rowIndex), 1, int.MaxValue);
            formatString.ThrowIfNullOrWhiteSpace(nameof(formatString));

            string cell = sheet[columnIndex, rowIndex].Address;
            return string.Format(formatString, cell);
        }
    }
}

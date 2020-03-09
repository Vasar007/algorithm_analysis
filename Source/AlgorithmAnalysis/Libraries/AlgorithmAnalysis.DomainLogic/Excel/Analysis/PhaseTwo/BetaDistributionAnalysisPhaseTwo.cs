using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.DomainLogic.Properties;
using AlgorithmAnalysis.Excel;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseTwo
{
    internal sealed class BetaDistributionAnalysisPhaseTwo : IAnalysisPhaseTwo
    {
        private readonly ParametersPack _args;

        private readonly int _iterationsNumber;

        private readonly ExcelColumnIndex _theoreticalMinColumn;

        private readonly ExcelColumnIndex _theoreticalMaxColumn;


        public BetaDistributionAnalysisPhaseTwo(ParametersPack args)
        {
            _args = args.ThrowIfNull(nameof(args));

            _iterationsNumber = args.GetIterationsNumber(phaseNumber: 2);
            _theoreticalMinColumn = GetTheoreticalMinColumn();
            _theoreticalMaxColumn = GetTheoreticalMaxColumn();
        }

        #region IAnalysisPhaseTwo Implementation

        public void ApplyAnalysisToSingleLaunch(IExcelSheet sheet, ExcelColumnIndex currentColumn,
            int currentRow, int operationNumber)
        {
            string dataAddress = sheet[currentColumn, currentRow].Address;

            int theoreticalDataRow = currentColumn.AsInt32().UseOneBasedIndexing().SkipHeader();
            string theoreticalMinAddress = sheet[_theoreticalMinColumn, theoreticalDataRow].Address;
            string theoreticalMaxAddress = sheet[_theoreticalMaxColumn, theoreticalDataRow].Address;

            string normalizedFormula = ManualFormulaProvider.Normalize(
                dataAddress, theoreticalMinAddress, theoreticalMaxAddress
            );

            int normalizedDataRowIndex = ExcelWrapperForPhaseTwo.GetNormalizedDataRowIndex(
                _args, currentRow
            );
            sheet[currentColumn, normalizedDataRowIndex].SetFormula(normalizedFormula);
        }

        public void ApplyAnalysisToDataset(IExcelSheet sheet, int currentColumnIndex)
        {
            FillData(sheet, ref currentColumnIndex);
        }

        #endregion

        private ExcelColumnIndex GetTheoreticalMinColumn()
        {
            const int columnsBetweenDataAndTheoreticalMin = 2;

            int columnIndex = _iterationsNumber + columnsBetweenDataAndTheoreticalMin;
            return columnIndex.AsEnum<ExcelColumnIndex>();
        }

        private ExcelColumnIndex GetTheoreticalMaxColumn()
        {
            int minColumn = GetTheoreticalMinColumn().AsInt32();
            return minColumn.AsEnum<ExcelColumnIndex>(offset: 2);
        }

        private void FillData(IExcelSheet sheet, ref int currentColumnIndex)
        {
            FillAnalysisColumns(sheet, ref currentColumnIndex);

            // Remain blank column.
            ++currentColumnIndex;

            var nColumnNameColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[nColumnNameColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.NColumnName);
            sheet.AutoSizeColumn(nColumnNameColumnIndex);

            var alphaNColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[alphaNColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.AlphaN);
            sheet.AutoSizeColumn(alphaNColumnIndex);

            var betaNColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[betaNColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.BetaN);
            sheet.AutoSizeColumn(betaNColumnIndex);

            // Remain blank column.
            ++currentColumnIndex;

            var leftYQuantileColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[leftYQuantileColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.LeftYQuantile);
            sheet.AutoSizeColumn(leftYQuantileColumnIndex);

            var complexityFunctionColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[complexityFunctionColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.ComplexityFunction);
            sheet.AutoSizeColumn(complexityFunctionColumnIndex);

            var comparisonColumnIndex = currentColumnIndex.AsEnum<ExcelColumnIndex>();
            sheet[comparisonColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.Comparison);
            sheet.AutoSizeColumn(comparisonColumnIndex);
        }

        private void FillAnalysisColumns(IExcelSheet sheet, ref int currentColumnIndex)
        {
            int rowIndex = 1;

            var sampleMeanColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[sampleMeanColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.SampleMean);

            var sampleVarianceColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[sampleVarianceColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.SampleVariance);

            var sampleDeviationColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[sampleDeviationColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.SampleDeviation);

            var normalizedMeanColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[normalizedMeanColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.NormalizedMean);

            var normalizedVarienceColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[normalizedVarienceColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.NormalizedVarience);

            var alphaColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[alphaColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.Alpha);

            var betaColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[betaColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.Beta);

            ++rowIndex;
            int launchesColumnIndex = 0;
            int firstRowIndex = ExcelWrapperForPhaseTwo.GetFirstDataRowIndex();
            int lastRowIndex = ExcelWrapperForPhaseTwo.GetLastDataRowIndex(_args);
            int firstNormalizedRowIndex = ExcelWrapperForPhaseTwo.GetFirstNormalizedDataRowIndex(_args);
            int lastNormalizedRowIndex = ExcelWrapperForPhaseTwo.GetLastNormalizedDataRowIndex(_args);

            for (int launchesNumber = _args.StartValue; launchesNumber <= _args.EndValue;
                 launchesNumber += _args.Step)
            {
                var launchesColumnIndexEnum = launchesColumnIndex++.AsEnum<ExcelColumnIndex>();

                string dataRange =
                    $"{sheet[launchesColumnIndexEnum, firstRowIndex].Address}:" +
                    $"{sheet[launchesColumnIndexEnum, lastRowIndex].Address}";

                string sampleMeanFormula = sheet.FormulaProvider.Average(dataRange);
                sheet[sampleMeanColumnIndex, rowIndex].SetFormula(sampleMeanFormula);

                string sampleVarienceFormula = sheet.FormulaProvider.Var(dataRange);
                sheet[sampleVarianceColumnIndex, rowIndex].SetFormula(sampleVarienceFormula);

                string sampleDeviationFormula = sheet.FormulaProvider.StdDev(dataRange);
                sheet[sampleDeviationColumnIndex, rowIndex].SetFormula(sampleDeviationFormula);

                string normalizedDataRange =
                    $"{sheet[launchesColumnIndexEnum, firstNormalizedRowIndex].Address}:" +
                    $"{sheet[launchesColumnIndexEnum, lastNormalizedRowIndex].Address}";

                string normalizedMeanFormula = sheet.FormulaProvider.Average(normalizedDataRange);
                sheet[normalizedMeanColumnIndex, rowIndex].SetFormula(normalizedMeanFormula);

                string normalizedVarienceFormula = sheet.FormulaProvider.Var(normalizedDataRange);
                sheet[normalizedVarienceColumnIndex, rowIndex].SetFormula(normalizedVarienceFormula);

                string normalizedMeanAddress = sheet[normalizedMeanColumnIndex, rowIndex].Address;
                string normalizedVarienceAddress = sheet[normalizedVarienceColumnIndex, rowIndex].Address;

                string alphaFormula = ManualFormulaProvider.Alpha(normalizedMeanAddress, normalizedVarienceAddress);
                sheet[alphaColumnIndex, rowIndex].SetFormula(alphaFormula);

                string betaFormula = ManualFormulaProvider.Beta(normalizedMeanAddress, normalizedVarienceAddress);
                sheet[betaColumnIndex, rowIndex].SetFormula(betaFormula);

                ++rowIndex;
            }

            sheet.AutoSizeColumn(sampleMeanColumnIndex);
            sheet.AutoSizeColumn(sampleVarianceColumnIndex);
            sheet.AutoSizeColumn(sampleDeviationColumnIndex);
            sheet.AutoSizeColumn(normalizedMeanColumnIndex);
            sheet.AutoSizeColumn(normalizedVarienceColumnIndex);
            sheet.AutoSizeColumn(alphaColumnIndex);
            sheet.AutoSizeColumn(betaColumnIndex);
        }
    }
}

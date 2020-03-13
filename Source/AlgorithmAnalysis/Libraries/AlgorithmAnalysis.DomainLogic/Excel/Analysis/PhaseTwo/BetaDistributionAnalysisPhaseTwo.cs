using System.Collections.Generic;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.DomainLogic.Properties;
using AlgorithmAnalysis.Excel;
using AlgorithmAnalysis.Math;
using AlgorithmAnalysis.Math.Functions;

namespace AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseTwo
{
    internal sealed class BetaDistributionAnalysisPhaseTwo : IAnalysisPhaseTwo
    {
        private readonly ParametersPack _args;

        private readonly IRegression _regression;

        private readonly int _iterationsNumber;

        private readonly ExcelColumnIndex _additionalDataColumn;

        private readonly ExcelColumnIndex _sampleSizeColumnIndex;

        private readonly ExcelColumnIndex _theoreticalMinColumn;

        private readonly ExcelColumnIndex _theoreticalMaxColumn;

        private readonly ExcelColumnIndex _alphaColumn;

        private readonly ExcelColumnIndex _betaColumn;


        public BetaDistributionAnalysisPhaseTwo(ParametersPack args, IRegression regression)
        {
            _args = args.ThrowIfNull(nameof(args));
            _regression = regression.ThrowIfNull(nameof(regression));

            _iterationsNumber = args.GetIterationsNumber(phaseNumber: 2);
            _additionalDataColumn = ExcelWrapperForPhaseTwo.GetAdditionalDataColumn(_iterationsNumber);
            _sampleSizeColumnIndex = ExcelWrapperForPhaseTwo.GetSampleSizeColumn(_iterationsNumber);
            _theoreticalMinColumn = ExcelWrapperForPhaseTwo.GetTheoreticalMinColumn(_iterationsNumber);
            _theoreticalMaxColumn = ExcelWrapperForPhaseTwo.GetTheoreticalMaxColumn(_iterationsNumber);
            _alphaColumn = GetAlphaColumn();
            _betaColumn = GetBetaColumn();
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
            FillStatisticalColumns(sheet, ref currentColumnIndex);

            // Remain blank column.
            ++currentColumnIndex;

            FillAnalysisColumns(sheet, ref currentColumnIndex);
        }

        #endregion

        private ExcelColumnIndex GetAlphaColumn()
        {
            const int columnsBetweenDataAndAlpha = 10;

            int columnIndex = _iterationsNumber + columnsBetweenDataAndAlpha;
            return columnIndex.AsEnum<ExcelColumnIndex>();
        }

        private ExcelColumnIndex GetBetaColumn()
        {
            const int columnsBetweenAlphaAndBeta = 0;

            int alphaColumn = GetAlphaColumn().AsInt32().UseOneBasedIndexing();
            return alphaColumn.AsEnum<ExcelColumnIndex>(columnsBetweenAlphaAndBeta);
        }

        private IExcelCellHolder GetConfidenceFactorCell(IExcelSheet sheet)
        {
            const int confidenceFactorRowIndex = 4;

            return sheet[_additionalDataColumn, confidenceFactorRowIndex];
        }

        private IModelledFunction GetOptimalFunction(IExcelSheet sheet,
            ExcelColumnIndex yColumnIndex)
        {
            var xValues = new List<double>(_iterationsNumber);
            var yValues = new List<double>(_iterationsNumber);

            int lastValueIndex = _iterationsNumber.SkipHeader();
            for (int rowIndex = 1.SkipHeader(); rowIndex <= lastValueIndex; ++rowIndex)
            {
                IExcelCellHolder sampleSizeCell = sheet[_sampleSizeColumnIndex, rowIndex];
                xValues.Add(sampleSizeCell.NumericValue);

                IExcelCellValueHolder yValueCell = sheet.EvaluateCell(yColumnIndex, rowIndex);
                yValues.Add(yValueCell.NumericValue);
            }

            return _regression.Fit(xValues, yValues);
        }

        private void FillStatisticalColumns(IExcelSheet sheet, ref int currentColumnIndex)
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

                string alphaFormula = ManualFormulaProvider.Alpha(
                    normalizedMeanAddress, normalizedVarienceAddress
                );
                sheet[alphaColumnIndex, rowIndex].SetFormula(alphaFormula);

                string betaFormula = ManualFormulaProvider.Beta(
                    normalizedMeanAddress, normalizedVarienceAddress
                );
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

        private void FillAnalysisColumns(IExcelSheet sheet, ref int currentColumnIndex)
        {
            int rowIndex = 1;

            var nColumnColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[nColumnColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.NColumnName);

            var alphaNColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[alphaNColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.AlphaN);

            var betaNColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[betaNColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.BetaN);

            // Remain blank column.
            ++currentColumnIndex;

            var leftYQuantileColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[leftYQuantileColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.LeftYQuantile);

            var complexityFunctionColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[complexityFunctionColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.ComplexityFunction);

            var comparisonColumnIndex = currentColumnIndex.AsEnum<ExcelColumnIndex>();
            sheet[comparisonColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.Comparison);

            IExcelCellHolder confidenceFactorCell = GetConfidenceFactorCell(sheet);

            IModelledFunction alphaOptimalFunction = GetOptimalFunction(sheet, _alphaColumn);
            string formulaForAlphaFunction = ManualFormulaProvider.GetFormulaForFunction(
                sheet.FormulaProvider, alphaOptimalFunction.Type
            );

            IModelledFunction betaOptimalFunction = GetOptimalFunction(sheet, _betaColumn);
            string formulaForBetaFunction = ManualFormulaProvider.GetFormulaForFunction(
                sheet.FormulaProvider, betaOptimalFunction.Type
            );

            ++rowIndex;

            for (int launchesNumber = _args.StartValue; launchesNumber <= _args.ExtrapolationSegmentValue;
                 launchesNumber += _args.Step)
            {
                IExcelCellHolder nColumnCell = sheet[nColumnColumnIndex, rowIndex];
                nColumnCell.SetValue(launchesNumber);

                string alphaNFormula = alphaOptimalFunction.ToFormulaString(
                    nColumnCell.Address, formulaForAlphaFunction
                );
                IExcelCellHolder alphaNCell = sheet[alphaNColumnIndex, rowIndex];
                alphaNCell.SetFormula(alphaNFormula);

                string betaNFormula = betaOptimalFunction.ToFormulaString(
                    nColumnCell.Address, formulaForBetaFunction
                );
                IExcelCellHolder betaNCell = sheet[betaNColumnIndex, rowIndex];
                betaNCell.SetFormula(betaNFormula);

                string leftYQuantileFormula = sheet.FormulaProvider.BetaInv(
                    confidenceFactorCell.Address, alphaNCell.Address, betaNCell.Address
                );
                IExcelCellHolder leftYQuantileCell = sheet[leftYQuantileColumnIndex, rowIndex];
                leftYQuantileCell.SetFormula(leftYQuantileFormula);

                string theoreticalMinAddress = sheet[_theoreticalMinColumn, rowIndex].Address;
                string theoreticalMaxAddress = sheet[_theoreticalMaxColumn, rowIndex].Address;

                string complexityFunctionFormula = ManualFormulaProvider.ConfidenceComplexity(
                    leftYQuantileCell.Address, theoreticalMinAddress, theoreticalMaxAddress
                );
                IExcelCellHolder complexityCell = sheet[complexityFunctionColumnIndex, rowIndex];
                complexityCell.SetFormula(complexityFunctionFormula);

                string comparisonFormula = $"{theoreticalMaxAddress} / {complexityCell.Address}";
                sheet[comparisonColumnIndex, rowIndex].SetFormula(comparisonFormula);

                ++rowIndex;
            }

            sheet.AutoSizeColumn(nColumnColumnIndex);
            sheet.AutoSizeColumn(alphaNColumnIndex);
            sheet.AutoSizeColumn(betaNColumnIndex);
            sheet.AutoSizeColumn(leftYQuantileColumnIndex);
            sheet.AutoSizeColumn(complexityFunctionColumnIndex);
            sheet.AutoSizeColumn(comparisonColumnIndex);
        }
    }
}

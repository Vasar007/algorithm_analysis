﻿using System.Collections.Generic;
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

        private readonly int _lastSegmentValueRowIndex;

        private readonly ExcelColumnIndex _additionalDataColumn;

        private readonly ExcelColumnIndex _sampleSizeColumnIndex;

        private readonly ExcelColumnIndex _theoreticalMinColumn;

        private readonly ExcelColumnIndex _theoreticalMaxColumn;

        private readonly ExcelColumnIndex _normalizedMeanColumn;

        private readonly ExcelColumnIndex _normalizedVarianceColumn;


        public BetaDistributionAnalysisPhaseTwo(ParametersPack args, IRegression regression)
        {
            _args = args.ThrowIfNull(nameof(args));
            _regression = regression.ThrowIfNull(nameof(regression));

            _iterationsNumber = args.GetIterationsNumber(phaseNumber: 2);
            _lastSegmentValueRowIndex = _iterationsNumber.SkipHeader();
            _additionalDataColumn = ExcelWrapperForPhaseTwo.GetAdditionalDataColumn(_iterationsNumber);
            _sampleSizeColumnIndex = ExcelWrapperForPhaseTwo.GetSampleSizeColumn(_iterationsNumber);
            _theoreticalMinColumn = ExcelWrapperForPhaseTwo.GetTheoreticalMinColumn(_iterationsNumber);
            _theoreticalMaxColumn = ExcelWrapperForPhaseTwo.GetTheoreticalMaxColumn(_iterationsNumber);
            _normalizedMeanColumn = GetNormalizedMeanColumn();
            _normalizedVarianceColumn = GetNormalizedVarianceColumn();
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

        private ExcelColumnIndex GetNormalizedMeanColumn()
        {
            const int columnsBetweenDataAndNormalizedMean = 8;

            int columnIndex = _iterationsNumber + columnsBetweenDataAndNormalizedMean;
            return columnIndex.AsEnum<ExcelColumnIndex>();
        }

        private ExcelColumnIndex GetNormalizedVarianceColumn()
        {
            const int columnsBetweenNormalozedMeanAndVariance = 0;

            int alphaColumn = GetNormalizedMeanColumn().AsInt32().UseOneBasedIndexing();
            return alphaColumn.AsEnum<ExcelColumnIndex>(columnsBetweenNormalozedMeanAndVariance);
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

            for (int rowIndex = 1.SkipHeader(); rowIndex <= _lastSegmentValueRowIndex; ++rowIndex)
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
            sheet[sampleMeanColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.SampleMeanColumnName);

            var sampleVarianceColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[sampleVarianceColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.SampleVarianceColumnName);

            var sampleDeviationColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[sampleDeviationColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.SampleDeviationColumnName);

            var normalizedMeanColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[normalizedMeanColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.NormalizedMeanColumnName);

            var normalizedVarianceColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[normalizedVarianceColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.NormalizedVarianceColumnName);

            var alphaColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[alphaColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.AlphaColumnName);

            var betaColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[betaColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.BetaColumnName);

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

                string normalizedVarianceFormula = sheet.FormulaProvider.Var(normalizedDataRange);
                sheet[normalizedVarianceColumnIndex, rowIndex].SetFormula(normalizedVarianceFormula);

                string normalizedMeanAddress = sheet[normalizedMeanColumnIndex, rowIndex].Address;
                string normalizedVarienceAddress = sheet[normalizedVarianceColumnIndex, rowIndex].Address;

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
            sheet.AutoSizeColumn(normalizedVarianceColumnIndex);
            sheet.AutoSizeColumn(alphaColumnIndex);
            sheet.AutoSizeColumn(betaColumnIndex);
        }

        private void FillAnalysisColumns(IExcelSheet sheet, ref int currentColumnIndex)
        {
            int rowIndex = 1;

            var nColumnColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[nColumnColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.NColumnName);

            var normalizedMeanNColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[normalizedMeanNColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.NormalizedNMeanColumnName);

            var normalizedVarianceNColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[normalizedVarianceNColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.NormalizedNVarianceColumnName);

            var alphaNColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[alphaNColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.AlphaNColumnName);

            var betaNColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[betaNColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.BetaNColumnName);
            
            var analysisValuesColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[analysisValuesColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.AnalysisValuesColumnName);

            // Remain blank column.
            ++currentColumnIndex;

            var leftYQuantileColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[leftYQuantileColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.LeftYQuantileColumnName);

            var complexityFunctionColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[complexityFunctionColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.ComplexityFunctionColumnName);

            var comparisonColumnIndex = currentColumnIndex.AsEnum<ExcelColumnIndex>();
            sheet[comparisonColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.ComparisonColumnName);

            IExcelCellHolder confidenceFactorCell = GetConfidenceFactorCell(sheet);

            IModelledFunction normalizedMeanOptimalFunction = GetOptimalFunction(
                sheet, _normalizedMeanColumn
            );
            string formulaForAlphaFunction = ManualFormulaProvider.GetFormulaForFunction(
                sheet.FormulaProvider, normalizedMeanOptimalFunction.Type
            );

            IModelledFunction normalizedVarienceOptimalFunction = GetOptimalFunction(
                sheet, _normalizedVarianceColumn
            );
            string formulaForBetaFunction = ManualFormulaProvider.GetFormulaForFunction(
                sheet.FormulaProvider, normalizedVarienceOptimalFunction.Type
            );

            ++rowIndex;

            for (int launchesNumber = _args.StartValue; launchesNumber <= _args.ExtrapolationSegmentValue;
                 launchesNumber += _args.Step)
            {
                IExcelCellHolder nColumnCell = sheet[nColumnColumnIndex, rowIndex];
                nColumnCell.SetValue(launchesNumber);

                string normalizedMeanNFormula = normalizedMeanOptimalFunction.ToFormulaString(
                    nColumnCell.Address, formulaForAlphaFunction
                );
                IExcelCellHolder normalizedMeanNCell = sheet[normalizedMeanNColumnIndex, rowIndex];
                normalizedMeanNCell.SetFormula(normalizedMeanNFormula);

                string normalizedVarianceNFormula = normalizedVarienceOptimalFunction.ToFormulaString(
                    nColumnCell.Address, formulaForBetaFunction
                );
                IExcelCellHolder normalizedVarianceNCell = sheet[normalizedVarianceNColumnIndex, rowIndex];
                normalizedVarianceNCell.SetFormula(normalizedVarianceNFormula);

                string normalizedMeanNAddress = sheet[normalizedMeanNColumnIndex, rowIndex].Address;
                string normalizedVarianceNAddress = sheet[normalizedVarianceNColumnIndex, rowIndex].Address;

                string alphaNFormula = ManualFormulaProvider.Alpha(
                    normalizedMeanNAddress, normalizedVarianceNAddress
                );
                IExcelCellHolder alphaNCell = sheet[alphaNColumnIndex, rowIndex];
                alphaNCell.SetFormula(alphaNFormula);

                string betaNFormula = ManualFormulaProvider.Beta(
                    normalizedMeanNAddress, normalizedVarianceNAddress
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

            string alphaPearsonFormula = sheet.FormulaProvider.Pearson(
                $"{sheet[_normalizedMeanColumn, 2].Address}:" +
                $"{sheet[_normalizedMeanColumn, _lastSegmentValueRowIndex].Address}",
                $"{sheet[alphaNColumnIndex, 2].Address}:" +
                $"{sheet[alphaNColumnIndex, _lastSegmentValueRowIndex].Address}"
            );
            sheet[analysisValuesColumnIndex, 2].SetFormula(alphaPearsonFormula);

            string betaPearsonFormula = sheet.FormulaProvider.Pearson(
                $"{sheet[_normalizedVarianceColumn, 2].Address}:" +
                $"{sheet[_normalizedVarianceColumn, _lastSegmentValueRowIndex].Address}",
                $"{sheet[betaNColumnIndex, 2].Address}:" +
                $"{sheet[betaNColumnIndex, _lastSegmentValueRowIndex].Address}"
            );
            sheet[analysisValuesColumnIndex, 3].SetFormula(betaPearsonFormula);

            sheet.AutoSizeColumn(nColumnColumnIndex);
            sheet.AutoSizeColumn(normalizedMeanNColumnIndex);
            sheet.AutoSizeColumn(normalizedVarianceNColumnIndex);
            sheet.AutoSizeColumn(alphaNColumnIndex);
            sheet.AutoSizeColumn(betaNColumnIndex);
            sheet.AutoSizeColumn(analysisValuesColumnIndex);
            sheet.AutoSizeColumn(leftYQuantileColumnIndex);
            sheet.AutoSizeColumn(complexityFunctionColumnIndex);
            sheet.AutoSizeColumn(comparisonColumnIndex);
        }
    }
}

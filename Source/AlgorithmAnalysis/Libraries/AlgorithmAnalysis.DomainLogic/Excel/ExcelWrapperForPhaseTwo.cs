using Acolyte.Assertions;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.DomainLogic.Properties;
using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelWrapperForPhaseTwo
    {
        private const int PhaseNumber = 2;

        private readonly string _outputExcelFilename;


        public ExcelWrapperForPhaseTwo(string outputExcelFilename)
        {
            _outputExcelFilename = outputExcelFilename.ThrowIfNullOrWhiteSpace(nameof(outputExcelFilename));
        }

        // TODO: implement phase two.
        public void ApplyAnalysisAndSaveData(ExcelContextForPhaseTwo excelContext)
        {
            using IExcelWorkbook workbook = ExcelHelper.GetOrCreateWorkbook(_outputExcelFilename);

            string sheetName = ExcelHelper.CreateSheetName(PhaseNumber);
            IExcelSheet sheet = workbook.GetOrCreateSheet(sheetName);
            FillSheetHeader(sheet, excelContext.Args);

            workbook.SaveToFile(_outputExcelFilename);
        }

        private static void FillSheetHeader(IExcelSheet sheet, ParametersPack args)
        {
            int currentColumnIndex = 0;

            FillLaunchesHeader(sheet, args, ref currentColumnIndex);
            FillAdditionalDataColumn(sheet, args, ref currentColumnIndex);
            FillBasicColumns(sheet, args, ref currentColumnIndex);

            var sampleMeanColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[sampleMeanColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.SampleMean);
            sheet.AutoSizeColumn(sampleMeanColumnIndex);

            var sampleVarianceColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[sampleVarianceColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.SampleVariance);
            sheet.AutoSizeColumn(sampleVarianceColumnIndex);

            var sampleDeviationColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[sampleDeviationColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.SampleDeviation);
            sheet.AutoSizeColumn(sampleDeviationColumnIndex);

            var normalizedMeanColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[normalizedMeanColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.NormalizedMean);
            sheet.AutoSizeColumn(normalizedMeanColumnIndex);

            var normalizedVarienceColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[normalizedVarienceColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.NormalizedVarience);
            sheet.AutoSizeColumn(normalizedVarienceColumnIndex);

            var alphaColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[alphaColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.Alpha);
            sheet.AutoSizeColumn(alphaColumnIndex);

            var betaColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[betaColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.Beta);
            sheet.AutoSizeColumn(betaColumnIndex);

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

        private static void FillLaunchesHeader(IExcelSheet sheet, ParametersPack args,
            ref int currentColumnIndex)
        {
            for (int launchesNumber = args.StartValue; launchesNumber <= args.EndValue;
                 launchesNumber += args.Step)
            {
                var launchesColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
                sheet[launchesColumnIndex, 1].SetValue(launchesNumber);
                sheet.AutoSizeColumn(launchesColumnIndex);
            }
        }

        private static void FillAdditionalDataColumn(IExcelSheet sheet, ParametersPack args,
            ref int currentColumnIndex)
        {
            var additionalDataColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[additionalDataColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.AdditionalData);
            sheet[additionalDataColumnIndex, 2].SetValue(args.LaunchesNumber);
            sheet[additionalDataColumnIndex, 3].SetValue(args.Step);
            sheet[additionalDataColumnIndex, 4].SetValue(double.Parse(ExcelStringsPhaseTwo.ConfidenceFactorValue));

            string significanceLevelFormula = string.Format(
                 ExcelStringsPhaseTwo.SignificanceLevelFormula,
                 sheet[additionalDataColumnIndex, 4].Address
             );
            sheet[additionalDataColumnIndex, 5].SetFormula(significanceLevelFormula);

            string operationsRange = $"2:{(args.LaunchesNumber + 1).ToString()}";
            sheet[additionalDataColumnIndex, 6].SetValue(operationsRange);

            string normalizedRange = $"{(args.LaunchesNumber + 3).ToString()}:" +
                                     $"{(args.LaunchesNumber * 2 + 2).ToString()}";
            sheet[additionalDataColumnIndex, 7].SetValue(normalizedRange);

            sheet.AutoSizeColumn(additionalDataColumnIndex);
        }

        private static void FillBasicColumns(IExcelSheet sheet, ParametersPack args,
            ref int currentColumnIndex)
        {
            int rowIndex = 1;

            var sampleSizeColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[sampleSizeColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.SampleSize);

            var theoreticalMinColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[theoreticalMinColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.TheoreticalMin);

            var theoreticalAverageColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[theoreticalAverageColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.TheoreticalAverage);

            var theoreticalMaxColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[theoreticalMaxColumnIndex, rowIndex++].SetValue(ExcelStringsPhaseTwo.TheoreticalMax);

            for (int launchesNumber = args.StartValue; launchesNumber <= args.ExtrapolationSegmentValue;
                 launchesNumber += args.Step)
            {
                sheet[sampleSizeColumnIndex, rowIndex].SetValue(launchesNumber);

                string minFormula = AnalysisHelper.GetMinFormula(sheet, sampleSizeColumnIndex, rowIndex);
                sheet[theoreticalMinColumnIndex, rowIndex].SetFormula(minFormula);

                string averageFormula = AnalysisHelper.GetAverageFormula(sheet, sampleSizeColumnIndex, rowIndex);
                sheet[theoreticalAverageColumnIndex, rowIndex].SetFormula(averageFormula);

                string maxFormula = AnalysisHelper.GetMaxFormula(sheet, sampleSizeColumnIndex, rowIndex);
                sheet[theoreticalMaxColumnIndex, rowIndex++].SetFormula(maxFormula);
            }

            sheet.AutoSizeColumn(sampleSizeColumnIndex);
            sheet.AutoSizeColumn(theoreticalMinColumnIndex);
            sheet.AutoSizeColumn(theoreticalAverageColumnIndex);
            sheet.AutoSizeColumn(theoreticalMaxColumnIndex);
        }
    }
}

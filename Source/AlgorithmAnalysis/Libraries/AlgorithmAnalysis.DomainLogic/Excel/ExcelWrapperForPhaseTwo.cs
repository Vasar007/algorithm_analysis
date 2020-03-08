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
            int iterationsNumber = args.GetIterationsNumber(PhaseNumber);

            FillLaunchesHeader(sheet, args, currentColumnIndex: 0);
            FillAdditionalDataColumn(sheet, args, iterationsNumber);

            var sampleSizeColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[sampleSizeColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.SampleSize);
            sheet.AutoSizeColumn(sampleSizeColumnIndex);

            var sampleMeanColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[sampleMeanColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.SampleMean);
            sheet.AutoSizeColumn(sampleMeanColumnIndex);

            var sampleVarianceColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[sampleVarianceColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.SampleVariance);
            sheet.AutoSizeColumn(sampleVarianceColumnIndex);

            var sampleDeviationColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[sampleDeviationColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.SampleDeviation);
            sheet.AutoSizeColumn(sampleDeviationColumnIndex);

            var theoreticalMinColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[theoreticalMinColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.TheoreticalMin);
            sheet.AutoSizeColumn(theoreticalMinColumnIndex);

            var theoreticalMaxColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[theoreticalMaxColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.TheoreticalMax);
            sheet.AutoSizeColumn(theoreticalMaxColumnIndex);

            var normalizedMeanColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[normalizedMeanColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.NormalizedMean);
            sheet.AutoSizeColumn(normalizedMeanColumnIndex);

            var normalizedVarienceColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[normalizedVarienceColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.NormalizedVarience);
            sheet.AutoSizeColumn(normalizedVarienceColumnIndex);

            var alphaColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[alphaColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.Alpha);
            sheet.AutoSizeColumn(alphaColumnIndex);

            var betaColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[betaColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.Beta);
            sheet.AutoSizeColumn(betaColumnIndex);

            ++iterationsNumber;

            var nColumnNameColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[nColumnNameColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.NColumnName);
            sheet.AutoSizeColumn(nColumnNameColumnIndex);

            var alphaNColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[alphaNColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.AlphaN);
            sheet.AutoSizeColumn(alphaNColumnIndex);

            var betaNColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[betaNColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.BetaN);
            sheet.AutoSizeColumn(betaNColumnIndex);

            ++iterationsNumber;

            var leftYQuantileColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[leftYQuantileColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.LeftYQuantile);
            sheet.AutoSizeColumn(leftYQuantileColumnIndex);

            var complexityFunctionColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[complexityFunctionColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.ComplexityFunction);
            sheet.AutoSizeColumn(complexityFunctionColumnIndex);

            var comparisonColumnIndex = iterationsNumber++.AsEnum<ExcelColumnIndex>();
            sheet[comparisonColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.Comparison);
            sheet.AutoSizeColumn(comparisonColumnIndex);
        }

        private static void FillAdditionalDataColumn(IExcelSheet sheet, ParametersPack args,
            int currentColumnIndex)
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

        private static void FillLaunchesHeader(IExcelSheet sheet, ParametersPack args,
            int currentColumnIndex)
        {
            for (int launchesNumber = args.StartValue; launchesNumber <= args.EndValue;
                 launchesNumber += args.Step)
            {
                var launchesColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
                sheet[launchesColumnIndex, 1].SetValue(launchesNumber);
                sheet.AutoSizeColumn(launchesColumnIndex);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseTwo;
using AlgorithmAnalysis.DomainLogic.Properties;
using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelWrapperForPhaseTwo
    {
        public ExcelWrapperForPhaseTwo()
        {
        }

        public void ApplyAnalysisAndSaveDataOneIteration(IEnumerable<int> data,
            ExcelContextForPhaseTwo<IAnalysisPhaseTwo> excelContext, string dataFilename)
        {
            data.ThrowIfNullOrEmpty(nameof(data));
            excelContext.ThrowIfNull(nameof(excelContext));
            dataFilename.ThrowIfNullOrWhiteSpace(nameof(dataFilename));

            using IExcelWorkbook workbook =
                ExcelHelper.GetOrCreateWorkbook(excelContext.OutputExcelFile);

            IExcelSheet sheet = workbook.GetOrCreateSheet(excelContext.SheetName);

            IAnalysisPhaseTwo analysis = excelContext.CreateAnalysis();

            int iterationNumber = excelContext.Args.GetNumberOfIterationByFilename(dataFilename);

            var currentColumn = iterationNumber.AsEnum<ExcelColumnIndex>();
            int rowCounter = GetFirstDataRowIndex();
            foreach (int item in data)
            {
                int currentRow = rowCounter++;

                sheet[currentColumn, currentRow].SetValue(item);
                analysis.ApplyAnalysisToSingleLaunch(sheet, currentColumn, currentRow, item);
            }

            if (rowCounter - 1 != GetLastDataRowIndex(excelContext.Args))
            {
                string message = "Too much data. Exceeded predefined place.";
                throw new ArgumentException(message, nameof(data));
            }

            workbook.SaveToFile(excelContext.OutputExcelFile);
            excelContext.OutputExcelFile.Refresh();
        }

        public void ApplyAnalysisAndSaveData(
            ExcelContextForPhaseTwo<IAnalysisPhaseTwo> excelContext)
        {
            excelContext.ThrowIfNull(nameof(excelContext));

            using IExcelWorkbook workbook =
                ExcelHelper.GetOrCreateWorkbook(excelContext.OutputExcelFile);

            IExcelSheet sheet = workbook.GetOrCreateSheet(excelContext.SheetName);

            int currentColumnIndex = FillSheetHeader(sheet, excelContext.Args);

            IAnalysisPhaseTwo analysis = excelContext.CreateAnalysis();
            analysis.ApplyAnalysisToDataset(sheet, currentColumnIndex);

            workbook.SaveToFile(excelContext.OutputExcelFile);
            excelContext.OutputExcelFile.Refresh();
        }

        public static int GetFirstDataRowIndex()
        {
            return 1.SkipHeader();
        }

        public static int GetLastDataRowIndex(ParametersPack args)
        {
            return args.LaunchesNumber.SkipHeader();
        }

        public static int GetNormalizedDataRowIndex(ParametersPack args, int dataRowIndex)
        {
            return args.LaunchesNumber.SkipHeader() + dataRowIndex;
        }

        public static int GetFirstNormalizedDataRowIndex(ParametersPack args)
        {
            int firstDataRowIndex = GetFirstDataRowIndex();
            return GetNormalizedDataRowIndex(args, firstDataRowIndex);
        }

        public static int GetLastNormalizedDataRowIndex(ParametersPack args)
        {
            int lastDataRowIndex = GetLastDataRowIndex(args);
            return GetNormalizedDataRowIndex(args, lastDataRowIndex);
        }

        public static ExcelColumnIndex GetAdditionalDataColumn(int iterationsNumber)
        {
            const int columnsBetweenDataAndAdditionalData = 0;

            int columnIndex = iterationsNumber + columnsBetweenDataAndAdditionalData;
            return columnIndex.AsEnum<ExcelColumnIndex>();
        }

        public static ExcelColumnIndex GetSampleSizeColumn(int iterationsNumber)
        {
            const int columnsBetweenDataAndSampleSize = 1;

            int columnIndex = iterationsNumber + columnsBetweenDataAndSampleSize;
            return columnIndex.AsEnum<ExcelColumnIndex>();
        }

        public static ExcelColumnIndex GetTheoreticalMinColumn(int iterationsNumber)
        {
            const int columnsBetweenDataAndTheoreticalMin = 2;

            int columnIndex = iterationsNumber + columnsBetweenDataAndTheoreticalMin;
            return columnIndex.AsEnum<ExcelColumnIndex>();
        }

        public static ExcelColumnIndex GetTheoreticalMaxColumn(int iterationsNumber)
        {
            const int columnsBetweenTheoreticalMinAndTheoreticalMax = 1;

            int minColumn = GetTheoreticalMinColumn(iterationsNumber).AsInt32().UseOneBasedIndexing();
            return minColumn.AsEnum<ExcelColumnIndex>(columnsBetweenTheoreticalMinAndTheoreticalMax);
        }

        private static int FillSheetHeader(IExcelSheet sheet, ParametersPack args)
        {
            // Columnm index starts with zero because we use doule-conversion trick
            // (int -> enum -> int).
            int currentColumnIndex = 0;

            FillLaunchesHeader(sheet, args, ref currentColumnIndex);
            FillAdditionalDataColumn(sheet, args, ref currentColumnIndex);
            FillBasicColumns(sheet, args, ref currentColumnIndex);

            return currentColumnIndex;
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
            sheet[additionalDataColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.AdditionalDataColumnName);
            sheet[additionalDataColumnIndex, 2].SetValue(args.LaunchesNumber);
            sheet[additionalDataColumnIndex, 3].SetValue(args.Step);
            sheet[additionalDataColumnIndex, 4].SetValue(double.Parse(ExcelStringsPhaseTwo.ConfidenceFactorValue));

            string significanceLevelFormula = string.Format(
                 ExcelStringsPhaseTwo.SignificanceLevelFormula,
                 sheet[additionalDataColumnIndex, 4].Address
             );
            sheet[additionalDataColumnIndex, 5].SetFormula(significanceLevelFormula);

            // Provide help information to see which columns contain data.
            string operationsRange = $"{GetFirstDataRowIndex().ToString()}:" +
                                     $"{GetLastDataRowIndex(args).ToString()}";
            sheet[additionalDataColumnIndex, 6].SetValue(operationsRange);

            string normalizedRange = $"{GetFirstNormalizedDataRowIndex(args).ToString()}:" +
                                     $"{GetLastNormalizedDataRowIndex(args).ToString()}";
            sheet[additionalDataColumnIndex, 7].SetValue(normalizedRange);

            sheet.AutoSizeColumn(additionalDataColumnIndex);
        }

        private static void FillBasicColumns(IExcelSheet sheet, ParametersPack args,
            ref int currentColumnIndex)
        {
            int rowIndex = 1;

            var sampleSizeColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[sampleSizeColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.SampleSizeColumnName);

            var theoreticalMinColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[theoreticalMinColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.TheoreticalMinColumnName);

            var theoreticalAverageColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[theoreticalAverageColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.TheoreticalAverageColumnName);

            var theoreticalMaxColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[theoreticalMaxColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.TheoreticalMaxColumnName);

            ++rowIndex;

            for (int launchesNumber = args.StartValue; launchesNumber <= args.ExtrapolationSegmentValue;
                 launchesNumber += args.Step)
            {
                sheet[sampleSizeColumnIndex, rowIndex].SetValue(launchesNumber);

                string minFormula = ManualFormulaProvider.Min(sheet, sampleSizeColumnIndex, rowIndex);
                sheet[theoreticalMinColumnIndex, rowIndex].SetFormula(minFormula);

                string averageFormula = ManualFormulaProvider.Average(sheet, sampleSizeColumnIndex, rowIndex);
                sheet[theoreticalAverageColumnIndex, rowIndex].SetFormula(averageFormula);

                string maxFormula = ManualFormulaProvider.Max(sheet, sampleSizeColumnIndex, rowIndex);
                sheet[theoreticalMaxColumnIndex, rowIndex].SetFormula(maxFormula);

                ++rowIndex;
            }

            sheet.AutoSizeColumn(sampleSizeColumnIndex);
            sheet.AutoSizeColumn(theoreticalMinColumnIndex);
            sheet.AutoSizeColumn(theoreticalAverageColumnIndex);
            sheet.AutoSizeColumn(theoreticalMaxColumnIndex);
        }
    }
}

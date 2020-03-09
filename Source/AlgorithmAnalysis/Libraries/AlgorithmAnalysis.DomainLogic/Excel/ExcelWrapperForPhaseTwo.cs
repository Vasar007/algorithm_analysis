using System.IO;
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

        // TODO: implement phase two.
        public void ApplyAnalysisAndSaveData(
            ExcelContextForPhaseTwo<IAnalysisPhaseTwo> excelContext)
        {
            using IExcelWorkbook workbook = ExcelHelper.GetOrCreateWorkbook(excelContext.OutputExcelFile);

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

        public static int GetFirstNormalizedDataRowIndex(ParametersPack args)
        {
            // +2 because remain blank row.
            return args.LaunchesNumber.SkipHeader() + 2;
        }

        public static int GetLastNormalizedDataRowIndex(ParametersPack args)
        {
            return args.LaunchesNumber.SkipHeader() * 2;
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
            sheet[additionalDataColumnIndex, 1].SetValue(ExcelStringsPhaseTwo.AdditionalData);
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
            sheet[sampleSizeColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.SampleSize);

            var theoreticalMinColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[theoreticalMinColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.TheoreticalMin);

            var theoreticalAverageColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[theoreticalAverageColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.TheoreticalAverage);

            var theoreticalMaxColumnIndex = currentColumnIndex++.AsEnum<ExcelColumnIndex>();
            sheet[theoreticalMaxColumnIndex, rowIndex].SetValue(ExcelStringsPhaseTwo.TheoreticalMax);

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

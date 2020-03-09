using System.Collections.Generic;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne;
using AlgorithmAnalysis.DomainLogic.Properties;
using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelWrapperForPhaseOnePartOne
    {
        public ExcelWrapperForPhaseOnePartOne()
        {
        }

        public int ApplyAnalysisAndSaveData(IEnumerable<int> data,
            ExcelContextForPhaseOne<IAnalysisPhaseOnePartOne> excelContext)
        {
            data.ThrowIfNullOrEmpty(nameof(data));
            excelContext.ThrowIfNull(nameof(excelContext));

            using IExcelWorkbook workbook =
                ExcelHelper.GetOrCreateWorkbook(excelContext.OutputExcelFile);

            IExcelSheet sheet = workbook.GetOrCreateSheet(excelContext.SheetName);
            FillSheetHeader(sheet, excelContext.Args);

            IAnalysisPhaseOnePartOne analysis = excelContext.CreatePartNAnalysis();

            int rowCounter = 2;
            foreach (int item in data)
            {
                int currentRow = rowCounter++;

                sheet[ExcelColumnIndex.A, currentRow].SetValue(item);
                analysis.ApplyAnalysisToSingleLaunch(sheet, currentRow, item);
            }

            analysis.ApplyAnalysisToDataset(sheet);

            workbook.SaveToFile(excelContext.OutputExcelFile);
            excelContext.OutputExcelFile.Refresh();

            return analysis.GetCalculatedSampleSize(sheet);
        }

        private static void FillSheetHeader(IExcelSheet sheet, ParametersPack args)
        {
            FillOperationColumn(sheet);
            FillAdditionalParametersColumns(sheet, args);
        }

        private static void FillOperationColumn(IExcelSheet sheet)
        {
            sheet[ExcelColumnIndex.A, 1].SetValue(ExcelStringsPhaseOnePartTwo.OperationColumnName);

            sheet.AutoSizeColumn(ExcelColumnIndex.A);
        }

        private static void FillAdditionalParametersColumns(IExcelSheet sheet, ParametersPack args)
        {
            // Descriptive cells.
            sheet[ExcelColumnIndex.E, 1].SetValue(ExcelStringsPhaseOnePartOne.AdditionalParametersColumnName);
            sheet[ExcelColumnIndex.E, 2].SetValue(ExcelStringsPhaseOnePartOne.InputDataSize);
            sheet[ExcelColumnIndex.E, 3].SetValue(ExcelStringsPhaseOnePartOne.MinFunc);
            sheet[ExcelColumnIndex.E, 4].SetValue(ExcelStringsPhaseOnePartOne.AverageFunc);
            sheet[ExcelColumnIndex.E, 5].SetValue(ExcelStringsPhaseOnePartOne.MaxFunc);
            sheet[ExcelColumnIndex.E, 6].SetValue(ExcelStringsPhaseOnePartOne.ExperimentsNumber);
            sheet[ExcelColumnIndex.E, 7].SetValue(ExcelStringsPhaseOnePartOne.ConfidenceFactor);
            sheet[ExcelColumnIndex.E, 8].SetValue(ExcelStringsPhaseOnePartOne.SignificanceLevel);
            sheet[ExcelColumnIndex.E, 9].SetValue(ExcelStringsPhaseOnePartOne.Epsilon);

            sheet.AutoSizeColumn(ExcelColumnIndex.E);

            // Value cells.
            sheet[ExcelColumnIndex.F, 1].SetValue(ExcelStringsPhaseOnePartOne.AdditionalParametersValuesColumnName);
            sheet[ExcelColumnIndex.F, 2].SetValue(args.StartValue);

            string minFormula = ManualFormulaProvider.Min(sheet, ExcelColumnIndex.F, 2);
            sheet[ExcelColumnIndex.F, 3].SetFormula(minFormula);

            string averageFormula = ManualFormulaProvider.Average(sheet, ExcelColumnIndex.F, 2);
            sheet[ExcelColumnIndex.F, 4].SetFormula(averageFormula);

            string maxFormula = ManualFormulaProvider.Max(sheet, ExcelColumnIndex.F, 2);
            sheet[ExcelColumnIndex.F, 5].SetFormula(maxFormula);
            sheet[ExcelColumnIndex.F, 6].SetValue(args.LaunchesNumber);
            sheet[ExcelColumnIndex.F, 7].SetValue(double.Parse(ExcelStringsPhaseOnePartOne.ConfidenceFactorValue));

            string formulaF8 = string.Format(
                ExcelStringsPhaseOnePartOne.SignificanceLevelFormula,
                sheet[ExcelColumnIndex.F, 7].Address
            );
            sheet[ExcelColumnIndex.F, 8].SetFormula(formulaF8);
            sheet[ExcelColumnIndex.F, 9].SetValue(double.Parse(ExcelStringsPhaseOnePartOne.EpsilonValue));

            sheet.AutoSizeColumn(ExcelColumnIndex.F);
        }
    }
}

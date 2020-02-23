using System.Collections.Generic;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne;
using AlgorithmAnalysis.DomainLogic.Properties;
using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelWrapperForPhaseOnePartOne
    {
        private readonly string _outputExcelFilename;


        public ExcelWrapperForPhaseOnePartOne(string outputExcelFilename)
        {
            _outputExcelFilename = outputExcelFilename.ThrowIfNullOrWhiteSpace(nameof(outputExcelFilename));
        }

        public int ApplyAnalysisAndSaveData(IEnumerable<int> data,
            ExcelContextForPhaseOne excelContext)
        {
            data.ThrowIfNullOrEmpty(nameof(data));
            excelContext.ThrowIfNull(nameof(excelContext));

            using IExcelWorkbook workbook = ExcelHelper.GetOrCreateWorkbook(_outputExcelFilename);

            IExcelSheet sheet = workbook.GetOrCreateSheet(excelContext.SheetName);
            FillSheetHeader(sheet, excelContext.Args);

            IAnalysisPhaseOnePartOne analysis = excelContext.CreatePartOne();

            int rowCounter = 2;
            foreach (int item in data)
            {
                int currentRow = rowCounter++;

                sheet.SetCenterizedCellValue(ExcelColumnIndex.A, currentRow, item);
                analysis.ApplyAnalysisToSingleLaunch(sheet, item, currentRow);
            }

            analysis.ApplyAnalysisToDataset(sheet);

            workbook.SaveToFile(_outputExcelFilename);

            return analysis.GetCalculatedSampleSize(sheet);
        }

        private static void FillSheetHeader(IExcelSheet sheet, ParametersPack args)
        {
            sheet.SetCenterizedCellValue(ExcelColumnIndex.A, 1, ExcelStrings.OperationColumnName);

            sheet.SetCenterizedCellValue(ExcelColumnIndex.E, 1, ExcelStrings.AdditionalParametersColumnName);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.E, 2, ExcelStrings.InputDataSize);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.E, 3, ExcelStrings.MinFunc);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.E, 4, ExcelStrings.AverageFunc);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.E, 5, ExcelStrings.MaxFunc);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.E, 6, ExcelStrings.ExperimentsNumber);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.E, 7, ExcelStrings.ConfidenceFactor);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.E, 8, ExcelStrings.SignificanceLevel);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.E, 9, ExcelStrings.Epsilon);

            sheet.SetCenterizedCellValue(ExcelColumnIndex.F, 1, ExcelStrings.AdditionalParametersValuesColumnName);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.F, 2, args.StartValue);

            string minFormula = AnalysisHelper.GetMinFormula(ExcelColumnIndex.F, 2);
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.F, 3, minFormula);

            string averageFormula = AnalysisHelper.GetAverageFormula(ExcelColumnIndex.F, 2);
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.F, 4, averageFormula);

            string maxFormula = AnalysisHelper.GetMaxFormula(ExcelColumnIndex.F, 2);
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.F, 5, maxFormula);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.F, 6, args.LaunchesNumber);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.F, 7, double.Parse(ExcelStrings.ConfidenceFactorValue));

            string formulaF8 = string.Format(
                ExcelStrings.SignificanceLevelFormula,
                ExcelColumnIndex.F.ToString(), "7"
            );
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.F, 8, formulaF8);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.F, 9, double.Parse(ExcelStrings.EpsilonValue));

            sheet.AutoSizeColumn(ExcelColumnIndex.A);
            sheet.AutoSizeColumn(ExcelColumnIndex.E);
            sheet.AutoSizeColumn(ExcelColumnIndex.F);
        }
    }
}

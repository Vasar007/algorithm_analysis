using System.Collections.Generic;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo;
using AlgorithmAnalysis.DomainLogic.Properties;
using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelWrapperForPhaseOnePartTwo
    {
        private readonly string _outputExcelFilename;


        public ExcelWrapperForPhaseOnePartTwo(string outputExcelFilename)
        {
            _outputExcelFilename = outputExcelFilename.ThrowIfNullOrWhiteSpace(nameof(outputExcelFilename));
        }

        public bool ApplyAnalysisAndSaveData(IEnumerable<int> data,
            ExcelContextForPhaseOne excelContext)
        {
            data.ThrowIfNullOrEmpty(nameof(data));
            excelContext.ThrowIfNull(nameof(excelContext));

            using IExcelWorkbook workbook = ExcelHelper.GetOrCreateWorkbook(_outputExcelFilename);

            IExcelSheet sheet = workbook.GetOrCreateSheet(excelContext.SheetName);
            FillSheetHeader(sheet, excelContext.Args);

            IAnalysisPhaseOnePartTwo analysis = excelContext.CreatePartTwo();

            int rowCounter = 2;
            foreach (int item in data)
            {
                int currentRow = rowCounter++;

                sheet.SetCenterizedCellValue(ExcelColumnIndex.A, currentRow, item);
                analysis.ApplyAnalysisToSingleLaunch(sheet, item, currentRow);
            }

            analysis.ApplyAnalysisToDataset(sheet);

            bool isH0HypothesisProved = analysis.CheckH0Hypothesis(sheet);
            workbook.SaveToFile(_outputExcelFilename);

            return isH0HypothesisProved;
        }

        private static void FillSheetHeader(IExcelSheet sheet, ParametersPack args)
        {
            sheet.SetCenterizedCellValue(ExcelColumnIndex.A, 1, ExcelStrings.OperationColumnName);

            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 1, ExcelStrings.AdditionalParametersColumnName);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 2, ExcelStrings.InputDataSize);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 3, ExcelStrings.MinFunc);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 4, ExcelStrings.AverageFunc);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 5, ExcelStrings.MaxFunc);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 6, ExcelStrings.ExperimentsNumber);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 7, ExcelStrings.ConfidenceFactor);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 8, ExcelStrings.SignificanceLevel);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 9, ExcelStrings.Epsilon);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 10, ExcelStrings.MinimumValue);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.I, 11, ExcelStrings.MaximumValue);

            sheet.SetCenterizedCellValue(ExcelColumnIndex.J, 1, ExcelStrings.AdditionalParametersValuesColumnName);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.J, 2, args.StartValue);
            string minFormula = AnalysisHelper.GetMinFormula(ExcelColumnIndex.J, 2);
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 3, minFormula);

            string averageFormula = AnalysisHelper.GetAverageFormula(ExcelColumnIndex.J, 2);
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 4, averageFormula);

            string maxFormula = AnalysisHelper.GetMaxFormula(ExcelColumnIndex.J, 2);
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 5, maxFormula);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.J, 6, args.LaunchesNumber);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.J, 7, double.Parse(ExcelStrings.ConfidenceFactorValue));

            string formulaJ8 = string.Format(
                 ExcelStrings.SignificanceLevelFormula,
                 ExcelColumnIndex.J.ToString(), "7"
             );
            sheet.SetCenterizedCellFormula(ExcelColumnIndex.J, 8, formulaJ8);
            sheet.SetCenterizedCellValue(ExcelColumnIndex.J, 9, double.Parse(ExcelStrings.EpsilonValue));

            sheet.AutoSizeColumn(ExcelColumnIndex.A);
            sheet.AutoSizeColumn(ExcelColumnIndex.I);
            sheet.AutoSizeColumn(ExcelColumnIndex.J);
        }
    }
}

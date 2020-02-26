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

                sheet[ExcelColumnIndex.A, currentRow].SetValue(item);
                analysis.ApplyAnalysisToSingleLaunch(sheet, item, currentRow);
            }

            analysis.ApplyAnalysisToDataset(sheet);

            workbook.SaveToFile(_outputExcelFilename);

            return analysis.GetCalculatedSampleSize(sheet);
        }

        private static void FillSheetHeader(IExcelSheet sheet, ParametersPack args)
        {
            sheet[ExcelColumnIndex.A, 1].SetValue(ExcelStrings.OperationColumnName);

            sheet[ExcelColumnIndex.E, 1].SetValue(ExcelStrings.AdditionalParametersColumnName);
            sheet[ExcelColumnIndex.E, 2].SetValue(ExcelStrings.InputDataSize);
            sheet[ExcelColumnIndex.E, 3].SetValue(ExcelStrings.MinFunc);
            sheet[ExcelColumnIndex.E, 4].SetValue(ExcelStrings.AverageFunc);
            sheet[ExcelColumnIndex.E, 5].SetValue(ExcelStrings.MaxFunc);
            sheet[ExcelColumnIndex.E, 6].SetValue(ExcelStrings.ExperimentsNumber);
            sheet[ExcelColumnIndex.E, 7].SetValue(ExcelStrings.ConfidenceFactor);
            sheet[ExcelColumnIndex.E, 8].SetValue(ExcelStrings.SignificanceLevel);
            sheet[ExcelColumnIndex.E, 9].SetValue(ExcelStrings.Epsilon);

            sheet[ExcelColumnIndex.F, 1].SetValue(ExcelStrings.AdditionalParametersValuesColumnName);
            sheet[ExcelColumnIndex.F, 2].SetValue(args.StartValue);

            string minFormula = AnalysisHelper.GetMinFormula(ExcelColumnIndex.F, 2);
            sheet[ExcelColumnIndex.F, 3].SetFormula(minFormula);

            string averageFormula = AnalysisHelper.GetAverageFormula(ExcelColumnIndex.F, 2);
            sheet[ExcelColumnIndex.F, 4].SetFormula(averageFormula);

            string maxFormula = AnalysisHelper.GetMaxFormula(ExcelColumnIndex.F, 2);
            sheet[ExcelColumnIndex.F, 5].SetFormula(maxFormula);
            sheet[ExcelColumnIndex.F, 6].SetValue(args.LaunchesNumber);
            sheet[ExcelColumnIndex.F, 7].SetValue(double.Parse(ExcelStrings.ConfidenceFactorValue));

            string formulaF8 = string.Format(
                ExcelStrings.SignificanceLevelFormula,
                ExcelColumnIndex.F.ToString(), "7"
            );
            sheet[ExcelColumnIndex.F, 8].SetFormula(formulaF8);
            sheet[ExcelColumnIndex.F, 9].SetValue(double.Parse(ExcelStrings.EpsilonValue));

            sheet.AutoSizeColumn(ExcelColumnIndex.A);
            sheet.AutoSizeColumn(ExcelColumnIndex.E);
            sheet.AutoSizeColumn(ExcelColumnIndex.F);
        }
    }
}

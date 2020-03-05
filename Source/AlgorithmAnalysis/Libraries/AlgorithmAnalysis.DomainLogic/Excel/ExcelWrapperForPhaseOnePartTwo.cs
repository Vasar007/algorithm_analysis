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
            ExcelContextForPhaseOne<IAnalysisPhaseOnePartTwo> excelContext)
        {
            data.ThrowIfNullOrEmpty(nameof(data));
            excelContext.ThrowIfNull(nameof(excelContext));

            using IExcelWorkbook workbook = ExcelHelper.GetOrCreateWorkbook(_outputExcelFilename);

            IExcelSheet sheet = workbook.GetOrCreateSheet(excelContext.SheetName);
            FillSheetHeader(sheet, excelContext.Args);

            IAnalysisPhaseOnePartTwo analysis = excelContext.CreatePartNAnalysis();

            int rowCounter = 2;
            foreach (int item in data)
            {
                int currentRow = rowCounter++;

                sheet[ExcelColumnIndex.A, currentRow].SetValue(item);
                analysis.ApplyAnalysisToSingleLaunch(sheet, item, currentRow);
            }

            analysis.ApplyAnalysisToDataset(sheet);

            workbook.SaveToFile(_outputExcelFilename);

            return analysis.CheckH0Hypothesis(sheet);
        }

        private static void FillSheetHeader(IExcelSheet sheet, ParametersPack args)
        {
            sheet[ExcelColumnIndex.A, 1].SetValue(ExcelStringsPhaseOnePartTwo.OperationColumnName);

            sheet[ExcelColumnIndex.I, 1].SetValue(ExcelStringsPhaseOnePartTwo.AdditionalParametersColumnName);
            sheet[ExcelColumnIndex.I, 2].SetValue(ExcelStringsPhaseOnePartTwo.InputDataSize);
            sheet[ExcelColumnIndex.I, 3].SetValue(ExcelStringsPhaseOnePartTwo.MinFunc);
            sheet[ExcelColumnIndex.I, 4].SetValue(ExcelStringsPhaseOnePartTwo.AverageFunc);
            sheet[ExcelColumnIndex.I, 5].SetValue(ExcelStringsPhaseOnePartTwo.MaxFunc);
            sheet[ExcelColumnIndex.I, 6].SetValue(ExcelStringsPhaseOnePartTwo.ExperimentsNumber);
            sheet[ExcelColumnIndex.I, 7].SetValue(ExcelStringsPhaseOnePartTwo.ConfidenceFactor);
            sheet[ExcelColumnIndex.I, 8].SetValue(ExcelStringsPhaseOnePartTwo.SignificanceLevel);
            sheet[ExcelColumnIndex.I, 9].SetValue(ExcelStringsPhaseOnePartTwo.Epsilon);
            sheet[ExcelColumnIndex.I, 10].SetValue(ExcelStringsPhaseOnePartTwo.MinimumValue);
            sheet[ExcelColumnIndex.I, 11].SetValue(ExcelStringsPhaseOnePartTwo.MaximumValue);

            sheet[ExcelColumnIndex.J, 1].SetValue(ExcelStringsPhaseOnePartTwo.AdditionalParametersValuesColumnName);
            sheet[ExcelColumnIndex.J, 2].SetValue(args.StartValue);
            string minFormula = AnalysisHelper.GetMinFormula(ExcelColumnIndex.J, 2);
            sheet[ExcelColumnIndex.J, 3].SetFormula(minFormula);

            string averageFormula = AnalysisHelper.GetAverageFormula(ExcelColumnIndex.J, 2);
            sheet[ExcelColumnIndex.J, 4].SetFormula(averageFormula);

            string maxFormula = AnalysisHelper.GetMaxFormula(ExcelColumnIndex.J, 2);
            sheet[ExcelColumnIndex.J, 5].SetFormula(maxFormula);
            sheet[ExcelColumnIndex.J, 6].SetValue(args.LaunchesNumber);
            sheet[ExcelColumnIndex.J, 7].SetValue(double.Parse(ExcelStringsPhaseOnePartTwo.ConfidenceFactorValue));

            string formulaJ8 = string.Format(
                 ExcelStringsPhaseOnePartTwo.SignificanceLevelFormula,
                 ExcelColumnIndex.J.ToString(), "7"
             );
            sheet[ExcelColumnIndex.J, 8].SetFormula(formulaJ8);
            sheet[ExcelColumnIndex.J, 9].SetValue(double.Parse(ExcelStringsPhaseOnePartTwo.EpsilonValue));

            sheet.AutoSizeColumn(ExcelColumnIndex.A);
            sheet.AutoSizeColumn(ExcelColumnIndex.I);
            sheet.AutoSizeColumn(ExcelColumnIndex.J);
        }
    }
}

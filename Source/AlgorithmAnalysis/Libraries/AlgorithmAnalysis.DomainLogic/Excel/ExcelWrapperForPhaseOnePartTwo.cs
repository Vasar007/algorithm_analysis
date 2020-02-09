using System.Collections.Generic;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo;
using AlgorithmAnalysis.DomainLogic.Properties;

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

            ExcelWorkbook workbook = ExcelHelper.GetWorkbook(_outputExcelFilename);

            ExcelSheet sheet = workbook.CreateSheet(excelContext.SheetName);
            FillSheetHeader(sheet, excelContext.Args);

            IAnalysisPhaseOnePartTwo analysis = excelContext.PartTwoFactory();

            int rowCounter = 2;
            foreach (int item in data)
            {
                int currentRow = rowCounter++;

                sheet
                    .GetOrCreateCenterizedCell(ExcelColumnIndex.A, currentRow)
                    .SetCellValue(item);

                analysis.ApplyAnalysisToSingleLaunch(sheet, item, currentRow);
            }

            analysis.ApplyAnalysisToDataset(sheet);

            workbook.SaveToFile(_outputExcelFilename);

            return analysis.CheckH0Hypothesis(sheet);
        }

        private static void FillSheetHeader(ExcelSheet sheet, ParametersPack args)
        {
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.A, 1)
                .SetCellValue(ExcelStrings.OperationColumnName);

            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.I, 1)
                .SetCellValue(ExcelStrings.AdditionalParametersColumnName);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.I, 2)
                .SetCellValue(ExcelStrings.InputDataSize);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.I, 3)
                .SetCellValue(ExcelStrings.MinFunc);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.I, 4)
                .SetCellValue(ExcelStrings.AverageFunc);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.I, 5)
                .SetCellValue(ExcelStrings.MaxFunc);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.I, 6)
                .SetCellValue(ExcelStrings.ExperimentsNumber);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.I, 7)
                .SetCellValue(ExcelStrings.ConfidenceFactor);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.I, 8)
                .SetCellValue(ExcelStrings.SignificanceLevel);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.I, 9)
                .SetCellValue(ExcelStrings.Epsilon);

            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 1)
                .SetCellValue(ExcelStrings.AdditionalParametersValuesColumnName);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 2)
                .SetCellValue(args.StartValue);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 3)
                .SetCellValue(ExcelStrings.MinFuncFormula); // TODO: use formula here.
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 4)
                .SetCellValue(ExcelStrings.AverageFuncFormula); // TODO: use formula here.
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 5)
                .SetCellValue(ExcelStrings.MaxFuncFormula); // TODO: use formula here.
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 6)
                .SetCellValue(args.LaunchesNumber);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 7)
                .SetCellValue(double.Parse(ExcelStrings.ConfidenceFactorValue));

            string formulaJ8 = string.Format(
                 ExcelStrings.SignificanceLevelFormula,
                 ExcelColumnIndex.J.ToString(),
                 "7"
             );
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 8)
                .SetCellFormula(formulaJ8);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.J, 9)
                .SetCellValue(double.Parse(ExcelStrings.EpsilonValue));

            sheet.AutoSizeColumn(ExcelColumnIndex.A);
            sheet.AutoSizeColumn(ExcelColumnIndex.I);
            sheet.AutoSizeColumn(ExcelColumnIndex.J);
        }
    }
}

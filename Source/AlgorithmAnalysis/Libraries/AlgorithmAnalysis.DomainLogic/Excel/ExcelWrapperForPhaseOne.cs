using System.Collections.Generic;
using System.IO;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis;
using AlgorithmAnalysis.DomainLogic.Properties;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelWrapperForPhaseOne
    {
        private readonly string _outputExcelFilename;


        public ExcelWrapperForPhaseOne(string outputExcelFilename)
        {
            _outputExcelFilename = outputExcelFilename.ThrowIfNullOrWhiteSpace(nameof(outputExcelFilename));
        }

        public int ApplyAnalysisAndSaveData(IEnumerable<int> data,
            ExcelContextForPhaseOne excelContext)
        {
            data.ThrowIfNullOrEmpty(nameof(data));
            excelContext.ThrowIfNull(nameof(excelContext));

            ExcelWorkbook workbook = GetWorkbook();

            // Create the first sheet using analysis data.
            ExcelSheet sheet = workbook.CreateSheet(excelContext.SheetName);
            FillSheetHeader(sheet, excelContext.Args);

            IAnalysisPhaseOnePartOne analysis = excelContext.AnalysisFactory(sheet);

            int rowCounter = 2;
            foreach (int item in data)
            {
                int currentRow = rowCounter++;

                sheet
                    .GetOrCreateCenterizedCell(ExcelColumnIndex.A, currentRow)
                    .SetCellValue(item);

                analysis.ApplyAnalysisToSingleLaunch(item, currentRow);
            }

            analysis.ApplyAnalysisToDataset();

            workbook.SaveToFile(_outputExcelFilename);

            return analysis.GetCalculatedSampleSize();
        }

        private ExcelWorkbook GetWorkbook()
        {
            if (File.Exists(_outputExcelFilename))
            {
                return new ExcelWorkbook(_outputExcelFilename);
            }

            return new ExcelWorkbook();
        }

        private static void FillSheetHeader(ExcelSheet sheet, ParametersPack args)
        {
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.A, 1)
                .SetCellValue(ExcelStrings.OperationColumnName);

            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.E, 1)
                .SetCellValue(ExcelStrings.AdditionalParametersColumnName);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.E, 2)
                .SetCellValue(ExcelStrings.InputDataSize);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.E, 3)
                .SetCellValue(ExcelStrings.MinFunc);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.E, 4)
                .SetCellValue(ExcelStrings.AverageFunc);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.E, 5)
                .SetCellValue(ExcelStrings.MaxFunc);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.E, 6)
                .SetCellValue(ExcelStrings.ExperimentsNumber);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.E, 7)
                .SetCellValue(ExcelStrings.ConfidenceFactor);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.E, 8)
                .SetCellValue(ExcelStrings.SignificanceLevel);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.E, 9)
                .SetCellValue(ExcelStrings.Epsilon);

            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.F, 1)
                .SetCellValue(ExcelStrings.AdditionalParametersValuesColumnName);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.F, 2)
                .SetCellValue(args.StartValue);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.F, 3)
                .SetCellValue(ExcelStrings.MinFuncFormula); // TODO: use formula here.
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.F, 4)
                .SetCellValue(ExcelStrings.AverageFuncFormula); // TODO: use formula here.
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.F, 5)
                .SetCellValue(ExcelStrings.MaxFuncFormula); // TODO: use formula here.
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.F, 6)
                .SetCellValue(args.LaunchesNumber);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.F, 7)
                .SetCellValue(double.Parse(ExcelStrings.ConfidenceFactorValue));
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.F, 8)
                .SetCellFormula(ExcelStrings.SignificanceLevelFormula);
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.F, 9)
                .SetCellValue(double.Parse(ExcelStrings.EpsilonValue));

            sheet.AutoSizeColumn(ExcelColumnIndex.A);
            sheet.AutoSizeColumn(ExcelColumnIndex.E);
            sheet.AutoSizeColumn(ExcelColumnIndex.F);
        }
    }
}

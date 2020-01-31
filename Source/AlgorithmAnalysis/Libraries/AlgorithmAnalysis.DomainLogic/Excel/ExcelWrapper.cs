using System.Collections.Generic;
using System.IO;
using Acolyte.Assertions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis;
using AlgorithmAnalysis.DomainLogic.Properties;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    // Suitable for Excel 2007. If you try to use the latest Excel 2019 functions, NPOI can throw
    // NotImplementedException.
    internal sealed class ExcelWrapper
    {
        private readonly string _filename;


        public ExcelWrapper(string filename)
        {
            _filename = filename.ThrowIfNullOrWhiteSpace(nameof(filename));
        }

        public void ApplyAnalysisAndSaveData(IEnumerable<int> data, AnalysisContext context,
            string sheetName)
        {
            data.ThrowIfNullOrEmpty(nameof(data));
            sheetName.ThrowIfNullOrEmpty(nameof(sheetName));
            context.ThrowIfNull(nameof(context));

            var workbook = new ExcelWorkbook();

            // Create the first sheet using analysis data.
            ExcelSheet sheet = workbook.CreateSheet(sheetName);
            FillSheetHeader(sheet, context.Args);

            IPhaseOnePartOneAnalysis analysis = context.CreatePhaseOnePartOneAnalysis(sheet);

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

            workbook.SaveToFile(_filename);
        }

        // TODO: remove this sample code.
        public void SaveDataToExcelFileExample(IEnumerable<int> data)
        {
            data.ThrowIfNullOrEmpty(nameof(data));

            IWorkbook workbook = new XSSFWorkbook();

            ISheet s1 = workbook.CreateSheet("Sheet1");
            //set A2
            s1.CreateRow(1).CreateCell(0).SetCellValue(-5);
            //set B2
            s1.GetRow(1).CreateCell(1).SetCellValue(1111);
            //set C2
            s1.GetRow(1).CreateCell(2).SetCellValue(7.623);
            //set A3
            s1.CreateRow(2).CreateCell(0).SetCellValue(2.2);

            //set A4=A2+A3
            s1.CreateRow(3).CreateCell(0).SetCellFormula("A2+A3");
            //set D2=SUM(A2:C2);
            s1.GetRow(1).CreateCell(3).SetCellFormula("SUM(A2:C2)");
            //set A5=cos(5)+sin(10)
            s1.CreateRow(4).CreateCell(0).SetCellFormula("cos(5)+sin(10)");

            //create another sheet
            ISheet s2 = workbook.CreateSheet("Sheet2");
            //set cross-sheet reference
            var cell = s2.CreateRow(0).CreateCell(0);
            cell.SetCellFormula("Sheet1!A2+Sheet1!A3");
            IFormulaEvaluator e = WorkbookFactory.CreateFormulaEvaluator(workbook);
            var _ = e.Evaluate(cell);

            // Write the stream data of workbook to the root directory.
            using FileStream file = new FileStream(_filename, FileMode.Create);
            workbook.Write(file);
        }

        private static void FillSheetHeader(ExcelSheet sheet, ParametersPack args)
        {
            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.A, 1)
                .SetCellValue(ExcelStrings.OperationColumnName);

            sheet
                .GetOrCreateCenterizedCell(ExcelColumnIndex.B, 1)
                .SetCellValue(ExcelStrings.NormalizedColumnName);

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
            sheet.AutoSizeColumn(ExcelColumnIndex.B);
            sheet.AutoSizeColumn(ExcelColumnIndex.E);
            sheet.AutoSizeColumn(ExcelColumnIndex.F);
        }
    }
}

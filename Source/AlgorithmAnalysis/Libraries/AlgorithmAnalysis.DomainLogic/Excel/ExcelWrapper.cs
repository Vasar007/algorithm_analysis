using System.Collections.Generic;
using System.IO;
using Acolyte.Assertions;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelWrapper
    {
        public ExcelWrapper()
        {
        }

        public void SaveDataToExcelFile(IReadOnlyList<int> data)
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
            s1.CreateRow(3).CreateCell(0).CellFormula = "A2+A3";
            //set D2=SUM(A2:C2);
            s1.GetRow(1).CreateCell(3).CellFormula = "SUM(A2:C2)";
            //set A5=cos(5)+sin(10)
            s1.CreateRow(4).CreateCell(0).CellFormula = "cos(5)+sin(10)";


            //create another sheet
            ISheet s2 = workbook.CreateSheet("Sheet2");
            //set cross-sheet reference
            var cell = s2.CreateRow(0).CreateCell(0);
            cell.CellFormula = "Sheet1!A2+Sheet1!A3";
            IFormulaEvaluator e = WorkbookFactory.CreateFormulaEvaluator(workbook);
            var _ = e.Evaluate(cell);

            //Write the stream data of workbook to the root directory
            using FileStream file = new FileStream(@"test.xlsx", FileMode.Create);
            workbook.Write(file);
        }
    }
}

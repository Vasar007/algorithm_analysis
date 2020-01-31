using System.IO;
using Acolyte.Assertions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelWorkbook
    {
        private readonly IWorkbook _workbook;


        public ExcelWorkbook()
        {
            _workbook = new XSSFWorkbook();
        }

        public ExcelSheet CreateSheet(string sheetName)
        {
            sheetName.ThrowIfNullOrEmpty(sheetName);

            ISheet sheet = _workbook.CreateSheet(sheetName);
            return new ExcelSheet(sheet);
        }

        public void SaveToFile(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace(nameof(filename));

            XSSFFormulaEvaluator.EvaluateAllFormulaCells(_workbook);

            // Write the stream data of workbook to the root directory.
            using FileStream file = new FileStream(filename, FileMode.OpenOrCreate);
            _workbook.Write(file);
        }
    }
}

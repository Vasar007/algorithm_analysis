using System.IO;
using Acolyte.Assertions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace AlgorithmAnalysis.Excel
{
    public sealed class ExcelWorkbook
    {
        // Suitable for Excel 2007. If you try to use the latest Excel 2019 functions, NPOI can
        // throw NotImplementedException.

        private readonly IWorkbook _workbook;


        public ExcelWorkbook()
        {
            _workbook = new XSSFWorkbook();
        }

        public ExcelWorkbook(string pathToWorkbook)
        {
            pathToWorkbook.ThrowIfNullOrWhiteSpace(nameof(pathToWorkbook));

            using var file = new FileStream(pathToWorkbook, FileMode.Open, FileAccess.Read);
            _workbook = new XSSFWorkbook(file);
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

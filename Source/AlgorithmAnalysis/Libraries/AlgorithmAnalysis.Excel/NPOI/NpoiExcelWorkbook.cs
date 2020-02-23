using System.IO;
using Acolyte.Assertions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.Excel.Formulas;

namespace AlgorithmAnalysis.Excel.NPOI
{
    public sealed class NpoiExcelWorkbook : IExcelWorkbook
    {
        // Suitable for Excel 2007. If you try to use the latest Excel 2019 functions, NPOI can
        // throw NotImplementedException.

        private readonly IWorkbook _workbook;

        private readonly IExcelFormulaProvider _formulaProvider;
        
        private readonly ExcelOptions _excelOptions;


        public NpoiExcelWorkbook(ExcelOptions excelOptions)
        {
            _excelOptions = excelOptions.ThrowIfNull(nameof(excelOptions));
            _workbook = new XSSFWorkbook();
            _formulaProvider = ExcelWrapperFactory.CreateFormulaProvider(excelOptions);
        }

        public NpoiExcelWorkbook(string pathToWorkbook, ExcelOptions excelOptions)
        {
            pathToWorkbook.ThrowIfNullOrWhiteSpace(nameof(pathToWorkbook));

            _excelOptions = excelOptions.ThrowIfNull(nameof(excelOptions));

            using (var file = new FileStream(pathToWorkbook, FileMode.Open, FileAccess.Read))
            {
                _workbook = new XSSFWorkbook(file);
            }
            _formulaProvider = ExcelWrapperFactory.CreateFormulaProvider(excelOptions);
        }

        #region IDisposable Implementation

        public void Dispose()
        {
            // Nothing to dispose.
        }

        #endregion

        #region IExcelWorkbook Implementation

        public IExcelSheet GetOrCreateSheet(string sheetName)
        {
            sheetName.ThrowIfNullOrEmpty(sheetName);

            ISheet sheet = _workbook.GetSheet(sheetName);
            if (sheet is null)
            {
                sheet = _workbook.CreateSheet(sheetName);
            }

            return new NpoiExcelSheet(sheet, _excelOptions, _formulaProvider);
        }

        public void SaveToFile(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace(nameof(filename));

            XSSFFormulaEvaluator.EvaluateAllFormulaCells(_workbook);

            // Write the stream data of workbook to the root directory.
            using FileStream file = new FileStream(filename, FileMode.OpenOrCreate);
            _workbook.Write(file);
        }

        #endregion
    }
}

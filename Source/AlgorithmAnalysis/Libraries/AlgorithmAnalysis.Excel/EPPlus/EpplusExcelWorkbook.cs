using System;
using System.IO;
using Acolyte.Assertions;
using OfficeOpenXml;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.Excel.EPPlus.Functions;
using AlgorithmAnalysis.Excel.Interop;
using AlgorithmAnalysis.Excel.Formulas;

namespace AlgorithmAnalysis.Excel.EPPlus
{
    internal sealed class EpplusExcelWorkbook : IExcelWorkbook
    {
        private static readonly string EpplusLibLogFilename =
            $"epplus-{DateTime.Now.ToString("yyyy-MM-dd")}.log";

        private readonly ExcelPackage _package;

        private readonly IExcelFormulaProvider _formulaProvider;

        private readonly ExcelOptions _excelOptions;

        private bool _disposed;



        public EpplusExcelWorkbook(ExcelOptions excelOptions)
        {
            InitPackageLicence();

            _excelOptions = excelOptions.ThrowIfNull(nameof(excelOptions));
            _package = new ExcelPackage();
            _formulaProvider = ExcelWrapperFactory.CreateFormulaProvider(excelOptions);

            AttachLogger();
            RegisterFunctionModules();
        }

        public EpplusExcelWorkbook(string pathToWorkbook, ExcelOptions excelOptions)
        {
            pathToWorkbook.ThrowIfNullOrWhiteSpace(nameof(pathToWorkbook));

            InitPackageLicence();

            _excelOptions = excelOptions.ThrowIfNull(nameof(excelOptions));
            _package = new ExcelPackage(new FileInfo(pathToWorkbook));
            _formulaProvider = ExcelWrapperFactory.CreateFormulaProvider(excelOptions);

            AttachLogger();
            RegisterFunctionModules();
        }

        #region IDisposable Implementation

        public void Dispose()
        {
            if (_disposed) return;

            _package.Workbook.FormulaParserManager.DetachLogger();

            _package.Dispose();
            ExcelApplication.Dispose();

            _disposed = true;
        }

        #endregion

        #region IExcelWorkbook Implementation

        public IExcelSheet GetOrCreateSheet(string sheetName)
        {
            sheetName.ThrowIfNullOrEmpty(sheetName);

            ExcelWorksheet sheet = _package.Workbook.Worksheets[sheetName];
            if (sheet is null)
            {
                sheet = _package.Workbook.Worksheets.Add(sheetName);
            }

            return new EpplusExcelSheet(sheet, _excelOptions, _formulaProvider);
        }

        public void SaveToFile(string filename)
        {
            _package.SaveAs(new FileInfo(filename));
        }

        #endregion

        private static void InitPackageLicence()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void RegisterFunctionModules()
        {
            _package.Workbook.FormulaParserManager.LoadFunctionModule(new ExtendedFunctionModule());
        }

        private void AttachLogger()
        {
            string logFolderPath = ConfigOptions.Logger.RelativeLogFolderPath;
            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }

            string logFilePath = Path.Combine(logFolderPath, EpplusLibLogFilename);

            var logfile = new FileInfo(logFilePath);
            _package.Workbook.FormulaParserManager.AttachLogger(logfile);
        }
    }
}

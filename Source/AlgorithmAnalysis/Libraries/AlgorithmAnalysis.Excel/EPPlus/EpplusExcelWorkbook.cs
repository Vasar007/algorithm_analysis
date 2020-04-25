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
        private readonly ExcelPackage _package;

        private readonly IExcelFormulaProvider _formulaProvider;

        private readonly ReportOptions _excelOptions;

        private readonly EpplusLogger _epplusLogger;

        private bool _disposed;


        public EpplusExcelWorkbook(ReportOptions excelOptions)
        {
            InitPackageLicence();

            _excelOptions = excelOptions.ThrowIfNull(nameof(excelOptions));
            _package = new ExcelPackage();
            _formulaProvider = ExcelWrapperFactory.CreateFormulaProvider(excelOptions);
            _epplusLogger = new EpplusLogger(_package, ConfigOptions.Logger);

            _epplusLogger.AttachLogger();
            RegisterFunctionModules();
        }

        public EpplusExcelWorkbook(FileInfo pathToWorkbook, ReportOptions excelOptions)
        {
            pathToWorkbook.ThrowIfNull(nameof(pathToWorkbook));

            InitPackageLicence();

            _excelOptions = excelOptions.ThrowIfNull(nameof(excelOptions));
            _package = new ExcelPackage(pathToWorkbook);
            _formulaProvider = ExcelWrapperFactory.CreateFormulaProvider(excelOptions);
            _epplusLogger = new EpplusLogger(_package, ConfigOptions.Logger);

            _epplusLogger.AttachLogger();
            RegisterFunctionModules();
        }

        #region IDisposable Implementation

        public void Dispose()
        {
            if (_disposed) return;

            _epplusLogger.DetachLogger();

            _package.Dispose();
            ExcelApplication.Dispose();

            _disposed = true;
        }

        #endregion

        #region IExcelWorkbook Implementation

        public IExcelSheet GetOrCreateSheet(string sheetName)
        {
            sheetName.ThrowIfNullOrEmpty(nameof(sheetName));

            ExcelWorksheet sheet = _package.Workbook.Worksheets[sheetName];
            if (sheet is null)
            {
                sheet = _package.Workbook.Worksheets.Add(sheetName);
            }

            return new EpplusExcelSheet(sheet, _excelOptions, _formulaProvider);
        }

        public void SaveToFile(FileInfo filename)
        {
            filename.ThrowIfNull(nameof(filename));

            _package.SaveAs(filename);
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
    }
}

﻿using System.IO;
using Acolyte.Assertions;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.Excel.EPPlus.Functions;
using AlgorithmAnalysis.Excel.Interop;
using OfficeOpenXml;

namespace AlgorithmAnalysis.Excel.EPPlus
{
    internal sealed class EpplusExcelWorkbook : IExcelWorkbook
    {
        private readonly ExcelPackage _package;

        private readonly ExcelOptions _excelOptions;

        private bool _disposed;


        public EpplusExcelWorkbook(ExcelOptions excelOptions)
        {
            InitPackageLicence();

            _package = new ExcelPackage();
            _excelOptions = excelOptions.ThrowIfNull(nameof(excelOptions));

            RegisterFunctionModules();
        }

        public EpplusExcelWorkbook(string pathToWorkbook, ExcelOptions excelOptions)
        {
            pathToWorkbook.ThrowIfNullOrWhiteSpace(nameof(pathToWorkbook));

            InitPackageLicence();

            _package = new ExcelPackage(new FileInfo(pathToWorkbook));
            _excelOptions = excelOptions.ThrowIfNull(nameof(excelOptions));

            RegisterFunctionModules();
        }

        #region IDisposable Implementation

        public void Dispose()
        {
            if (_disposed) return;

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
                sheet = _package.Workbook.Worksheets.Add(sheetName);

            return new EpplusExcelSheet(sheet, _excelOptions);
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
    }
}
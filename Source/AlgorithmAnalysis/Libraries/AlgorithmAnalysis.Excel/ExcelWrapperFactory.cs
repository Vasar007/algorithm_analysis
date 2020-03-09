using System;
using System.IO;
using Acolyte.Assertions;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.Excel.EPPlus;
using AlgorithmAnalysis.Excel.Formulas;
using AlgorithmAnalysis.Excel.NPOI;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Excel
{
    public static class ExcelWrapperFactory
    {
        #region Workbook

        public static IExcelWorkbook CreateWorkbook(FileInfo outputExcelFile,
            ExcelOptions excelOptions)
        {
            excelOptions.ThrowIfNull(nameof(excelOptions));

            return excelOptions.LibraryProvider switch
            {
                ExcelLibraryProvider.NPOI =>
                    new NpoiExcelWorkbook(outputExcelFile, excelOptions),

                ExcelLibraryProvider.EPPlus =>
                    new EpplusExcelWorkbook(outputExcelFile, excelOptions),

                _ => throw new ArgumentOutOfRangeException(
                         nameof(excelOptions), excelOptions.LibraryProvider,
                         $"Unknown Excel library provider: '{excelOptions.LibraryProvider.ToString()}'."
                     )
            };
        }

        public static IExcelWorkbook CreateWorkbook(ExcelOptions excelOptions)
        {
            excelOptions.ThrowIfNull(nameof(excelOptions));

            return excelOptions.LibraryProvider switch
            {
                ExcelLibraryProvider.NPOI => new NpoiExcelWorkbook(excelOptions),

                ExcelLibraryProvider.EPPlus => new EpplusExcelWorkbook(excelOptions),

                _ => throw new ArgumentOutOfRangeException(
                         nameof(excelOptions), excelOptions.LibraryProvider,
                         $"Unknown Excel library provider: '{excelOptions.LibraryProvider.ToString()}'."
                     )
            };
        }

        public static IExcelWorkbook CreateWorkbook(FileInfo outputExcelFile)
        {
            return CreateWorkbook(outputExcelFile, ConfigOptions.Excel);
        }

        public static IExcelWorkbook CreateWorkbook()
        {
            return CreateWorkbook(ConfigOptions.Excel);
        }

        #endregion

        #region Formula provider

        public static IExcelFormulaProvider CreateFormulaProvider(ExcelOptions excelOptions)
        {
            excelOptions.ThrowIfNull(nameof(excelOptions));

            return excelOptions.Version switch
            {
                ExcelVersion.V2007 => new ExcelFormulaProvider(excelOptions.Version),

                ExcelVersion.V2019 => new ExcelFormulaProvider(excelOptions.Version),

                _ => throw new ArgumentOutOfRangeException(
                         nameof(excelOptions), excelOptions.Version,
                         $"Unknown Excel version: '{excelOptions.Version.ToString()}'."
                     )
            };
        }

        public static IExcelFormulaProvider CreateFormulaProvider()
        {
            return CreateFormulaProvider(ConfigOptions.Excel);
        }

        #endregion
    }
}

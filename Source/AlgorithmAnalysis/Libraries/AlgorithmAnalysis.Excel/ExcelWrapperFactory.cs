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

        public static IExcelWorkbook CreateWorkbook(FileInfo outputReportFile,
            ReportOptions reportOptions)
        {
            reportOptions.ThrowIfNull(nameof(reportOptions));

            return reportOptions.LibraryProvider switch
            {
                ExcelLibraryProvider.NPOI =>
                    new NpoiExcelWorkbook(outputReportFile, reportOptions),

                ExcelLibraryProvider.EPPlus =>
                    new EpplusExcelWorkbook(outputReportFile, reportOptions),

                _ => throw new ArgumentOutOfRangeException(
                         nameof(reportOptions), reportOptions.LibraryProvider,
                         $"Unknown Excel library provider: '{reportOptions.LibraryProvider.ToString()}'."
                     )
            };
        }

        public static IExcelWorkbook CreateWorkbook(ReportOptions reportOptions)
        {
            reportOptions.ThrowIfNull(nameof(reportOptions));

            return reportOptions.LibraryProvider switch
            {
                ExcelLibraryProvider.NPOI => new NpoiExcelWorkbook(reportOptions),

                ExcelLibraryProvider.EPPlus => new EpplusExcelWorkbook(reportOptions),

                _ => throw new ArgumentOutOfRangeException(
                         nameof(reportOptions), reportOptions.LibraryProvider,
                         $"Unknown Excel library provider: '{reportOptions.LibraryProvider.ToString()}'."
                     )
            };
        }

        public static IExcelWorkbook CreateWorkbook(FileInfo outputReportFile)
        {
            return CreateWorkbook(outputReportFile, ConfigOptions.Report);
        }

        public static IExcelWorkbook CreateWorkbook()
        {
            return CreateWorkbook(ConfigOptions.Report);
        }

        #endregion

        #region Formula provider

        public static IExcelFormulaProvider CreateFormulaProvider(ReportOptions reportOptions)
        {
            reportOptions.ThrowIfNull(nameof(reportOptions));

            return reportOptions.ExcelVersion switch
            {
                ExcelVersion.V2007 => new ExcelFormulaProvider(reportOptions.ExcelVersion),

                ExcelVersion.V2019 => new ExcelFormulaProvider(reportOptions.ExcelVersion),

                _ => throw new ArgumentOutOfRangeException(
                         nameof(reportOptions), reportOptions.ExcelVersion,
                         $"Unknown Excel version: '{reportOptions.ExcelVersion.ToString()}'."
                     )
            };
        }

        public static IExcelFormulaProvider CreateFormulaProvider()
        {
            return CreateFormulaProvider(ConfigOptions.Report);
        }

        #endregion
    }
}

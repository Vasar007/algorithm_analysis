using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.Excel.EPPlus;
using AlgorithmAnalysis.Excel.NPOI;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Excel
{
    public static class ExcelWrapperFactory
    {
        public static IExcelWorkbook CreateWorkbook(string outputExcelFilename,
            ExcelOptions excelOptions)
        {
            excelOptions.ThrowIfNull(nameof(excelOptions));

            return excelOptions.LibraryProvider switch
            {
                ExcelLibraryProvider.Default =>
                    CreateDefaultWorkbook(outputExcelFilename, excelOptions),

                ExcelLibraryProvider.NPOI =>
                    new NpoiExcelWorkbook(outputExcelFilename, excelOptions),

                ExcelLibraryProvider.EPPlus =>
                    new EpplusExcelWorkbook(outputExcelFilename, excelOptions),

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
                ExcelLibraryProvider.Default => CreateDefaultWorkbook(excelOptions),

                ExcelLibraryProvider.NPOI => new NpoiExcelWorkbook(excelOptions),

                ExcelLibraryProvider.EPPlus => new EpplusExcelWorkbook(excelOptions),

                _ => throw new ArgumentOutOfRangeException(
                         nameof(excelOptions), excelOptions.LibraryProvider,
                         $"Unknown Excel library provider: '{excelOptions.LibraryProvider.ToString()}'."
                     )
            };
        }

        public static IExcelWorkbook CreateWorkbook(string outputExcelFilename)
        {
            return CreateWorkbook(outputExcelFilename, ConfigOptions.Excel);
        }

        public static IExcelWorkbook CreateWorkbook()
        {
            return CreateWorkbook(ConfigOptions.Excel);
        }

        public static IExcelWorkbook CreateDefaultWorkbook(string outputExcelFilename,
            ExcelOptions excelOptions)
        {
            return new NpoiExcelWorkbook(outputExcelFilename, excelOptions);
        }

        public static IExcelWorkbook CreateDefaultWorkbook(ExcelOptions excelOptions)
        {
            return new NpoiExcelWorkbook(excelOptions);
        }
    }
}

using System.IO;
using Acolyte.Assertions;
using AlgorithmAnalysis.Excel;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal static class ExcelHelper
    {
        public const string SheetNamePrefix = "Sheet";

        public static IExcelWorkbook GetOrCreateWorkbook(FileInfo outputExcelFile)
        {
            outputExcelFile.ThrowIfNull(nameof(outputExcelFile));

            // Use static File method because FileInfo can have out-of-date state.
            if (File.Exists(outputExcelFile.FullName))
            {
                return ExcelWrapperFactory.CreateWorkbook(outputExcelFile);
            }

            return ExcelWrapperFactory.CreateWorkbook();
        }

        public static string CreateSheetName(string sheetNamePrefix, string phaseNumber,
            string? iterationNumber)
        {
            sheetNamePrefix.ThrowIfNull(nameof(sheetNamePrefix));
            phaseNumber.ThrowIfNullOrEmpty(nameof(phaseNumber));

            return iterationNumber is null
                    ? $"{sheetNamePrefix}{phaseNumber.ToString()}"
                    : $"{sheetNamePrefix}{phaseNumber.ToString()}-{iterationNumber.ToString()}";
        }

        public static string CreateSheetName(string sheetNamePrefix, int phaseNumber,
            int iterationNumber)
        {
            return CreateSheetName(
                sheetNamePrefix, phaseNumber.ToString(), iterationNumber.ToString()
            );
        }

        public static string CreateSheetName(int phaseNumber)
        {
            return CreateSheetName(SheetNamePrefix, phaseNumber);
        }

        public static string CreateSheetName(string sheetNamePrefix, int phaseNumber)
        {
            return CreateSheetName(
                sheetNamePrefix, phaseNumber.ToString(), iterationNumber: null
            );
        }

        public static string CreateSheetName(int phaseNumber, int iterationNumber)
        {
            return CreateSheetName(SheetNamePrefix, phaseNumber, iterationNumber);
        }
    }
}

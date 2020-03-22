using System.IO;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Configuration;

namespace AlgorithmAnalysis.DesktopApp.Domain
{
    internal sealed class ResultWrapper
    {
        public FileInfo OutputExcelFile { get; }


        private ResultWrapper(FileInfo outputExcelFile)
        {
            OutputExcelFile = outputExcelFile.ThrowIfNull(nameof(outputExcelFile));
        }

        public static ResultWrapper Create(ExcelOptions excelOptions)
        {
            string outputExcelFolderPath = Utils.GetOrCreateFolderUsingFilePath(
                excelOptions.OutputExcelFilename
            );

            string outputExcelFilename = Path.GetFileName(excelOptions.OutputExcelFilename);
            string outputExcelFilePath = Path.Combine(outputExcelFolderPath, outputExcelFilename);

            var outputExcelFile = new FileInfo(outputExcelFilePath);
            return new ResultWrapper(outputExcelFile);
        }
    }
}

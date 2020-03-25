using System.IO;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Configuration;

namespace AlgorithmAnalysis.DesktopApp.Models
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
            string outputExcelFilePath = Utils.GetOrCreateFolderAndAppendFilePathToResult(
                excelOptions.OutputExcelFilePath, CommonConstants.DefaultResultFolderPath
            );

            var outputExcelFile = new FileInfo(outputExcelFilePath);
            return new ResultWrapper(outputExcelFile);
        }
    }
}

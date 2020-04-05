using System.IO;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Configuration;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class ResultWrapper
    {
        public FileInfo OutputReportFile { get; }


        private ResultWrapper(FileInfo outputReportFile)
        {
            OutputReportFile = outputReportFile.ThrowIfNull(nameof(outputReportFile));
        }

        public static ResultWrapper Create(ReportOptions reportOptions)
        {
            string outputReportFilePath = Utils.GetOrCreateFolderAndAppendFilePathToResult(
                reportOptions.OutputReportFilePath, PredefinedPaths.DefaultResultFolderPath
            );

            var outputReportFile = new FileInfo(outputReportFilePath);
            return new ResultWrapper(outputReportFile);
        }
    }
}

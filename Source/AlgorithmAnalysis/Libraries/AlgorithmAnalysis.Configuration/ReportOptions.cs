using System.IO;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Configuration
{
    public sealed class ReportOptions : IOptions
    {
        public ExcelCellCreationMode CellCreationMode { get; set; } = ExcelCellCreationMode.Centerized;

        public ExcelLibraryProvider LibraryProvider { get; set; } = ExcelLibraryProvider.EPPlus;

        public ExcelVersion ExcelVersion { get; set; } = ExcelVersion.V2007;

        public string OutputReportFilePath { get; set; } =
             Path.Combine(PredefinedPaths.DefaultResultFolderPath, CommonConstants.DefaultResultFilename);


        public ReportOptions()
        {
        }
    }
}

using System.IO;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Configuration
{
    public sealed class ExcelOptions : IOptions
    {
        public ExcelCellCreationMode CellCreationMode { get; set; } = ExcelCellCreationMode.Centerized;

        public ExcelLibraryProvider LibraryProvider { get; set; } = ExcelLibraryProvider.EPPlus;

        public ExcelVersion Version { get; set; } = ExcelVersion.V2007;

        public string OutputExcelFilePath { get; set; } =
             Path.Combine(CommonConstants.DefaultResultFolderPath, CommonConstants.DefaultResultFilename);


        public ExcelOptions()
        {
        }
    }
}

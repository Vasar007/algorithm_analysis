using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Configuration
{
    public sealed class ExcelOptions : IOptions
    {
        public ExcelCellCreationMode CellCreationMode { get; set; } = ExcelCellCreationMode.Centerized;

        public ExcelLibraryProvider LibraryProvider { get; set; } = ExcelLibraryProvider.EPPlus;

        public ExcelVersion Version { get; set; } = ExcelVersion.V2019;

        public string OutputExcelFilename { get; set; } = "results.xlsx";


        public ExcelOptions()
        {
        }
    }
}

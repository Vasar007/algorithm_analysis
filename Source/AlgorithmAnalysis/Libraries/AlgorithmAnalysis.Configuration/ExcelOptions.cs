using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Configuration
{
    public sealed class ExcelOptions : IOptions
    {
        public ExcelCellCreationMode CellCreationMode { get; set; } = ExcelCellCreationMode.Default;

        public ExcelLibraryProvider LibraryProvider { get; set; } = ExcelLibraryProvider.Default;


        public ExcelOptions()
        {
        }
    }
}

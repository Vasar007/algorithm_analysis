using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Configuration
{
    public sealed class ExcelOptions : IOptions
    {
        public ExcelCellCreationMode CellCreationMode { get; set; } = ExcelCellCreationMode.Default;


        public ExcelOptions()
        {
        }
    }
}

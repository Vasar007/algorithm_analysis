using System;

namespace AlgorithmAnalysis.Excel
{
    public interface IExcelWorkbook : IDisposable
    {
        IExcelSheet GetOrCreateSheet(string sheetName);

        void SaveToFile(string filename);
    }
}

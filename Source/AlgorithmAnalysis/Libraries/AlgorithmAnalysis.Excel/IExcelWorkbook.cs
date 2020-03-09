using System;
using System.IO;

namespace AlgorithmAnalysis.Excel
{
    public interface IExcelWorkbook : IDisposable
    {
        IExcelSheet GetOrCreateSheet(string sheetName);

        void SaveToFile(FileInfo filename);
    }
}

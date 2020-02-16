namespace AlgorithmAnalysis.Excel
{
    public interface IExcelWorkbook
    {
        IExcelSheet CreateSheet(string sheetName);

        void SaveToFile(string filename);
    }
}

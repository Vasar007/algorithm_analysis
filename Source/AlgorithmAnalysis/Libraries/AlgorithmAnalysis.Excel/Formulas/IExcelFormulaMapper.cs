using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Excel.Formulas
{
    public interface IExcelFormulaMapper
    {
        string GetFormulaName(ExcelVersion excelVersion, string methodName);
    }
}

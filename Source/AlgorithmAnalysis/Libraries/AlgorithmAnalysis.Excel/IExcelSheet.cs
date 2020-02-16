using System;

namespace AlgorithmAnalysis.Excel
{
    public interface IExcelSheet
    {
        ICellHolder this[ExcelColumnIndex columnIndex, int rowIndex] { get; }


        ICellHolder GetOrCreateCell(ExcelColumnIndex columnIndex, int rowIndex, bool centrized);

        ICellHolder GetOrCreateCell(ExcelColumnIndex columnIndex, int rowIndex);

        ICellHolder GetOrCreateCenterizedCell(ExcelColumnIndex columnIndex, int rowIndex);
        
        void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, string value);
        
        void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, bool value);
        
        void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, double value);
        
        void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, DateTime value);
        
        void SetCellFormula(ExcelColumnIndex columnIndex, int rowIndex, string formula);
        
        void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex, string value);
        
        void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex, bool value);
        
        void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex, double value);
        
        void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex, DateTime value);
        
        void SetCenterizedCellFormula(ExcelColumnIndex columnIndex, int rowIndex, string formula);
        
        void AddMergedRegion(ExcelColumnIndex firstColumnIndex, int firstRowIndex,
            ExcelColumnIndex lastColumnIndex, int lastRowIndex);
        
        void AutoSizeColumn(ExcelColumnIndex columnIndex);
        
        void AutoSizeColumn(ExcelColumnIndex columnIndex, bool useMergedCells);

        ICellValueHolder EvaluateCell(ExcelColumnIndex columnIndex, int rowIndex);
        
        void SetArrayFormula(string formula,
            ExcelColumnIndex firstColumnIndex, int firstRowIndex,
            ExcelColumnIndex lastColumnIndex, int lastRowIndex);

    }
}

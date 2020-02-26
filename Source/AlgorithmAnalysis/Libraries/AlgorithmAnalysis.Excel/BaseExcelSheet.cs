using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.Excel.Formulas;

namespace AlgorithmAnalysis.Excel
{
    internal abstract class BaseExcelSheet : IExcelSheet
    {
        protected readonly ExcelOptions _excelOptions;

        public IExcelFormulaProvider FormulaProvider { get; }

        public IExcelCellHolder this[ExcelColumnIndex columnIndex, int rowIndex] =>
            ExcelWrapperHelper.GetCellHolder(this, columnIndex, rowIndex, _excelOptions);


        protected BaseExcelSheet(ExcelOptions excelOptions, IExcelFormulaProvider provider)
        {
            _excelOptions = excelOptions.ThrowIfNull(nameof(excelOptions));
            FormulaProvider = provider.ThrowIfNull(nameof(provider));
        }

        public abstract IExcelCellHolder GetOrCreateCell(ExcelColumnIndex columnIndex, int rowIndex,
            bool centrized);

        public IExcelCellHolder GetOrCreateCell(ExcelColumnIndex columnIndex, int rowIndex)
        {
            return GetOrCreateCell(columnIndex, rowIndex, centrized: false);
        }

        public IExcelCellHolder GetOrCreateCenterizedCell(ExcelColumnIndex columnIndex, int rowIndex)
        {
            return GetOrCreateCell(columnIndex, rowIndex, centrized: true);
        }

        public void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, string value)
        {
            GetOrCreateCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, bool value)
        {
            GetOrCreateCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, double value)
        {
            GetOrCreateCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCellValue(ExcelColumnIndex columnIndex, int rowIndex, DateTime value)
        {
            GetOrCreateCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCellFormula(ExcelColumnIndex columnIndex, int rowIndex, string formula)
        {
            formula.ThrowIfNullOrWhiteSpace(nameof(formula));

            GetOrCreateCell(columnIndex, rowIndex).SetFormula(formula);
        }

        public void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex, string value)
        {
            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex, bool value)
        {
            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex, double value)
        {
            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCenterizedCellValue(ExcelColumnIndex columnIndex, int rowIndex,
            DateTime value)
        {
            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetValue(value);
        }

        public void SetCenterizedCellFormula(ExcelColumnIndex columnIndex, int rowIndex,
            string formula)
        {
            formula.ThrowIfNullOrWhiteSpace(nameof(formula));

            GetOrCreateCenterizedCell(columnIndex, rowIndex).SetFormula(formula);
        }

        public abstract void EvaluateAll();

        public IExcelCellValueHolder EvaluateCell(ExcelColumnIndex columnIndex, int rowIndex)
        {
            return this[columnIndex, rowIndex].Evaluate();
        }

        public abstract void AddMergedRegion(
            ExcelColumnIndex firstColumnIndex,
            int firstRowIndex,
            ExcelColumnIndex lastColumnIndex,
            int lastRowIndex);

        public abstract void AutoSizeColumn(ExcelColumnIndex columnIndex);

        public abstract void AutoSizeColumn(ExcelColumnIndex columnIndex, bool useMergedCells);

        public abstract void SetArrayFormula(
            string arrayFormula,
            ExcelColumnIndex firstColumnIndex,
            int firstRowIndex,
            ExcelColumnIndex lastColumnIndex,
            int lastRowIndex);
    }
}

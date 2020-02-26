using Acolyte.Assertions;
using OfficeOpenXml;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Excel.Formulas;

namespace AlgorithmAnalysis.Excel.EPPlus
{
    internal sealed class EpplusExcelSheet : BaseExcelSheet, IExcelSheet
    {
        private readonly ExcelWorksheet _sheet;


        internal EpplusExcelSheet(
            ExcelWorksheet sheet,
            ExcelOptions excelOptions,
            IExcelFormulaProvider formulaProvider)
            : base(excelOptions, formulaProvider)
        {
            _sheet = sheet.ThrowIfNull(nameof(sheet));
        }

        public override IExcelCellHolder GetOrCreateCell(ExcelColumnIndex columnIndex, int rowIndex,
            bool centrized)
        {
            string cellAddress = ExcelWrapperHelper.GetCellAddressFrom(columnIndex, rowIndex);
            ExcelRange excelRange = _sheet.Cells[cellAddress];

            excelRange = centrized
                ? excelRange.Center()
                : excelRange;

            return new EpplusExcelCellHolder(excelRange);
        }

        public override void AddMergedRegion(
            ExcelColumnIndex firstColumnIndex,
            int firstRowIndex,
            ExcelColumnIndex lastColumnIndex,
            int lastRowIndex)
        {
            string firstCellAddress =
                ExcelWrapperHelper.GetCellAddressFrom(firstColumnIndex, firstRowIndex);

            string lastCellAddress =
                ExcelWrapperHelper.GetCellAddressFrom(lastColumnIndex, lastRowIndex);

            _sheet.Cells[$"{firstCellAddress}:{lastCellAddress}"].Merge = true;
        }

        public override void AutoSizeColumn(ExcelColumnIndex columnIndex)
        {
            columnIndex.ThrowIfEnumValueIsUndefined(nameof(columnIndex));

            _sheet.Column(columnIndex.AsInt32().UseOneBasedIndexing()).AutoFit();
        }

        public override void AutoSizeColumn(ExcelColumnIndex columnIndex, bool useMergedCells)
        {
            columnIndex.ThrowIfEnumValueIsUndefined(nameof(columnIndex));

            // TODO: find a way to enable auto size for merged cells.
            _sheet.Column(columnIndex.AsInt32().UseOneBasedIndexing()).AutoFit();
        }

        public override void EvaluateAll()
        {
            _sheet.Calculate();
        }

        public override void SetArrayFormula(
            string arrayFormula,
            ExcelColumnIndex firstColumnIndex,
            int firstRowIndex,
            ExcelColumnIndex lastColumnIndex,
            int lastRowIndex)
        {
            arrayFormula.ThrowIfNullOrWhiteSpace(nameof(arrayFormula));

            string firstCellAddress =
                ExcelWrapperHelper.GetCellAddressFrom(firstColumnIndex, firstRowIndex);

            string lastCellAddress =
                ExcelWrapperHelper.GetCellAddressFrom(lastColumnIndex, lastRowIndex);

            _sheet.Cells[$"{firstCellAddress}:{lastCellAddress}"].CreateArrayFormula(arrayFormula);
        }
    }
}

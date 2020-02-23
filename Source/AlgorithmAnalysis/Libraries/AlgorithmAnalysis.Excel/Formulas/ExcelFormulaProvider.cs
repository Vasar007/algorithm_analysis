﻿using Acolyte.Assertions;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Excel.Formulas
{
    internal sealed class ExcelFormulaProvider : IExcelFormulaProvider
    {
        private readonly ExcelFormulaNamesMapper _mapper;

        private readonly ExcelVersion _excelVersion;


        public ExcelFormulaProvider(ExcelVersion excelVersion)
        {
            _excelVersion = excelVersion.ThrowIfEnumValueIsUndefined(nameof(excelVersion));

            _mapper = new ExcelFormulaNamesMapper();
        }

        #region IFormulaProvider Implementation

        public string Average(string range)
        {
            range.ThrowIfNullOrWhiteSpace(nameof(range));

            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({range})";
        }

        public string BetaDist(string x, string alpha, string beta, bool cumulative)
        {
            x.ThrowIfNullOrWhiteSpace(nameof(x));
            alpha.ThrowIfNullOrWhiteSpace(nameof(alpha));
            beta.ThrowIfNullOrWhiteSpace(nameof(beta));

            string cumulativeStr = cumulative
                ? "TRUE"
                : "FALSE";

            // Original formula has 6 parameters but we do not use optional parameters.
            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({x}, {alpha}, {beta}, {cumulativeStr},)";
        }

        public string ChiInv(string probability, string degreeFreedom)
        {
            probability.ThrowIfNullOrWhiteSpace(nameof(probability));
            degreeFreedom.ThrowIfNullOrWhiteSpace(nameof(degreeFreedom));

            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({probability}, {degreeFreedom})";
        }

        public string ChiTest(string actualRange, string expectedRange)
        {
            actualRange.ThrowIfNullOrWhiteSpace(nameof(actualRange));
            expectedRange.ThrowIfNullOrWhiteSpace(nameof(expectedRange));

            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({actualRange}, {expectedRange})";
        }

        public string CountIfS(string criteriaRange1, string criteria1)
        {
            criteriaRange1.ThrowIfNullOrWhiteSpace(nameof(criteriaRange1));
            criteria1.ThrowIfNullOrWhiteSpace(nameof(criteria1));

            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({criteriaRange1}, {criteria1})";
        }

        public string CountIfS(string criteriaRange1, string criteria1,
            string criteriaRange2, string criteria2)
        {
            criteriaRange1.ThrowIfNullOrWhiteSpace(nameof(criteriaRange1));
            criteria1.ThrowIfNullOrWhiteSpace(nameof(criteria1));
            criteriaRange2.ThrowIfNullOrWhiteSpace(nameof(criteriaRange2));
            criteria2.ThrowIfNullOrWhiteSpace(nameof(criteria2));

            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({criteriaRange1}, {criteria1}, {criteriaRange2}, {criteria2})";
        }

        public string Frequency(string dataArray, string binsArray)
        {
            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({dataArray}, {binsArray})";
        }

        public string Max(string range)
        {
            range.ThrowIfNullOrWhiteSpace(nameof(range));

            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({range})";
        }

        public string Min(string range)
        {
            range.ThrowIfNullOrWhiteSpace(nameof(range));

            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({range})";
        }

        public string RoundUp(string number, string numberOfDigits)
        {
            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({number}, {numberOfDigits})";
        }

        public string StdDev(string range)
        {
            range.ThrowIfNullOrWhiteSpace(nameof(range));

            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({range})";
        }

        public string Sum(string range)
        {
            range.ThrowIfNullOrWhiteSpace(nameof(range));

            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({range})";
        }

        public string Var(string range)
        {
            range.ThrowIfNullOrWhiteSpace(nameof(range));

            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({range})";
        }

        #endregion
    }
}

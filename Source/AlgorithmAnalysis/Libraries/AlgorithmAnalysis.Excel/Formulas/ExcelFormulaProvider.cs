using System;
using Acolyte.Assertions;
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

        public string GetFormulaByName(string methodName)
        {
            methodName.ThrowIfNullOrWhiteSpace(nameof(methodName));

            return _mapper.GetFormulaName(_excelVersion, methodName);
        }

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

            string lastPart = _excelVersion switch
            {
                ExcelVersion.V2007 => string.Empty,

                ExcelVersion.V2019 => $", {cumulativeStr},",

                _ => throw new ArgumentOutOfRangeException(
                         nameof(_excelVersion), _excelVersion,
                         $"Unknown Excel version: '{_excelVersion.ToString()}'."
                     )
            };
            return $"{formulaName}({x}, {alpha}, {beta}{lastPart})";
        }

        public string BetaInv(string probability, string alpha, string beta)
        {
            probability.ThrowIfNullOrWhiteSpace(nameof(probability));
            alpha.ThrowIfNullOrWhiteSpace(nameof(alpha));
            beta.ThrowIfNullOrWhiteSpace(nameof(beta));

            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({probability}, {alpha}, {beta})";
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

        public string Exp(string number)
        {
            number.ThrowIfNullOrWhiteSpace(nameof(number));

            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({number})";
        }

        public string Frequency(string dataArray, string binsArray)
        {
            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({dataArray}, {binsArray})";
        }

        public string Ln(string number)
        {
            number.ThrowIfNullOrWhiteSpace(nameof(number));

            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({number})";
        }

        public string Log(string number, string @base)
        {
            number.ThrowIfNullOrWhiteSpace(nameof(number));
            @base.ThrowIfNullOrWhiteSpace(nameof(@base));

            string formulaName = _mapper.GetFormulaName(_excelVersion);
            return $"{formulaName}({number}, {@base})";
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

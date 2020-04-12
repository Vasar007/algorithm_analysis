using System;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Parsing
{
    public sealed class SimpleFormulaParser : IFormulaParser
    {
        public SimpleFormulaParser()
        {
        }

        #region IFormulaParser Implementation

        public string ParseFormulaFormat(string formulaFormat)
        {
            formulaFormat.ThrowIfNullOrWhiteSpace(nameof(formulaFormat));

            return formulaFormat.Replace(
                CommonConstants.StringFormatZero,
                CommonConstants.FormulaSymbol,
                StringComparison.InvariantCultureIgnoreCase
            );
        }

        public string ParseRawFormula(string rawFormula)
        {
            rawFormula.ThrowIfNullOrWhiteSpace(nameof(rawFormula));

            return rawFormula.Replace(
                CommonConstants.FormulaSymbol,
                CommonConstants.StringFormatZero,
                StringComparison.InvariantCultureIgnoreCase
            );
        }

        #endregion
    }
}

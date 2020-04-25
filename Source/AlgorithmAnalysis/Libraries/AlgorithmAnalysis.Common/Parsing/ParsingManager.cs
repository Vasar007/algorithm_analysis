using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Parsing
{
    public static class ParsingManager
    {
        public static string TransformFormulaFormatToRawFormula(string formulaFormat,
            IFormulaParser formulaParser)
        {
            formulaParser.ThrowIfNull(nameof(formulaParser));

            return formulaParser.ParseFormulaFormat(formulaFormat);
        }

        public static string TransformFormulaFormatToRawFormula(string formulaFormat)
        {
            var formulaParser = new SimpleFormulaParser();
            return TransformFormulaFormatToRawFormula(formulaFormat, formulaParser);
        }

        public static string TransformRawFormulaToFormulaFormat(string rawFormula,
            IFormulaParser formulaParser)
        {
            formulaParser.ThrowIfNull(nameof(formulaParser));

            return formulaParser.ParseRawFormula(rawFormula);
        }

        public static string TransformRawFormulaToFormulaFormat(string rawFormula)
        {
            var formulaParser = new SimpleFormulaParser();
            return TransformRawFormulaToFormulaFormat(rawFormula, formulaParser);
        }
    }
}

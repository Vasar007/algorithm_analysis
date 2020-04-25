namespace AlgorithmAnalysis.Common.Parsing
{
    public interface IFormulaParser
    {
        string ParseFormulaFormat(string formulaFormat);

        string ParseRawFormula(string rawFormula);
    }
}

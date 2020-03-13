namespace AlgorithmAnalysis.Excel.Formulas
{
    public interface IExcelFormulaProvider
    {
        string GetFormulaByName(string methodName);


        string Average(string range);

        string BetaDist(string x, string alpha, string beta, bool cumulative);

        string BetaInv(string probability, string alpha, string beta);

        string ChiInv(string probability, string degreeFreedom);

        string ChiTest(string actualRange, string expectedRange);

        string CountIfS(string criteriaRange1, string criteria1);

        string CountIfS(string criteriaRange1, string criteria1,
            string criteriaRange2, string criteria2);

        string Exp(string number);

        string Frequency(string dataArray, string binsArray);

        string Log(string number, string @base);

        string Ln(string number);

        string Max(string range);

        string Min(string range);

        string RoundUp(string number, string numberOfDigits);

        string StdDev(string range);

        string Sum(string range);

        string Var(string range);
    }
}

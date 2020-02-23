using OfficeOpenXml.FormulaParsing.Excel.Functions;

namespace AlgorithmAnalysis.Excel.EPPlus.Functions
{
    internal sealed class ExtendedFunctionModule : FunctionsModule
    {
        public ExtendedFunctionModule()
        {
            // TODO: implement proper excel formula provider.
            // CHIINV == CHISQ.INV.RT
            Functions.Add("CHISQ.INV.RT", new ChisqInvRtExcelFunction());
            Functions.Add("CHIINV", new ChisqInvRtExcelFunction());

            // CHITEST == CHISQ.TEST
            Functions.Add("CHISQ.TEST", new ChisqTestExcelFunction());
            Functions.Add("CHITEST", new ChisqTestExcelFunction());

            // BETADIST == BETA.DIST
            Functions.Add("BETA.DIST", new BetaDistExcelFunction());
            Functions.Add("BETADIST", new BetaDistExcel2007Function());

            Functions.Add("FREQUENCY", new FrequencyExcelFunction());
        }
    }
}

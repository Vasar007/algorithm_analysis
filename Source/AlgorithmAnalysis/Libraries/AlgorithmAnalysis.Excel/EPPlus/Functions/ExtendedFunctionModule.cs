using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using AlgorithmAnalysis.Excel.Formulas;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Excel.EPPlus.Functions
{
    internal sealed class ExtendedFunctionModule : FunctionsModule
    {
        private static readonly IReadOnlyList<(string FormulaName, ExcelFunction ExcelFunction)> _cache =
            PrepareCache();


        public ExtendedFunctionModule()
        {
            foreach ((string formulaName, ExcelFunction excelFunction) in _cache)
            {
                Functions.Add(formulaName, excelFunction);
            }
        }

        private static IReadOnlyList<(string FormulaName, ExcelFunction ExcelFunction)>
            PrepareCache()
        {
            var result = new List<(string FormulaName, ExcelFunction ExcelFunction)>();

            var mapper = new ExcelFormulaNamesMapper();

            // CHIINV == CHISQ.INV.RT
            string chiInv2019 = mapper.GetFormulaName(ExcelVersion.V2019, nameof(IExcelFormulaProvider.ChiInv));
            result.Add((chiInv2019, new ChisqInvRtExcelFunction()));
            string chiInv2007 = mapper.GetFormulaName(ExcelVersion.V2007, nameof(IExcelFormulaProvider.ChiInv));
            result.Add((chiInv2007, new ChisqInvRtExcelFunction()));

            // CHITEST == CHISQ.TEST
            string chiTest2019 = mapper.GetFormulaName(ExcelVersion.V2019, nameof(IExcelFormulaProvider.ChiTest));
            result.Add((chiTest2019, new ChisqTestExcelFunction()));
            string chiTest2007 = mapper.GetFormulaName(ExcelVersion.V2007, nameof(IExcelFormulaProvider.ChiTest));
            result.Add((chiTest2007, new ChisqTestExcelFunction()));

            // BETADIST == BETA.DIST
            string betaDist2019 = mapper.GetFormulaName(ExcelVersion.V2019, nameof(IExcelFormulaProvider.BetaDist));
            result.Add((betaDist2019, new BetaDistExcelFunction()));
            string betaDist2007 = mapper.GetFormulaName(ExcelVersion.V2007, nameof(IExcelFormulaProvider.BetaDist));
            result.Add((betaDist2007, new BetaDistExcel2007Function()));

            // FREQUENCY name does not change.
            string frequency = mapper.GetFormulaName(ExcelVersion.V2019, nameof(IExcelFormulaProvider.Frequency));
            result.Add((frequency, new FrequencyExcelFunction()));

            return result;
        }
    }
}

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Acolyte.Assertions;
using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Excel.Formulas
{
    internal sealed class ExcelFormulaNamesMapper
    {
        private readonly IReadOnlyDictionary<(ExcelVersion ExcelVersion, string MethodName), string> _storage;

        public ExcelFormulaNamesMapper()
        {
            _storage = CreateFormulaNamesMapping();
        }

        public string GetFormulaName(ExcelVersion excelVersion,
            [CallerMemberName] string methodName = "")
        {
            excelVersion.ThrowIfEnumValueIsUndefined(nameof(excelVersion));
            methodName.ThrowIfNullOrWhiteSpace(nameof(methodName));

            if (_storage.TryGetValue((excelVersion, methodName), out string formulaName))
            {
                return formulaName;
            }

            string message =
                "Failed to find formula name for parameters:" +
                $"Excel version: '{excelVersion.ToString()}', method name: '{methodName}'.";
            throw new KeyNotFoundException(message);
        }

        private static IReadOnlyDictionary<(ExcelVersion ExcelVersion, string MethodName), string>
            CreateFormulaNamesMapping()
        {
            // Dictionary items format:
            // { (Excel version, name of method), EXCEL_FUNCTION }
            return new Dictionary<(ExcelVersion ExcelVersion, string MethodName), string>
            {
                { (ExcelVersion.V2007, nameof(IExcelFormulaProvider.Average)), "AVERAGE" },
                { (ExcelVersion.V2019, nameof(IExcelFormulaProvider.Average)), "AVERAGE" },

                { (ExcelVersion.V2007, nameof(IExcelFormulaProvider.BetaDist)), "BETADIST" },
                { (ExcelVersion.V2019, nameof(IExcelFormulaProvider.BetaDist)), "BETA.DIST" },

                { (ExcelVersion.V2007, nameof(IExcelFormulaProvider.ChiInv)), "CHIINV" },
                { (ExcelVersion.V2019, nameof(IExcelFormulaProvider.ChiInv)), "CHISQ.INV.RT" },

                { (ExcelVersion.V2007, nameof(IExcelFormulaProvider.ChiTest)), "CHITEST" },
                { (ExcelVersion.V2019, nameof(IExcelFormulaProvider.ChiTest)), "CHISQ.TEST" },

                { (ExcelVersion.V2007, nameof(IExcelFormulaProvider.CountIfS)), "COUNTIFS" },
                { (ExcelVersion.V2019, nameof(IExcelFormulaProvider.CountIfS)), "COUNTIFS" },

                { (ExcelVersion.V2007, nameof(IExcelFormulaProvider.Frequency)), "FREQUENCY" },
                { (ExcelVersion.V2019, nameof(IExcelFormulaProvider.Frequency)), "FREQUENCY" },

                { (ExcelVersion.V2007, nameof(IExcelFormulaProvider.Max)), "MAX" },
                { (ExcelVersion.V2019, nameof(IExcelFormulaProvider.Max)), "MAX" },

                { (ExcelVersion.V2007, nameof(IExcelFormulaProvider.Min)), "MIN" },
                { (ExcelVersion.V2019, nameof(IExcelFormulaProvider.Min)), "MIN" },

                { (ExcelVersion.V2007, nameof(IExcelFormulaProvider.RoundUp)), "ROUNDUP" },
                { (ExcelVersion.V2019, nameof(IExcelFormulaProvider.RoundUp)), "ROUNDUP" },

                { (ExcelVersion.V2007, nameof(IExcelFormulaProvider.StdDev)), "STDEV" },
                { (ExcelVersion.V2019, nameof(IExcelFormulaProvider.StdDev)), "STDEV.S" },

                { (ExcelVersion.V2007, nameof(IExcelFormulaProvider.Sum)), "SUM" },
                { (ExcelVersion.V2019, nameof(IExcelFormulaProvider.Sum)), "SUM" },

                { (ExcelVersion.V2007, nameof(IExcelFormulaProvider.Var)), "VAR" },
                { (ExcelVersion.V2019, nameof(IExcelFormulaProvider.Var)), "VAR.S" },
            };
        }
    }
}

using System.Collections.Generic;
using AlgorithmAnalysis.Excel.Interop;
using OfficeOpenXml.FormulaParsing;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;

namespace AlgorithmAnalysis.Excel.EPPlus.Functions
{
    internal sealed class BetaDistExcelFunction : ExcelFunction
    {
        private const int ExpectedArgumentsNumber = 4;


        public BetaDistExcelFunction()
        {
        }

        #region ExcelFunction Overridden Methods

        public override CompileResult Execute(IEnumerable<FunctionArgument> arguments,
            ParsingContext context)
        {
            // Sanity check, will set excel VALUE error if min length is not met.
            ValidateArguments(arguments, ExpectedArgumentsNumber);

            // Do the work.
            double x = ArgToDecimal(arguments, 0);
            double alpha = ArgToDecimal(arguments, 1);
            double beta = ArgToDecimal(arguments, 2);
            bool cumulative = ArgToBool(arguments, 3);
            double result = ExcelApplication.Instance.WorksheetFunction.Beta_Dist(
                x, alpha, beta, cumulative
            );

            // Return the result.
            return CreateResult(result, DataType.Decimal);
        }

        #endregion
    }
}

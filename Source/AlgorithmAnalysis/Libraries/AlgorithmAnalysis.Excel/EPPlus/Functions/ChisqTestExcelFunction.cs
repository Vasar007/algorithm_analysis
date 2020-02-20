using System.Collections.Generic;
using System.Linq;
using AlgorithmAnalysis.Excel.Interop;
using OfficeOpenXml.FormulaParsing;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;

namespace AlgorithmAnalysis.Excel.EPPlus.Functions
{
    internal sealed class ChisqTestExcelFunction : ExcelFunction
    {
        private const int ExpectedArgumentsNumber = 2;


        public ChisqTestExcelFunction()
        {
        }

        #region ExcelFunction Overridden Methods

        public override CompileResult Execute(IEnumerable<FunctionArgument> arguments,
            ParsingContext context)
        {
            // Sanity check, will set excel VALUE error if min length is not met.
            ValidateArguments(arguments, ExpectedArgumentsNumber);

            var actualRange = ArgToRangeInfo(arguments, 0).Select(cell => cell.Value).ToArray();
            var expectedRange = ArgToRangeInfo(arguments, 1).Select(cell => cell.Value).ToArray();

            // Do the work.
            double result = ExcelApplication.Instance.WorksheetFunction.ChiSq_Test(
                actualRange, expectedRange
            );

            // Return the result.
            return CreateResult(result, DataType.Decimal);
        }

        #endregion
    }
}

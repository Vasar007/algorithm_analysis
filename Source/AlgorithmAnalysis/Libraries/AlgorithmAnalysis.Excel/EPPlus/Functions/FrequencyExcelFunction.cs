using System.Collections.Generic;
using System.Linq;
using AlgorithmAnalysis.Excel.Interop;
using OfficeOpenXml.FormulaParsing;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;

namespace AlgorithmAnalysis.Excel.EPPlus.Functions
{
    internal sealed class FrequencyExcelFunction : ExcelFunction
    {
        private const int ExpectedArgumentsNumber = 2;


        public FrequencyExcelFunction()
        {
        }

        #region ExcelFunction Overridden Methods

        public override CompileResult Execute(IEnumerable<FunctionArgument> arguments,
            ParsingContext context)
        {
            // Sanity check, will set excel VALUE error if min length is not met.
            ValidateArguments(arguments, ExpectedArgumentsNumber);

            // Do the work.
            var dataArray = ArgToRangeInfo(arguments, 0).Select(cell => cell.Value).ToArray();
            var binsArray = ArgToRangeInfo(arguments, 1).Select(cell => cell.Value).ToArray();
            object result = ExcelApplication.Instance.WorksheetFunction.Frequency(
                dataArray, binsArray
            );

            // Return the result.
            return CreateResult(result, DataType.Enumerable);
        }

        #endregion
    }
}

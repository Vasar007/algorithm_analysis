using System.Collections.Generic;
using System.Linq;
using AlgorithmAnalysis.Excel.Interop;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;

namespace AlgorithmAnalysis.Excel.EPPlus.Functions
{
    internal sealed class ChisqInvRtExcelFunction : ExcelFunction
    {
        private const int ExpectedArgumentsNumber = 2;


        public ChisqInvRtExcelFunction()
        {
        }

        #region ExcelFunction Overridden Methods

        public override CompileResult Execute(IEnumerable<FunctionArgument> arguments,
            ParsingContext context)
        {
            // Sanity check, will set excel VALUE error if min length is not met.
            ValidateArguments(arguments, ExpectedArgumentsNumber);

            IReadOnlyList<ExcelDoubleCellValue> numbers = ArgsToDoubleEnumerable(arguments, context)
                .ToList();

            if (numbers.Count != ExpectedArgumentsNumber)
            {
                return CreateResult(ExcelErrorValue.Values.Value, DataType.ExcelError);
            }

            // Do the work.
            double probability = numbers[0].Value;
            double degreeFreedom = numbers[1].Value;
            double result = ExcelApplication.Instance.WorksheetFunction.ChiSq_Inv_RT(
                probability, degreeFreedom
            );

            // Return the result.
            return CreateResult(result, DataType.Decimal);
        }

        #endregion
    }
}

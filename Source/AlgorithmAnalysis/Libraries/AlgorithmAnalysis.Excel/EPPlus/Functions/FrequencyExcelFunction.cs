using System;
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

            double[] convertedResult = ConvertResult(result);

            // Return the result.
            return CreateResult(convertedResult, DataType.Enumerable);
        }

        #endregion

        private double[] ConvertResult(object result)
        {
            // Excel returns two-dimensional array. However, it is a set of double arrays
            // where every array has exactly one double value.

            if (!(result is object[,] converted)) return Array.Empty<double>();

            var numbers = new List<double>(converted.Length);
            foreach (object item in converted)
            {
                if (item is double number)
                {
                    numbers.Add(number);
                }
            }

            return numbers.ToArray();
        }
    }
}

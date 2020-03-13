using System;
using Acolyte.Common;

namespace AlgorithmAnalysis.Math.Functions
{
    /// <summary>
    /// Least-Squares fitting the points (x, y) to a line
    /// y : x -> a + b * x.
    /// </summary>
    internal sealed class LineFunction : BaseModelledFunction
    {
        public override string FormulaFormat { get; }


        public LineFunction(Func<double, double> function, Tuple<double, double> parameters)
            : base(FunctionType.Line, function, parameters.ToReadOnlyList())
        {
            FormulaFormat = $"{Parameters[0]} + {Parameters[1]} * {{0}}";
        }
    }
}

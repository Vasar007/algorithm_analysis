using System;
using Acolyte.Common;

namespace AlgorithmAnalysis.Math.Functions
{
    /// <summary>
    /// Least-Squares fitting the points (x, y) to a logarithm
    /// y : x -> a + b * ln(x).
    /// </summary>
    internal sealed class LogarithmFunction : BaseModelledFunction
    {
        public override string FormulaFormat { get; }


        public LogarithmFunction(Func<double, double> function, Tuple<double, double> parameters)
            : base(FunctionType.Logarithm, function, parameters.ToReadOnlyList())
        {
            FormulaFormat = $"{Parameters[0]} + {Parameters[1]} * {{1}}({{0}})";
        }
    }
}

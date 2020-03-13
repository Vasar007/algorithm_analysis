using System;
using Acolyte.Common;

namespace AlgorithmAnalysis.Math.Functions
{
    /// <summary>
    /// Least-Squares fitting the points (x, y) to an exponential
    /// y : x -> a * exp(r * x).
    /// </summary>
    internal sealed class ExponentialFunction : BaseModelledFunction
    {
        public override string FormulaFormat { get; }


        public ExponentialFunction(Func<double, double> function, Tuple<double, double> parameters)
            : base(FunctionType.Exponential, function, parameters.ToReadOnlyList())
        {
            FormulaFormat = $"{Parameters[0]} * {{1}}({Parameters[1]} * {{0}})";
        }
    }
}

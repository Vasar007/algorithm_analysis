using System;
using Acolyte.Common;

namespace AlgorithmAnalysis.Math.Functions
{
    /// <summary>
    /// Least-Squares fitting the points (x, y) to a power
    /// y : x -> a * x^b.
    /// </summary>
    internal sealed class PowerFunction : BaseModelledFunction
    {
        public override string FormulaFormat { get; }


        public PowerFunction(Func<double, double> function, Tuple<double, double> parameters)
            : base(FunctionType.Power, function, parameters.ToReadOnlyList())
        {
            FormulaFormat = $"{Parameters[0]} * {{0}}^{Parameters[1]}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AlgorithmAnalysis.Math.Functions
{
    /// <summary>
    /// Least-Squares fitting the points (x, y) to a k-order polynomial
    /// y : x -> p0 + p1 * x + p2 * x^2 + ... + pk * x^k.
    /// </summary>
    internal sealed class PolynomialFunction : BaseModelledFunction
    {
        public override string FormulaFormat { get; }


        public PolynomialFunction(Func<double, double> function, double[] parameters)
            : base(FunctionType.Polynomial, function, parameters)
        {
            FormulaFormat = CreateFormulaFormat(Parameters);
        }

        private static string CreateFormulaFormat(IReadOnlyList<double> parameters)
        {
            var sb = new StringBuilder(capacity: 32);

            int order = parameters.Count;
            for (int power = 0; power < order; ++power)
            {
                string parameter = parameters[power].ToString();

                if (power == 0)
                {
                   sb.Append(parameter);
                }
                else
                {
                    sb.Append($" + {parameter} * {{0}}^{power.ToString()}");
                }
            }

            return sb.ToString();
        }
    }
}

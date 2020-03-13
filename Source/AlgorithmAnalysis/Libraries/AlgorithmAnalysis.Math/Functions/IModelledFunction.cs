using System;
using System.Collections.Generic;

namespace AlgorithmAnalysis.Math.Functions
{
    public interface IModelledFunction
    {
        public FunctionType Type { get; }

        Func<double, double> Function { get; }

        IReadOnlyList<double> Parameters { get; }

        string FormulaFormat { get; }


        double Calculate(double value);

        IEnumerable<double> Calculate(IEnumerable<double> values);

        string ToFormulaString(params string[] args);
    }
}

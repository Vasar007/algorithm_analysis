using System;
using System.Collections.Generic;
using System.Linq;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Math.Functions
{
    internal abstract class BaseModelledFunction : IModelledFunction
    {
        public FunctionType Type { get; }

        public Func<double, double> Function { get; }

        public IReadOnlyList<double> Parameters { get; }

        public abstract string FormulaFormat { get; }


        protected BaseModelledFunction(FunctionType functionType, Func<double, double> function,
            IReadOnlyList<double> parameters)
        {
            Type = functionType.ThrowIfEnumValueIsUndefined(nameof(functionType));
            Function = function.ThrowIfNull(nameof(function));
            Parameters = parameters.ThrowIfNull(nameof(parameters));
        }

        public double Calculate(double value)
        {
            return Function(value);
        }

        public IEnumerable<double> Calculate(IEnumerable<double> values)
        {
            values.ThrowIfNullOrEmpty(nameof(values));

            return values.Select(Calculate);
        }

        public string ToFormulaString(params string[] args)
        {
            args.ThrowIfNull(nameof(args));

            return string.Format(FormulaFormat, args);
        }
    }
}

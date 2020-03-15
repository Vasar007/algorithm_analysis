using System.Linq;
using System.Collections.Generic;
using Acolyte.Assertions;
using Acolyte.Collections;
using MathNet.Numerics;
using AlgorithmAnalysis.Math.Functions;

namespace AlgorithmAnalysis.Math.Selectors
{
    public sealed class FunctionSelectorBasedOnRSquaredValue : IFunctionSelector
    {
        public FunctionSelectorBasedOnRSquaredValue()
        {
        }

        #region IFunctionSelector Implementation

        public IModelledFunction SelectBest(IEnumerable<IModelledFunction> modelledFunctions,
            IEnumerable<double> observedValues)
        {
            modelledFunctions.ThrowIfNullOrEmpty(nameof(modelledFunctions));
            observedValues.ThrowIfNullOrEmpty(nameof(observedValues));

            IReadOnlyList<double> enumeratedValues = observedValues.ToReadOnlyList();

            return modelledFunctions
                .Select(modelledFunction => CalculateRSquaredValue(modelledFunction, enumeratedValues))
                .MaxBy(result => result.rSquaredValue)
                .modelledFunction;
        }

        #endregion

        private static (IModelledFunction modelledFunction, double rSquaredValue)
            CalculateRSquaredValue(IModelledFunction modelledFunction,
            IEnumerable<double> observedValues)
        {
            double rSquaredValue = GoodnessOfFit.RSquared(
                observedValues, modelledFunction.Calculate(observedValues)
            );

            return (modelledFunction, rSquaredValue);
        }
    }
}

using System.Linq;
using System.Collections.Generic;
using Acolyte.Assertions;
using Acolyte.Collections;
using MathNet.Numerics;
using AlgorithmAnalysis.Math.Functions;

namespace AlgorithmAnalysis.Math.Selectors
{
    internal sealed class FunctionSelectorBasedOnCoefficientOfDetermination : IFunctionSelector
    {
        public FunctionSelectorBasedOnCoefficientOfDetermination()
        {
        }

        #region IFunctionSelector Implementation

        public IModelledFunction SelectBest(IEnumerable<IModelledFunction> modelledFunctions,
            IEnumerable<double> observedValues)
        {
            IReadOnlyList<double> enumeratedValues = observedValues.ToReadOnlyList();

            return modelledFunctions
                .Select(modelledFunction => CalculateCoefficientOfDetermination(modelledFunction, enumeratedValues))
                .MaxBy(result => result.coefficientOfDetermination)
                .modelledFunction;
        }

        #endregion

        private static (IModelledFunction modelledFunction, double coefficientOfDetermination)
            CalculateCoefficientOfDetermination(IModelledFunction modelledFunction,
            IEnumerable<double> observedValues)
        {
            double coefficientOfDetermination = GoodnessOfFit.CoefficientOfDetermination(
                modelledFunction.Calculate(observedValues), observedValues
            );

            return (modelledFunction, coefficientOfDetermination);
        }
    }
}

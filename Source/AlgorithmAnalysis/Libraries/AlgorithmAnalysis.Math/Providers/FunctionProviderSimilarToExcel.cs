using System.Collections.Generic;
using System.Linq;
using AlgorithmAnalysis.Math.Functions;

namespace AlgorithmAnalysis.Math.Providers
{
    internal sealed class FunctionProviderSimilarToExcel : IFunctionProvider
    {
        // TODO: add config parameter to specify several polinomial functions with
        // different order values.

        public FunctionProviderSimilarToExcel()
        {
        }

        #region IFunctionProvider Implementation

        public IEnumerable<IModelledFunction> Provide(IEnumerable<double> xValues,
            IEnumerable<double> yValues)
        {
            double[] xArray = xValues.ToArray();
            double[] yArray = yValues.ToArray();

            return new[]
            {
                FitDecorator.ExponentialFunc(xArray, yArray),
                FitDecorator.LineFunc(xArray, yArray),
                FitDecorator.LogarithmFunc(xArray, yArray),
                FitDecorator.PolynomialFunc(xArray, yArray, order: 2),
                FitDecorator.PowerFunc(xArray, yArray)
            };
        }

        #endregion
    }
}

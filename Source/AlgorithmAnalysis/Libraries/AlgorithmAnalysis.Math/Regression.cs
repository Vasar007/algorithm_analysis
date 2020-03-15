using System;
using System.Collections.Generic;
using System.Linq;
using Acolyte.Assertions;
using AlgorithmAnalysis.Logging;
using AlgorithmAnalysis.Math.Functions;
using AlgorithmAnalysis.Math.Providers;
using AlgorithmAnalysis.Math.Selectors;

namespace AlgorithmAnalysis.Math
{
    public sealed class Regression : IRegression
    {
        private static readonly ILogger _logger = LoggerFactory.CreateLoggerFor<Regression>();

        private readonly IFunctionProvider _functionProvider;

        private readonly IFunctionSelector _functionSelector;


        public Regression(IFunctionProvider functionProvider, IFunctionSelector functionSelector)
        {
            _functionProvider = functionProvider.ThrowIfNull(nameof(functionProvider));
            _functionSelector = functionSelector.ThrowIfNull(nameof(functionSelector));
        }

        public static Regression CreateDefault()
        {
            return new Regression(
                functionProvider: new FunctionProviderSimilarToExcel(),
                functionSelector: new FunctionSelectorBasedOnCoefficientOfDetermination()
            );
        }

        // TODO: allow to specify function provider too (need to add new option in UI and context).
        public static Regression Create(IFunctionSelector goodnessOfFit)
        {
            return new Regression(
                functionProvider: new FunctionProviderSimilarToExcel(),
                functionSelector: goodnessOfFit
            );
        }

        #region IRegression Implementation

        public IModelledFunction Fit(IEnumerable<double> xValues, IEnumerable<double> yValues)
        {
            xValues.ThrowIfNullOrEmpty(nameof(xValues));
            yValues.ThrowIfNullOrEmpty(nameof(yValues));

            _logger.Info("Finding the best function for observed values.");

            IEnumerable<IModelledFunction> functions = _functionProvider.Provide(xValues, yValues);
            if (!functions.Any())
            {
                throw new InvalidOperationException("No functions were provided to select.");
            }

            return _functionSelector.SelectBest(functions, yValues);
        }

        #endregion
    }
}

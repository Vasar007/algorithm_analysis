using System.Collections.Generic;
using AlgorithmAnalysis.Math.Functions;

namespace AlgorithmAnalysis.Math.Selectors
{
    public interface IFunctionSelector
    {
        IModelledFunction SelectBest(IEnumerable<IModelledFunction> modelledFunctions,
            IEnumerable<double> observedValues);
    }
}

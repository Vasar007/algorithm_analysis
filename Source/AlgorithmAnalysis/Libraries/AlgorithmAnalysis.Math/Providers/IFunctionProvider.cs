using System.Collections.Generic;
using AlgorithmAnalysis.Math.Functions;

namespace AlgorithmAnalysis.Math.Providers
{
    public interface IFunctionProvider
    {
        IEnumerable<IModelledFunction> Provide(IEnumerable<double> xValues,
            IEnumerable<double> yValues);
    }
}

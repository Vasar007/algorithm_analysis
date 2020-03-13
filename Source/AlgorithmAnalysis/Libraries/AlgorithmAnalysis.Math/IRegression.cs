using System.Collections.Generic;
using AlgorithmAnalysis.Math.Functions;

namespace AlgorithmAnalysis.Math
{
    public interface IRegression
    {
        IModelledFunction Fit(IEnumerable<double> xValues, IEnumerable<double> yValues);
    }
}

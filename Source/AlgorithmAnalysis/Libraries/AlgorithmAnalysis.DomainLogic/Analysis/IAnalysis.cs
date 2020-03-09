using System.Threading.Tasks;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    internal interface IAnalysis
    {
        Task<AnalysisResult> AnalyzeAsync(AnalysisContext context);
    }
}

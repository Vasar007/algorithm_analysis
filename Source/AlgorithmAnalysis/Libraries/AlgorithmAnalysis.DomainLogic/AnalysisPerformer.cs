using System.Collections.Generic;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Analysis;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class AnalysisPerformer
    {
        private readonly IReadOnlyList<IAnalysis> _analyses;


        public AnalysisPerformer(string outputExcelFilename)
        {
            _analyses = ConstructAnalysis(outputExcelFilename);
        }

        public void PerformAnalysis(AnalysisContext context)
        {
            context.ThrowIfNull(nameof(context));

            foreach (IAnalysis analysis in _analyses)
            {
                analysis.Analyze(context);
            }
        }

        private static IReadOnlyList<IAnalysis> ConstructAnalysis(string outputExcelFilename)
        {
            return new List<IAnalysis>
            {
                new AnalysisPhaseOne(outputExcelFilename),
                new AnalysisPhaseTwo()
            };
        }
    }
}

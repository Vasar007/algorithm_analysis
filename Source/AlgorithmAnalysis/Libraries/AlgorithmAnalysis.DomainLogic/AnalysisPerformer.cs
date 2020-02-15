using System;
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

            // TODO: remove when implement algorithm min/average/max formulas.
            if (context.Args.AlgorithmType.Value > 0)
            {
                throw new NotImplementedException("Library can work with only one algorithm type.");
            }

            foreach (IAnalysis analysis in _analyses)
            {
                AnalysisResult result = analysis.Analyze(context);

                // TODO: return progress statuses with messages.
                if (!result.Success) break;
            }
        }

        private static IReadOnlyList<IAnalysis> ConstructAnalysis(string outputExcelFilename)
        {
            return new List<IAnalysis>
            {
                new AnalysisPhaseOne(outputExcelFilename),
                new AnalysisPhaseTwo(outputExcelFilename)
            };
        }
    }
}

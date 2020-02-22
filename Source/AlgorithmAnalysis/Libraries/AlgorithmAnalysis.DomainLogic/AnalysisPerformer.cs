using System;
using System.Collections.Generic;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Analysis;
using AlgorithmAnalysis.Logging;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class AnalysisPerformer
    {
        private static readonly ILogger _logger =
            LoggerFactory.CreateLoggerFor<AnalysisPerformer>();

        private readonly IReadOnlyList<IAnalysis> _analyses;


        public AnalysisPerformer(string outputExcelFilename)
        {
            _analyses = ConstructAnalysis(outputExcelFilename);
        }

        public AnalysisResult PerformAnalysis(AnalysisContext context)
        {
            context.ThrowIfNull(nameof(context));

            // TODO: remove when implement algorithm min/average/max formulas.
            if (context.Args.AlgorithmType.Value > 0)
            {
                throw new NotImplementedException("Library can work with only one algorithm type.");
            }

            try
            {
                return PerformInternal(context);
            }
            catch (Exception ex)
            {
                string message = $"Analysis failed: {ex.Message}";

                _logger.Error(ex, message);
                return AnalysisResult.CreateFailure(message);
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

        private AnalysisResult PerformInternal(AnalysisContext context)
        {
            foreach (IAnalysis analysis in _analyses)
            {
                AnalysisResult result = analysis.Analyze(context);

                // TODO: return progress statuses with messages.
                if (!result.Success) return result;
            }

            return AnalysisResult.CreateSuccess("Analysis finished successfully.");
        }
    }
}

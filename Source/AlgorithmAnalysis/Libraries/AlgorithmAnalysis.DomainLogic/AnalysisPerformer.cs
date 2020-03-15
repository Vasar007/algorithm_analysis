using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common.Files;
using AlgorithmAnalysis.DomainLogic.Analysis;
using AlgorithmAnalysis.Logging;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class AnalysisPerformer
    {
        private static readonly ILogger _logger =
            LoggerFactory.CreateLoggerFor<AnalysisPerformer>();

        private readonly IReadOnlyList<IAnalysis> _analyses;


        public AnalysisPerformer()
        {
            _analyses = ConstructAnalysis(new LocalFileWorker());
        }

        public async Task<AnalysisResult> PerformAnalysisAsync(AnalysisContext context)
        {
            context.ThrowIfNull(nameof(context));

            _logger.Info("Starting algorithm analysis.");
            _logger.Info($"Parameters: {context.ToLogString()}");

            // TODO: remove when implement algorithm min/average/max formulas.
            if (context.Args.AlgorithmType.Value != 0)
            {
                throw new NotImplementedException("Library can work with only one algorithm type.");
            }

            try
            {
                AnalysisResult result = await PerformInternalAsync(context);
                _logger.Info($"Analysis completely finished. Final result: {result.ToLogString()}");
                return result;
            }
            catch (Exception ex)
            {
                string message = $"Analysis failed: {ex.Message}";

                _logger.Error(ex, message);
                return AnalysisResult.CreateFailure(message, context);
            }
        }

        private static IReadOnlyList<IAnalysis> ConstructAnalysis(LocalFileWorker fileWorker)
        {
            return new List<IAnalysis>
            {
                new AnalysisPhaseOne(fileWorker),
                new AnalysisPhaseTwo(fileWorker)
            };
        }

        private async Task<AnalysisResult> PerformInternalAsync(AnalysisContext context)
        {
            _logger.Info("Performing all analysis sequentially.");

            AnalysisResult result = AnalysisResult.CreateDefault(context);
            foreach (IAnalysis analysis in _analyses)
            {
                result = await analysis.AnalyzeAsync(result.Context);
                _logger.Info($"Analysis result: {result.ToLogString()}");

                // TODO: return all progress statuses with messages.
                if (!result.Success)
                {
                    _logger.Warn($"Analysis failed. Stopping analysis and return last result.");
                    return result;
                }
            }

            if (result is null)
            {
                throw new InvalidOperationException("Failed to perform analysis.");
            }

            _logger.Info($"All analysis completed successfully.");
            return result;
        }
    }
}

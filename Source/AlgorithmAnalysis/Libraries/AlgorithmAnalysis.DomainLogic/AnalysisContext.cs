using System;
using AlgorithmAnalysis.DomainLogic.Excel;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class AnalysisContext
    {
        public ParametersPack Args { get; }

        public PhaseOnePartOneAnalysisKind AnalysisKind { get; }


        public AnalysisContext(ParametersPack args, PhaseOnePartOneAnalysisKind analysisKind)
        {
            Args = args.ThrowIfNull(nameof(args));
            AnalysisKind = analysisKind.ThrowIfEnumValueIsUndefined(nameof(analysisKind));
        }

        internal IPhaseOnePartOneAnalysis CreatePhaseOnePartOneAnalysis(ExcelSheet sheet)
        {
            return AnalysisKind switch
            {
                PhaseOnePartOneAnalysisKind.NormalDistribution =>
                    new NormalDistributionAnalysis(sheet, Args),

                PhaseOnePartOneAnalysisKind.BetaDistribution =>
                    throw new NotImplementedException(
                        "Beta distribution analysis for phase one part one is not implemented."
                    ),
                
                _ => throw new ArgumentOutOfRangeException(
                         nameof(AnalysisKind),
                         AnalysisKind,
                         $"Unknown analysis kind value: '{AnalysisKind.ToString()}'."
                     )
            };
        }
    }
}

using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelContextForPhaseOne<TAnalysisPhaseOne>
        where TAnalysisPhaseOne : IAnalysisPhaseOne
    {
        public delegate TAnalysisPhaseOne AnalysisCreation(ParametersPack args);

        private readonly AnalysisCreation _partOneFactory;

        public ParametersPack Args { get; }

        public bool ShowAnalysisWindow { get; }

        public string SheetName { get; }


        private ExcelContextForPhaseOne(
            ParametersPack args,
            bool showAnalysisWindow,
            string sheetName,
            AnalysisCreation analysisFactory)
        {
            Args = args.ThrowIfNull(nameof(args));
            ShowAnalysisWindow = showAnalysisWindow;
            _partOneFactory = analysisFactory.ThrowIfNull(nameof(analysisFactory));
            SheetName = sheetName.ThrowIfNullOrEmpty(nameof(sheetName));
        }

        public static ExcelContextForPhaseOne<TAnalysisPhaseOne> CreateFor(
            ParametersPack args,
            bool showAnalysisWindow,
            string sheetName,
            AnalysisCreation analysisFactory)
        {
            return new ExcelContextForPhaseOne<TAnalysisPhaseOne>(
                args: args,
                showAnalysisWindow: showAnalysisWindow,
                sheetName: sheetName,
                analysisFactory: analysisFactory
            );
        }

        public TAnalysisPhaseOne CreatePartNAnalysis()
        {
            return _partOneFactory(Args);
        }
    }
}

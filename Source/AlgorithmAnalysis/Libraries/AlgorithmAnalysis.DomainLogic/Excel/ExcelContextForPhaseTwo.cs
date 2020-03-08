using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseTwo;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelContextForPhaseTwo<TAnalysisPhaseTwo>
        where TAnalysisPhaseTwo : IAnalysisPhaseTwo
    {
        public delegate TAnalysisPhaseTwo AnalysisCreation(ParametersPack args);

        private readonly AnalysisCreation _partTwoFactory;

        public ParametersPack Args { get; }

        public bool ShowAnalysisWindow { get; }

        public string SheetName { get; }


        private ExcelContextForPhaseTwo(
            ParametersPack args,
            bool showAnalysisWindow,
            string sheetName,
            AnalysisCreation analysisFactory)
        {
            Args = args.ThrowIfNull(nameof(args));
            ShowAnalysisWindow = showAnalysisWindow;
            _partTwoFactory = analysisFactory.ThrowIfNull(nameof(analysisFactory));
            SheetName = sheetName.ThrowIfNullOrEmpty(nameof(sheetName));
        }

        public static ExcelContextForPhaseTwo<TAnalysisPhaseTwo> CreateFor(
            ParametersPack args,
            bool showAnalysisWindow,
            string sheetName,
            AnalysisCreation analysisFactory)
        {
            return new ExcelContextForPhaseTwo<TAnalysisPhaseTwo>(
                args: args,
                showAnalysisWindow: showAnalysisWindow,
                sheetName: sheetName,
                analysisFactory: analysisFactory
            );
        }

        public TAnalysisPhaseTwo CreateAnalysis()
        {
            return _partTwoFactory(Args);
        }
    }
}

using System.IO;
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

        public FileInfo OutputExcelFile { get; }

        public string SheetName { get; }


        private ExcelContextForPhaseOne(
            ParametersPack args,
            bool showAnalysisWindow,
            FileInfo outputExcelFile,
            string sheetName,
            AnalysisCreation analysisFactory)
        {
            Args = args.ThrowIfNull(nameof(args));
            ShowAnalysisWindow = showAnalysisWindow;
            OutputExcelFile = outputExcelFile.ThrowIfNull(nameof(outputExcelFile));
            SheetName = sheetName.ThrowIfNullOrEmpty(nameof(sheetName));
            _partOneFactory = analysisFactory.ThrowIfNull(nameof(analysisFactory));
        }

        public static ExcelContextForPhaseOne<TAnalysisPhaseOne> CreateFor(
             ParametersPack args,
            bool showAnalysisWindow,
            FileInfo outputExcelFile,
            string sheetName,
            AnalysisCreation analysisFactory)
        {
            return new ExcelContextForPhaseOne<TAnalysisPhaseOne>(
                args: args,
                showAnalysisWindow: showAnalysisWindow,
                outputExcelFile: outputExcelFile,
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

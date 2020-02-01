using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelContextForPhaseOne
    {
        public delegate IAnalysisPhaseOnePartOne CreateAnalysisPhaseOnePartOne(ExcelSheet sheet);

        public ParametersPack Args { get; }

        public bool ShowAnalysisWindow { get; }

        public CreateAnalysisPhaseOnePartOne AnalysisFactory { get; }
        
        public string SheetName { get; }


        public ExcelContextForPhaseOne(
            ParametersPack args,
            bool showAnalysisWindow,
            CreateAnalysisPhaseOnePartOne analysisFactory,
            string sheetName)
        {
            Args = args.ThrowIfNull(nameof(args));
            ShowAnalysisWindow = showAnalysisWindow;
            AnalysisFactory = analysisFactory.ThrowIfNull(nameof(analysisFactory));
            SheetName = sheetName.ThrowIfNullOrEmpty(nameof(sheetName));
        }
    }
}

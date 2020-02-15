using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelContextForPhaseOne
    {
        public delegate IAnalysisPhaseOnePartOne AnalysisPartOneCreation(ParametersPack args);
        public delegate IAnalysisPhaseOnePartTwo AnalysisPartTwoCreation(ParametersPack args);

        private readonly AnalysisPartOneCreation _partOneFactory;

        private readonly AnalysisPartTwoCreation _partTwoFactory;

        public ParametersPack Args { get; }

        public bool ShowAnalysisWindow { get; }

        public string SheetName { get; }


        public ExcelContextForPhaseOne(
            ParametersPack args,
            bool showAnalysisWindow,
            string sheetName,
            AnalysisPartOneCreation partOneFactory,
            AnalysisPartTwoCreation partTwoFactory)
        {
            Args = args.ThrowIfNull(nameof(args));
            ShowAnalysisWindow = showAnalysisWindow;
            _partOneFactory = partOneFactory.ThrowIfNull(nameof(partOneFactory));
            _partTwoFactory = partTwoFactory.ThrowIfNull(nameof(partTwoFactory));
            SheetName = sheetName.ThrowIfNullOrEmpty(nameof(sheetName));
        }

        public static ExcelContextForPhaseOne CreateWithoutFactories(
            ParametersPack args,
            bool showAnalysisWindow,
            string sheetName)
        {
            return new ExcelContextForPhaseOne(
                args: args,
                showAnalysisWindow: showAnalysisWindow,
                sheetName: sheetName,
                partOneFactory: args => DummyAnalysisPhaseOnePartOne.Create(),
                partTwoFactory: args => DummyAnalysisPhaseOnePartTwo.Create()
            );
        }

        public static ExcelContextForPhaseOne CreateForPartOne(
            ParametersPack args,
            bool showAnalysisWindow,
            string sheetName,
            AnalysisPartOneCreation partOneFactory)
        {
            return new ExcelContextForPhaseOne(
                args: args,
                showAnalysisWindow: showAnalysisWindow,
                sheetName: sheetName,
                partOneFactory: partOneFactory,
                partTwoFactory: args => DummyAnalysisPhaseOnePartTwo.Create()
            );
        }

        public static ExcelContextForPhaseOne CreateForPartTwo(
            ParametersPack args,
            bool showAnalysisWindow,
            string sheetName,
            AnalysisPartTwoCreation partTwoFactory)
        {
            return new ExcelContextForPhaseOne(
                args: args,
                showAnalysisWindow: showAnalysisWindow,
                sheetName: sheetName,
                partOneFactory: args => DummyAnalysisPhaseOnePartOne.Create(),
                partTwoFactory: partTwoFactory
            );
        }

        public IAnalysisPhaseOnePartOne CreatePartOne()
        {
            return _partOneFactory(Args);
        }

        public IAnalysisPhaseOnePartTwo CreatePartTwo()
        {
            return _partTwoFactory(Args);
        }
    }
}

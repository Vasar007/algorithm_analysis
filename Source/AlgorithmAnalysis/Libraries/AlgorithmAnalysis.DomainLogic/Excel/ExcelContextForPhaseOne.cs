using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartOne;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne.PartTwo;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelContextForPhaseOne
    {
        public ParametersPack Args { get; }

        public bool ShowAnalysisWindow { get; }

        public string SheetName { get; }

        public Func<IAnalysisPhaseOnePartOne> PartOneFactory { get; }

        public Func<IAnalysisPhaseOnePartTwo> PartTwoFactory { get; }


        public ExcelContextForPhaseOne(
            ParametersPack args,
            bool showAnalysisWindow,
            string sheetName,
            Func<IAnalysisPhaseOnePartOne> partOneFactory,
            Func<IAnalysisPhaseOnePartTwo> partTwoFactory)
        {
            Args = args.ThrowIfNull(nameof(args));
            ShowAnalysisWindow = showAnalysisWindow;
            PartOneFactory = partOneFactory.ThrowIfNull(nameof(partOneFactory));
            PartTwoFactory = partTwoFactory.ThrowIfNull(nameof(partTwoFactory));
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
                partOneFactory: DummyAnalysisPhaseOnePartOne.Create,
                partTwoFactory: DummyAnalysisPhaseOnePartTwo.Create
            );
        }

        public static ExcelContextForPhaseOne CreateForPartOne(
            ParametersPack args,
            bool showAnalysisWindow,
            string sheetName,
            Func<IAnalysisPhaseOnePartOne> partOneFactory)
        {
            return new ExcelContextForPhaseOne(
                args: args,
                showAnalysisWindow: showAnalysisWindow,
                sheetName: sheetName,
                partOneFactory: partOneFactory,
                partTwoFactory: DummyAnalysisPhaseOnePartTwo.Create
            );
        }

        public static ExcelContextForPhaseOne CreateForPartTwo(
            ParametersPack args,
            bool showAnalysisWindow,
            string sheetName,
            Func<IAnalysisPhaseOnePartTwo> partTwoFactory)
        {
            return new ExcelContextForPhaseOne(
                args: args,
                showAnalysisWindow: showAnalysisWindow,
                sheetName: sheetName,
                partOneFactory: DummyAnalysisPhaseOnePartOne.Create,
                partTwoFactory: partTwoFactory
            );
        }
    }
}

﻿using System.IO;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseTwo;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelContextForPhaseTwo<TAnalysisPhaseTwo>
        where TAnalysisPhaseTwo : IAnalysisPhaseTwo
    {
        public delegate TAnalysisPhaseTwo AnalysisPhaseTwoCreation(ParametersPack args);

        private readonly AnalysisPhaseTwoCreation _partTwoFactory;

        public ParametersPack Args { get; }

        public bool ShowAnalysisWindow { get; }

        public FileInfo OutputExcelFile { get; }

        public string SheetName { get; }


        private ExcelContextForPhaseTwo(
            ParametersPack args,
            bool showAnalysisWindow,
            FileInfo outputExcelFile,
            string sheetName,
            AnalysisPhaseTwoCreation analysisFactory)
        {
            Args = args.ThrowIfNull(nameof(args));
            ShowAnalysisWindow = showAnalysisWindow;
            OutputExcelFile = outputExcelFile.ThrowIfNull(nameof(outputExcelFile));
            SheetName = sheetName.ThrowIfNullOrEmpty(nameof(sheetName));
            _partTwoFactory = analysisFactory.ThrowIfNull(nameof(analysisFactory));
        }

        public static ExcelContextForPhaseTwo<TAnalysisPhaseTwo> CreateFor(
            AnalysisContext analysisContext,
            string sheetName,
            AnalysisPhaseTwoCreation analysisFactory)
        {
            analysisContext.ThrowIfNull(nameof(analysisContext));

            return new ExcelContextForPhaseTwo<TAnalysisPhaseTwo>(
                args: analysisContext.Args,
                showAnalysisWindow: analysisContext.ShowAnalysisWindow,
                outputExcelFile: analysisContext.OutputExcelFile,
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

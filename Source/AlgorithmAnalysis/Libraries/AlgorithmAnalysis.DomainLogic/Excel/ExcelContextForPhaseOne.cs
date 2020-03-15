using System;
using System.IO;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel.Analysis.PhaseOne;

namespace AlgorithmAnalysis.DomainLogic.Excel
{
    internal sealed class ExcelContextForPhaseOne<TAnalysisPhaseOne>
        where TAnalysisPhaseOne : IAnalysisPhaseOne
    {
        private readonly Func<ParametersPack, TAnalysisPhaseOne> _partOneFactory;

        public ParametersPack Args { get; }

        public AnalysisLaunchContext LaunchContext { get; }

        public FileInfo OutputExcelFile { get; }

        public string SheetName { get; }


        private ExcelContextForPhaseOne(
            ParametersPack args,
            AnalysisLaunchContext launchContext,
            FileInfo outputExcelFile,
            string sheetName,
            Func<ParametersPack, TAnalysisPhaseOne> analysisFactory)
        {
            Args = args.ThrowIfNull(nameof(args));
            LaunchContext = launchContext.ThrowIfNull(nameof(launchContext));
            OutputExcelFile = outputExcelFile.ThrowIfNull(nameof(outputExcelFile));
            SheetName = sheetName.ThrowIfNullOrEmpty(nameof(sheetName));
            _partOneFactory = analysisFactory.ThrowIfNull(nameof(analysisFactory));
        }

        public static ExcelContextForPhaseOne<TAnalysisPhaseOne> CreateFor(
             ParametersPack args,
            AnalysisLaunchContext launchContext,
            FileInfo outputExcelFile,
            string sheetName,
            Func<ParametersPack, TAnalysisPhaseOne> analysisFactory)
        {
            return new ExcelContextForPhaseOne<TAnalysisPhaseOne>(
                args: args,
                launchContext: launchContext,
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

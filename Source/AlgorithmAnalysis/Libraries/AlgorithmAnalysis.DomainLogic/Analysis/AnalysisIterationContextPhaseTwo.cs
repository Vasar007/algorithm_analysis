using System.IO;
using Acolyte.Assertions;
using AlgorithmAnalysis.Common.Files;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    internal sealed class AnalysisIterationContextPhaseTwo
    {
        public ParametersPack Args { get; }
        public AnalysisLaunchContext LaunchContext { get; }
        public LocalFileWorker FileWorker { get; }
        public string AnalysisInputArgs { get; }
        public FileInfo FinalOutputFile { get; }


        public AnalysisIterationContextPhaseTwo(
            ParametersPack args,
            AnalysisLaunchContext launchContext,
            LocalFileWorker fileWorker,
            string analysisInputArgs,
            FileInfo finalOutputFile)
        {
            Args = args.ThrowIfNull(nameof(args));
            LaunchContext = launchContext.ThrowIfNull(nameof(launchContext));
            FileWorker = fileWorker.ThrowIfNull(nameof(fileWorker));
            AnalysisInputArgs = analysisInputArgs.ThrowIfNullOrWhiteSpace(nameof(analysisInputArgs));
            FinalOutputFile = finalOutputFile.ThrowIfNull(nameof(finalOutputFile));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using Acolyte.Assertions;
using Acolyte.Threading;
using AlgorithmAnalysis.Common.Files;
using AlgorithmAnalysis.Common.Processes;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    internal static class AnalysisRunner
    {
        public static FileObject PerformOneIterationOfPhaseOne(ParametersPack args,
            AnalysisLaunchContext launchContext, LocalFileWorker fileWorker)
        {
            args.ThrowIfNull(nameof(args));
            launchContext.ThrowIfNull(nameof(launchContext));
            fileWorker.ThrowIfNull(nameof(fileWorker));

            // Contract: output files are located in the same directory as our app.
            IReadOnlyList<FileInfo> finalOutputFiles = args.GetOutputFilenames(phaseNumber: 1);
            CheckExpectedFilenamesNumber(expectedFilesNumber: 2, finalOutputFiles);

            var fileDeleter = new FileDeleter(finalOutputFiles);

            // Contract: the analysis program is located in the same directory as our app.
            var processLaunchContext = ProcessLaunchContext.Create(
                file: args.AnalysisProgramName,
                args: args.PackAsInputArgumentsForPhaseOne(),
                showWindow: launchContext.ShowAnalysisWindow
            );

            using (var analysisRunner = ProgramRunner.RunProgram(processLaunchContext))
            {
                analysisRunner.Wait();
            }

            // The first data file is iteration result, the last is common analysis data file.
            // We don't need to read/use the last one.
            FileInfo finalOutputFile = finalOutputFiles.First();

            DataObject<OutputFileData> data = fileWorker.ReadDataFile(finalOutputFile);

            return new FileObject(fileDeleter, data);
        }

        public async static Task PerformFullAnalysisForPhaseTwoAsync(ParametersPack args,
            AnalysisLaunchContext launchContext, LocalFileWorker fileWorker,
            Action<FileObject> callback)
        {
            args.ThrowIfNull(nameof(args));
            launchContext.ThrowIfNull(nameof(launchContext));
            fileWorker.ThrowIfNull(nameof(fileWorker));
            callback.ThrowIfNull(nameof(callback));

            // Contract: output files are located in the same directory as our app.
            IReadOnlyList<FileInfo> finalOutputFiles = args.GetOutputFilenames(phaseNumber: 2);
            using var fileDeleter = new FileDeleter(finalOutputFiles);

            IReadOnlyList<string> analysisInputArgsCollection =
                args.CollectionPackAsInputArgumentsForPhaseTwo();

            var limitedScheduler =
                new LimitedConcurrencyLevelTaskScheduler(launchContext.MaxDegreeOfParallelism);

            var processingTasks = new List<Task<FileObject>>(analysisInputArgsCollection.Count);

            // The last is common analysis data file.
            // We don't need to read/use the last one.
            for (int index = 0; index < analysisInputArgsCollection.Count; ++index)
            {
                var iterationContext = new AnalysisIterationContextPhaseTwo(
                    args: args,
                    launchContext: launchContext,
                    fileWorker: fileWorker,
                    analysisInputArgs: analysisInputArgsCollection[index],
                    finalOutputFile: finalOutputFiles[index]
                );

                Task<FileObject> processingTask = TaskHelper.StartNew(
                    () => PerformOneIterationOfPhaseTwoAsync(iterationContext),
                    limitedScheduler
                );
                processingTasks.Add(processingTask);
            }

            await Task.WhenAll(processingTasks.Select(task => AwaitAndProcessAsync(task, callback)));
        }

        private static void CheckExpectedFilenamesNumber(int expectedFilesNumber,
            IReadOnlyList<FileInfo> actualOutputFiles)
        {
            expectedFilesNumber.ThrowIfValueIsOutOfRange(nameof(expectedFilesNumber), 1, int.MaxValue);
            actualOutputFiles.ThrowIfNullOrEmpty(nameof(actualOutputFiles));

            if (actualOutputFiles.Count != expectedFilesNumber)
            {
                string message =
                    "Failed to perform analysis. Should be only " +
                    $"{expectedFilesNumber.ToString()} output files but was " +
                    $"{actualOutputFiles.Count.ToString()}.";

                throw new InvalidOperationException(message);
            }
        }

        private static async Task<FileObject> PerformOneIterationOfPhaseTwoAsync(
            AnalysisIterationContextPhaseTwo iterationContext)
        {
            iterationContext.ThrowIfNull(nameof(iterationContext));

            var fileDeleter = new FileDeleter(iterationContext.FinalOutputFile);

            // Contract: the analysis program is located in the same directory as our app.
            var processLaunchContext = ProcessLaunchContext.Create(
                file: iterationContext.Args.AnalysisProgramName,
                args: iterationContext.AnalysisInputArgs,
                showWindow: iterationContext.LaunchContext.ShowAnalysisWindow
            );

            using (var analysisRunner = ProgramRunner.RunProgram(processLaunchContext))
            {
                await analysisRunner.WaitAsync();
            }

            DataObject<OutputFileData> data =
                iterationContext.FileWorker.ReadDataFile(iterationContext.FinalOutputFile);

            return new FileObject(fileDeleter, data);
        }

        private static async Task AwaitAndProcessAsync(Task<FileObject> task,
            Action<FileObject> callback)
        {
            _ = task.ThrowIfNull(nameof(task));
            callback.ThrowIfNull(nameof(callback));

            using FileObject fileObject = await task;
            callback(fileObject);
        }
    }
}

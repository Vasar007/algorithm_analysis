using System;
using System.Collections.Generic;
using System.Linq;
using Acolyte.Assertions;
using AlgorithmAnalysis.DomainLogic.Excel;
using AlgorithmAnalysis.DomainLogic.Files;
using AlgorithmAnalysis.DomainLogic.Processes;

namespace AlgorithmAnalysis.DomainLogic
{
    public sealed class AnalysisPerformer
    {
        private readonly LocalFileWorker _fileWorker;

        private readonly ExcelWrapper _excelWrapper;


        public AnalysisPerformer(string filename)
        {
            _fileWorker = new LocalFileWorker();
            _excelWrapper = new ExcelWrapper(filename);
        }

        public void PerformAnalysis(AnalysisContext context)
        {
            context.ThrowIfNull(nameof(context));

            PerformPhaseOne(context);
            PerformPhaseTwo(context);
        }

        private void PerformPhaseOne(AnalysisContext context)
        {
            ParametersPack args = context.Args;

            using (var holder = ProcessHolder.Start(args.AnalysisProgramName,
                                                    args.PackAsInputArgumentsForPhaseOne()))
            {
                holder.CheckExecutionStatus();
                holder.WaitForExit();
            }

            // Contract: output files are located in the same directory as our app.
            IReadOnlyList<string> finalOutputFilenames = args.GetOutputFilenames();

            if (finalOutputFilenames.Count > 1)
            {
                string message =
                    "Phase 1 of analysis failed: there are more than one output files.";

                throw new InvalidOperationException(message);
            }

            string finalOutputFilename = finalOutputFilenames.First();
            using (DataObject<OutputFileData> data = _fileWorker.ReadDataFile(finalOutputFilename,
                                                                              args))
            {
                IEnumerable<int> operationNumbers = data.GetData(item => item.operationNumber);

                // TODO: add formulas for solution based on beta distribution.
                _excelWrapper.ApplyAnalysisAndSaveData(operationNumbers, context, "Sheet1-1");
            }
            // TODO: delete output files with data.

            // TODO: find appropriate launches number iteratively (part 1 of phase 1).
            // TODO: check H0 hypothesis on calculated launches number (part 2 of phase 1).
        }

        private static void PerformPhaseTwo(AnalysisContext context)
        {
            ParametersPack args = context.Args;

            using (var holder = ProcessHolder.Start(
                       args.AnalysisProgramName,
                       args.PackAsInputArgumentsForPhaseTwo()
                   ))
            {
                holder.WaitForExit();
                holder.CheckExecutionStatus();
            }

            // TODO: launch analysis several times in segment [StartValue, EndValue] with step=Step.
            // TODO: find output files with data and parse them.
            // TODO: save output data to the Excel tables and apply formulas.
            // TODO: delete output files with data.
        }
    }
}

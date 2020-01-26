﻿using System;
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


        public AnalysisPerformer()
        {
            _fileWorker = new LocalFileWorker();
            _excelWrapper = new ExcelWrapper();
        }

        public void PerformAnalysis(ParametersPack args)
        {
            args.ThrowIfNull(nameof(args));

            PerformPhaseOne(args);
            PerformPhaseTwo(args);
        }

        private void PerformPhaseOne(ParametersPack args)
        {
            using (var holder = new ProcessHolder(args))
            {
                holder.WaitForExit();
            }

            // Contract: output files are located in the same directory as our app.
            IReadOnlyList<string> finalOutputFilenames = args.GetOutputFilenames();

            if (finalOutputFilenames.Count > 1)
            {
                throw new InvalidOperationException(
                    "Phase 1 of analysis failed: there are more than one output files."
                );
            }

            string finalOutputFilename = finalOutputFilenames.First();
            DataObject<OutputFileData> data = _fileWorker.ReadDataFile(finalOutputFilename, args);
            IEnumerable<int> operationNumbers = data.GetData(item => item.operationNumber);

            // TODO: save output data to the Excel tables and apply formulas.
            _excelWrapper.SaveDataToExcelFile(operationNumbers);

            // TODO: delete output files with data.

            // TODO: find appropriate launches number iteratively (part 1 of phase 1).
            // TODO: check H0 hypothesis on calculated launches number (part 2 of phase 1).
        }

        private static void PerformPhaseTwo(ParametersPack args)
        {
            // TODO: launch analysis several times in segment [StartValue, EndValue] with step=Step.
            // TODO: find output files with data and parse them.
            // TODO: save output data to the Excel tables and apply formulas.
            // TODO: delete output files with data.
        }
    }
}

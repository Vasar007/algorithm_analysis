using System.Collections.Generic;
using AlgorithmAnalysis.DomainLogic.Files;

namespace AlgorithmAnalysis.DomainLogic.Analysis
{
    internal sealed class AnalysisPhaseTwo : IAnalysis
    {
        private const int PhaseNumber = 2;


        public AnalysisPhaseTwo(string outputExcelFilename)
        {
        }

        #region IAnalysis Implementation

        public AnalysisResult Analyze(AnalysisContext context)
        {
            // TODO: launch analysis several times in segment [StartValue, EndValue] with step=Step.
            // TODO: find output files with data and parse them.
            // TODO: save output data to the Excel tables and apply formulas.
            // TODO: delete output files with data.
            return AnalysisResult.CreateFailure("Phase 2 is not implemented.");
        }

        #endregion
    }
}

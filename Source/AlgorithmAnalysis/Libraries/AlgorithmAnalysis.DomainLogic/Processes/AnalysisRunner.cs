using System;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.DomainLogic.Processes
{
    internal sealed class AnalysisRunner : IDisposable
    {
        private readonly ProcessHolder _processHolder;

        private bool _disposed;


        public AnalysisRunner(ProcessHolder processHolder)
        {
            _processHolder = processHolder.ThrowIfNull(nameof(processHolder));
        }

        public static AnalysisRunner RunAnalysisProgram(string analysisProgramName, string args,
            bool showWindow)
        {
            analysisProgramName.ThrowIfNullOrWhiteSpace(nameof(analysisProgramName));
            args.ThrowIfNullOrWhiteSpace(nameof(args));

            var processHolder = ProcessHolder.Start(
                analysisProgramName, args, showWindow
            );

            return new AnalysisRunner(processHolder);
        }

        public void Wait()
        {
            _processHolder.CheckExecutionStatus();
            _processHolder.WaitForExit();
        }

        #region IDisposable Impelementation

        public void Dispose()
        {
            if (_disposed) return;

            _processHolder.Dispose();

            _disposed = true;
        }

        #endregion
    }
}

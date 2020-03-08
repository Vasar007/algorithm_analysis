using System;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Processes
{
    public sealed class ProgramRunner : IDisposable
    {
        private readonly ProcessHolder _processHolder;

        private bool _disposed;


        public ProgramRunner(ProcessHolder processHolder)
        {
            _processHolder = processHolder.ThrowIfNull(nameof(processHolder));
        }

        public static ProgramRunner RunProgram(string programName, string args,
            bool showWindow)
        {
            programName.ThrowIfNullOrWhiteSpace(nameof(programName));
            args.ThrowIfNullOrWhiteSpace(nameof(args));

            var processHolder = ProcessHolder.Start(
                programName, args, showWindow
            );

            return new ProgramRunner(processHolder);
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

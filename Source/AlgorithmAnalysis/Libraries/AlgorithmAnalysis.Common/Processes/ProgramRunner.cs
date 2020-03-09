using System;
using System.IO;
using System.Threading.Tasks;
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

        public static ProgramRunner RunProgram(FileInfo programName, string args,
            bool showWindow)
        {
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

        public Task WaitAsync()
        {
            _processHolder.CheckExecutionStatus();
            return _processHolder.WaitForExitAsync();
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

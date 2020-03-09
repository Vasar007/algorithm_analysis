using System;
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

        public static ProgramRunner RunProgram(ProcessLaunchContext launchContext)
        {
            var processHolder = ProcessHolder.Start(launchContext);

            return new ProgramRunner(processHolder);
        }

        public void Wait()
        {
            _processHolder.CheckExecutionStatus();
            _processHolder.WaitForExit();
        }

        public void Wait(TimeSpan delay)
        {
            _processHolder.CheckExecutionStatus();
            _processHolder.WaitForExit(delay);
        }

        public Task WaitAsync()
        {
            _processHolder.CheckExecutionStatus();
            return _processHolder.WaitForExitAsync();
        }

        public Task WaitAsync(TimeSpan delay)
        {
            _processHolder.CheckExecutionStatus();
            return _processHolder.WaitForExitAsync(delay);
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

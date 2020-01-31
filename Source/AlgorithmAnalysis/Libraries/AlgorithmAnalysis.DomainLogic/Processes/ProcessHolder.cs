using System;
using System.Diagnostics;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.DomainLogic.Processes
{
    internal sealed class ProcessHolder : IDisposable
    {
        private readonly Process _process;

        private bool _disposed;


        private ProcessHolder(Process process)
        {
            _process = process.ThrowIfNull(nameof(process));
        }

        public static ProcessHolder Start(string filename, string args)
        {
            filename.ThrowIfNullOrWhiteSpace(nameof(filename));
            args.ThrowIfNullOrEmpty(nameof(args));

            return new ProcessHolder(StartProgram(filename, args));
        }

        #region IDisposable Implementation

        public void Dispose()
        {
            if (_disposed) return;

            _process.Dispose();

            _disposed = true;
        }

        #endregion

        public void WaitForExit()
        {
            if (_process.HasExited) return;

            _process.WaitForExit();
        }

        public bool WaitForExit(int milliseconds)
        {
            if (_process.HasExited) return true;

            return _process.WaitForExit(milliseconds);
        }

        public void Kill()
        {
            if (!_process.HasExited)
            {
                _process.Kill();
            }
        }

        public string GetOutput()
        {
            return _process.StandardOutput.ReadToEnd();
        }

        public void CheckExecutionStatus()
        {
            string error = _process.StandardError.ReadToEnd();
            if (_process.ExitCode != ExitCode.EXIT_SUCCESS.AsInt32())
            {
                throw new ApplicationException(error);
            }
        }

        private static Process StartProgram(string filename, string args)
        {
            // Contract: the analysis program is located in the same directory as our app.
            var starterInfo = new ProcessStartInfo(filename, args)
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            return Process.Start(starterInfo);
        }
    }
}

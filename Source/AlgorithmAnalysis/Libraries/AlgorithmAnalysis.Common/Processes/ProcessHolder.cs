using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Processes
{
    public sealed class ProcessHolder : IDisposable
    {
        private readonly Process _process;

        private bool _disposed;


        private ProcessHolder(Process process)
        {
            _process = process.ThrowIfNull(nameof(process));
        }

        public static ProcessHolder Start(FileInfo file, string args, bool showWindow)
        {
            file.ThrowIfNull(nameof(file));
            args.ThrowIfNullOrEmpty(nameof(args));

            return new ProcessHolder(StartProgram(file, args, showWindow));
        }

        #region IDisposable Implementation

        public void Dispose()
        {
            if (_disposed) return;

            // TODO: process should be killed if desktop application will be terminated.
            Kill();
            _process.Dispose();

            _disposed = true;
        }

        #endregion

        public void WaitForExit()
        {
            if (_process.HasExited) return;

            _process.WaitForExit();
        }

        public bool WaitForExit(TimeSpan delay)
        {
            if (_process.HasExited) return true;

            var miliseconds = (int) delay.TotalMilliseconds;
            return _process.WaitForExit(miliseconds);
        }

        public Task WaitForExitAsync()
        {
            if (_process.HasExited) return Task.CompletedTask;

            return _process.WaitForExitAsync();
        }

        public async Task<bool> WaitForExitAsync(TimeSpan delay)
        {
            if (_process.HasExited) return true;

            try
            {
                CancellationTokenSource cts = new CancellationTokenSource(delay);
                Task waitingTask = _process.WaitForExitAsync(cts.Token);

                await waitingTask;
                return waitingTask.IsCompletedSuccessfully;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
        }

        public void Kill()
        {
            if (_process.HasExited) return;

            _process.Kill();
        }

        public void CheckExecutionStatus()
        {
            const int exitSuccess = (int) ExitCode.EXIT_SUCCESS;

            string error = _process.StandardError.ReadToEnd();
            if (_process.ExitCode != exitSuccess)
            {
                throw new ApplicationException($"Exception occured in analysis module: {error}");
            }
        }

        private static Process StartProgram(FileInfo file, string args, bool showWindow)
        {
            // Contract: the analysis program is located in the same directory as our app.
            ProcessStartInfo starterInfo = CreateStartInfo(file, args, showWindow);

            return Process.Start(starterInfo);
        }

        private static ProcessStartInfo CreateStartInfo(FileInfo file, string args, bool showWindow)
        {
            var starterInfo = new ProcessStartInfo(file.FullName, args)
            {
                WorkingDirectory = file.Directory.FullName,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            if (showWindow)
            {
                starterInfo.WindowStyle = ProcessWindowStyle.Normal;
                starterInfo.CreateNoWindow = false;
                return starterInfo;
            }

            starterInfo.WindowStyle = ProcessWindowStyle.Hidden;
            starterInfo.CreateNoWindow = true;
            return starterInfo;
        }
    }
}

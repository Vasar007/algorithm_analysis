﻿using System;
using System.Diagnostics;
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

        public static ProcessHolder Start(string filename, string args, bool showWindow)
        {
            filename.ThrowIfNullOrWhiteSpace(nameof(filename));
            args.ThrowIfNullOrEmpty(nameof(args));

            return new ProcessHolder(StartProgram(filename, args, showWindow));
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

        public bool WaitForExit(int milliseconds)
        {
            if (_process.HasExited) return true;

            return _process.WaitForExit(milliseconds);
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

        private static Process StartProgram(string filename, string args, bool showWindow)
        {
            // Contract: the analysis program is located in the same directory as our app.
            ProcessStartInfo starterInfo = CreateStartInfo(filename, args, showWindow);

            return Process.Start(starterInfo);
        }

        private static ProcessStartInfo CreateStartInfo(string filename, string args,
            bool showWindow)
        {
            var starterInfo = new ProcessStartInfo(filename, args)
            {
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
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
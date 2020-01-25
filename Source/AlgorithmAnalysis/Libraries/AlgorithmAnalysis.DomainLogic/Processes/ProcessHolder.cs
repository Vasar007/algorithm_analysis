using System;
using System.Diagnostics;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.DomainLogic.Processes
{
    internal sealed class ProcessHolder : IDisposable
    {
        private bool _disposed;

        // Initializes through "StartProgram" method in ctor.
        private Process _process = default!;


        public ProcessHolder(ParametersPack args)
        {
            args.ThrowIfNull(nameof(args));

            StartProgram(args);
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
            _process.WaitForExit();
        }

        public void WaitForExit(int milliseconds)
        {
            _process.WaitForExit(milliseconds);
        }

        private void StartProgram(ParametersPack args)
        {
            // Contract: the analysis program is located in the same directory as our app.
            var starterInfo = new ProcessStartInfo(
                args.AnalysisProgramName,
                args.PackAsInputArguments()
            )
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };

            _process = Process.Start(starterInfo);
        }
    }
}

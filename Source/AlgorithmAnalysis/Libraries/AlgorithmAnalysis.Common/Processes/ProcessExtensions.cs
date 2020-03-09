using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Processes
{
    public static class ProcessExtensions
    {
        public static TimeSpan DefaultDelay { get; } = TimeSpan.FromMilliseconds(100);


        public static async Task WaitForExitAsync(this Process process, TimeSpan delay,
            CancellationToken cancellationToken)
        {
            process.ThrowIfNull(nameof(process));

            while (!process.HasExited)
            {
                await Task.Delay(delay, cancellationToken);
            }
        }

        public static Task WaitForExitAsync(this Process process, TimeSpan delay)
        {
            return WaitForExitAsync(process, delay, CancellationToken.None);
        }

        public static Task WaitForExitAsync(this Process process,
            CancellationToken cancellationToken)
        {
            return WaitForExitAsync(process, DefaultDelay, cancellationToken);
        }

        public static Task WaitForExitAsync(this Process process)
        {
            return WaitForExitAsync(process, DefaultDelay, CancellationToken.None);
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Threading
{
    public sealed class SemaphoreScope : IDisposable
    {
        private readonly SemaphoreSlim _semaphore;
        
        private bool _disposed;

        public bool LockAcquired { get; }


        private SemaphoreScope(
            SemaphoreSlim semaphore,
            bool lockAcquired)
        {
            _semaphore = semaphore.ThrowIfNull(nameof(semaphore));
            LockAcquired = lockAcquired;
        }

        public static Task<SemaphoreScope> AcquireAsync(SemaphoreSlim semaphore)
        {
            return AcquireAsync(semaphore, TimeSpan.FromMilliseconds(-1), CancellationToken.None);
        }

        public static Task<SemaphoreScope> AcquireAsync(SemaphoreSlim semaphore,
            CancellationToken cancellationToken)
        {
            return AcquireAsync(semaphore, TimeSpan.FromMilliseconds(-1), cancellationToken);
        }

        public static Task<SemaphoreScope> AcquireAsync(SemaphoreSlim semaphore, TimeSpan timeout)
        {
            return AcquireAsync(semaphore, timeout, CancellationToken.None);
        }

        public static async Task<SemaphoreScope> AcquireAsync(SemaphoreSlim semaphore,
            TimeSpan timeout, CancellationToken cancellationToken)
        {
            semaphore.ThrowIfNull(nameof(semaphore));

            // If the timeout is set to -1 milliseconds, the semaphore's method waits indefinitely.
            bool lockAcquired = await semaphore.WaitAsync(timeout, cancellationToken);
            return new SemaphoreScope(semaphore, lockAcquired);
        }

        public void ThrowIfLockNotAcquired()
        {
            if (!LockAcquired)
            {
                throw new SynchronizationLockException("Semaphore lock was not acquired.");
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_disposed) return;

            if (LockAcquired)
            {
                _semaphore.Release();
            }

            _disposed = true;
        }

        #endregion
    }
}

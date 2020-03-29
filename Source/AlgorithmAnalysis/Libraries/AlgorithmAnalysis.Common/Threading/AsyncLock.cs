using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlgorithmAnalysis.Common.Threading
{
    public sealed class AsyncLock : IDisposable
    {
        private readonly SemaphoreSlim _semaphore;


        public AsyncLock()
        {
            _semaphore = new SemaphoreSlim(initialCount: 1, maxCount: 1);
        }

        public Task<IDisposable> EnterAsync()
        {
            return EnterAsync(CancellationToken.None);
        }

        public async Task<IDisposable> EnterAsync(CancellationToken cancellationToken)
        {
            return await SemaphoreScope.AcquireAsync(_semaphore, cancellationToken);
        }

        #region IDisposable Members

        private bool _disposed;

        public void Dispose()
        {
            if (_disposed) return;

            _semaphore.Dispose();

            _disposed = true;
        }

        #endregion
    }
}

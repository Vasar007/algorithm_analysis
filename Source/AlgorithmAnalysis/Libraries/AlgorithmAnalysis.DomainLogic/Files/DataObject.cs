using System;
using System.Collections.Generic;
using System.Linq;
using Acolyte.Assertions;
using FileHelpers;

namespace AlgorithmAnalysis.DomainLogic.Files
{
    internal sealed class DataObject<TSource> : IDisposable
        where TSource : class
    {
        private readonly FileHelperAsyncEngine<TSource> _engine;

        private readonly string _dataFilename;

        private bool _disposed;


        public DataObject(FileHelperAsyncEngine<TSource> engine, string dataFilename)
        {
            _engine = engine.ThrowIfNull(nameof(engine));
            _dataFilename = dataFilename.ThrowIfNull(nameof(dataFilename));
        }

        #region IDisposable Implementation

        public void Dispose()
        {
            if (_disposed) return;

            if (_engine is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _disposed = true;
        }

        #endregion

        public IEnumerable<TSource> GetData()
        {
            using (_engine.BeginReadFile(_dataFilename))
            {
                // The engine is IEnumerable.
                foreach (TSource item in _engine)
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<TResult> GetData<TResult>(Func<TSource, TResult> selector)
        {
            selector.ThrowIfNull(nameof(selector));

            using (_engine.BeginReadFile(_dataFilename))
            {
                // The engine is IEnumerable.
                foreach (TResult item in _engine.Select(selector))
                {
                    yield return item;
                }
            }
        }
    }

    internal static class DataObject
    {
        public static DataObject<TSource> Create<TSource>(
            this FileHelperAsyncEngine<TSource> engine, string dataFilename)
        where TSource : class
        {
            return new DataObject<TSource>(engine, dataFilename);
        }
    }
}

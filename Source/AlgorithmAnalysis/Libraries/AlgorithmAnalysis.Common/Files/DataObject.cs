using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Acolyte.Assertions;
using FileHelpers;

namespace AlgorithmAnalysis.Common.Files
{
    public sealed class DataObject<TSource> : IDisposable
        where TSource : class
    {
        private readonly FileHelperAsyncEngine<TSource> _engine;

        private readonly FileInfo _dataFile;

        private bool _disposed;


        public DataObject(FileHelperAsyncEngine<TSource> engine, FileInfo dataFile)
        {
            _engine = engine.ThrowIfNull(nameof(engine));
            _dataFile = dataFile.ThrowIfNull(nameof(dataFile));
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
            using (_engine.BeginReadFile(_dataFile.FullName))
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

            using (_engine.BeginReadFile(_dataFile.FullName))
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
            this FileHelperAsyncEngine<TSource> engine, FileInfo dataFile)
            where TSource : class
        {
            return new DataObject<TSource>(engine, dataFile);
        }
    }
}

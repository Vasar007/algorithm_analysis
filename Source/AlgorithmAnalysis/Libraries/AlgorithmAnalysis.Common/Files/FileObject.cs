using System;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Files
{
    public sealed class FileObject : IDisposable
    {
        private readonly FileHolder _fileHolder;

        private bool _disposed;

        public DataObject<OutputFileData> Data { get; }


        public FileObject(FileHolder fileHolder, DataObject<OutputFileData> dataObject)
        {
            _fileHolder = fileHolder.ThrowIfNull(nameof(fileHolder));
            Data = dataObject.ThrowIfNull(nameof(dataObject));
        }

        public void Dispose()
        {
            if (_disposed) return;

            _fileHolder.Dispose();
            Data.Dispose();

            _disposed = true;
        }
    }
}

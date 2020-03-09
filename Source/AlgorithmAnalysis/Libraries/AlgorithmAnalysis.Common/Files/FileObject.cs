using System;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Files
{
    public sealed class FileObject : IDisposable
    {
        private readonly FileDeleter _fileDeleter;

        private bool _disposed;

        public DataObject<OutputFileData> Data { get; }


        public FileObject(FileDeleter fileDeleter, DataObject<OutputFileData> dataObject)
        {
            _fileDeleter = fileDeleter.ThrowIfNull(nameof(fileDeleter));
            Data = dataObject.ThrowIfNull(nameof(dataObject));
        }

        public void Dispose()
        {
            if (_disposed) return;

            _fileDeleter.Dispose();
            Data.Dispose();

            _disposed = true;
        }
    }
}

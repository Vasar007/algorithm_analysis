using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.DomainLogic.Files
{
    internal sealed class FileHolder : IDisposable
    {
        private readonly IEnumerable<string> _dataFilenames;

        private bool _disposed;


        public FileHolder(IEnumerable<string> dataFilenames)
        {
            _dataFilenames = dataFilenames.ThrowIfNull(nameof(dataFilenames));
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (_disposed) return;

            TryDeleteOutputDataFiles();

            _disposed = true;
        }

        #endregion

        private void TryDeleteOutputDataFiles()
        {
            try
            {
                foreach (string dataFilename in _dataFilenames)
                {
                    if (File.Exists(dataFilename))
                    {
                        File.Delete(dataFilename);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred: {ex}");
            }
        }
    }
}

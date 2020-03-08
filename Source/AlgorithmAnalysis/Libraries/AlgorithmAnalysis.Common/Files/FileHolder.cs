using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common.Files
{
    public sealed class FileHolder : IDisposable
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
            foreach (string dataFilename in _dataFilenames)
            {
                TryDeleteDataFile(dataFilename);
            }
        }

        private static void TryDeleteDataFile(string dataFilename)
        {
            try
            {
                if (File.Exists(dataFilename))
                {
                    File.Delete(dataFilename);
                }
            }
            catch (Exception ex)
            {
                string message =
                    $"Failed to delete output data file '{dataFilename}':" +
                    $"{Environment.NewLine}{ex}";

                Debug.WriteLine(message);
                Trace.WriteLine(message);
            }
        }
    }
}

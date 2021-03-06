﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Acolyte.Assertions;
using Acolyte.Common;

namespace AlgorithmAnalysis.Common.Files
{
    public sealed class FileDeleter : IDisposable
    {
        private readonly IEnumerable<FileInfo> _dataFiles;

        private bool _disposed;


        public FileDeleter(IEnumerable<FileInfo> dataFiles)
        {
            _dataFiles = dataFiles.ThrowIfNull(nameof(dataFiles));
        }

        public FileDeleter(FileInfo dataFile)
            : this(dataFile.ToEnumerable())
        {
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
            foreach (FileInfo dataFile in _dataFiles)
            {
                TryDeleteDataFile(dataFile);
            }
        }

        private static void TryDeleteDataFile(FileInfo dataFile)
        {
            try
            {
                // Use static File methods because FileInfo can have out-of-date state.
                if (File.Exists(dataFile.FullName))
                {
                    File.Delete(dataFile.FullName);
                }
            }
            catch (Exception ex)
            {
                string message =
                    $"Failed to delete output data file '{dataFile}':" +
                    $"{Environment.NewLine}{ex}";

                Debug.WriteLine(message);
                Trace.WriteLine(message);
            }
        }
    }
}

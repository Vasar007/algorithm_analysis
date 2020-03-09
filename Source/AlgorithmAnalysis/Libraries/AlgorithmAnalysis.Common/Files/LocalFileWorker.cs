using System;
using System.IO;
using Acolyte.Assertions;
using FileHelpers;

namespace AlgorithmAnalysis.Common.Files
{
    public sealed class LocalFileWorker
    {
        public LocalFileWorker()
        {
        }

        public DataObject<OutputFileData> ReadDataFile(FileInfo dataFile)
        {
            dataFile.ThrowIfNull(nameof(dataFile));

            if (!File.Exists(dataFile.FullName))
            {
                throw new ArgumentException(
                    $"File '{dataFile}' does not exist.", nameof(dataFile)
                );
            }

            // Output data file contains exactly "LaunchesNumber" values.
            var engine = new FileHelperAsyncEngine<OutputFileData>();
            return DataObject.Create(engine, dataFile);
        }
    }
}
